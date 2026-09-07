using CompanyProfile.Api.Resources;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CompanyProfile.Api.Filters;

public sealed class ValidationFilter<T> : IEndpointFilter
    where T : class
{
    private readonly IStringLocalizer<ValidationResources> _localizer;

    public ValidationFilter(
        IStringLocalizer<ValidationResources> localizer)
    {
        _localizer = localizer;
    }


    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var model = context.Arguments
            .OfType<T>()
            .FirstOrDefault();

        if (model is null)
            return await next(context);

        var validationContext = new ValidationContext(model);

        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            model,
            validationContext,
            validationResults,
            validateAllProperties: true);

        if (isValid)
            return await next(context);

        var errors = new Dictionary<string, string[]>();

        foreach (var result in validationResults)
        {
            var members = result.MemberNames.Any()
                ? result.MemberNames
                : [string.Empty];

            foreach (var member in members)
            {
                var property = typeof(T).GetProperty(member);

                var errorKey = result.ErrorMessage
                    ?? "Validation.Invalid";

                var message = GetLocalizedMessage(
                    property,
                    errorKey);

                if (!errors.TryGetValue(member, out var existing))
                {
                    errors[member] = [message];
                }
                else if (!existing.Contains(message))
                {
                    errors[member] =
                    [
                        .. existing,
                        message
                    ];
                }
            }
        }

        return Results.ValidationProblem(errors);
    }

    private string GetLocalizedMessage(
        PropertyInfo? property,
        string errorKey)
    {
        var localizedTemplate = _localizer[errorKey];
       
        if (localizedTemplate.ResourceNotFound)
            return "not found";

        var displayName = GetDisplayName(property);

        var attribute = property?
            .GetCustomAttributes<ValidationAttribute>()
            .FirstOrDefault(x => x.ErrorMessage == errorKey);

        return attribute switch
        {
            StringLengthAttribute stringLength =>
                string.Format(
                    localizedTemplate.Value,
                    displayName,
                    stringLength.MaximumLength),

            RequiredAttribute or EmailAddressAttribute =>
                string.Format(
                    localizedTemplate.Value,
                    displayName),

            _ => localizedTemplate.Value
        };
    }

    private string GetDisplayName(PropertyInfo? property)
    {
        if (property is null)
            return string.Empty;

        var displayAttribute = property
            .GetCustomAttributes<DisplayAttribute>()
            .FirstOrDefault();

        if (displayAttribute is null ||
            string.IsNullOrWhiteSpace(displayAttribute.Name))
        {
            return property.Name;
        }

        var localizedName =
            _localizer[displayAttribute.Name];

        return localizedName.ResourceNotFound
            ? property.Name
            : "not found";
        ;
    }
}