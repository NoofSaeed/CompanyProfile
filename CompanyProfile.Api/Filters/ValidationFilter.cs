namespace CompanyProfile.Api.Filters;

using CompanyProfile.Shared.Resources;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public sealed class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IStringLocalizer<ValidationResources> _localizer;

    public ValidationFilter(IStringLocalizer<ValidationResources> localizer)
    {
        _localizer = localizer;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
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

                var message = GetLocalizedMessage(property, result.ErrorMessage);

                if (!errors.TryGetValue(member, out var existing))
                {
                    errors[member] = [message];
                }
                else if (!existing.Contains(message))
                {
                    errors[member] = [.. existing, message];
                }
            }
        }

        return Results.ValidationProblem(errors);
    }

    private string GetLocalizedMessage(PropertyInfo? property, string? rawErrorMessage)
    {
        if (string.IsNullOrEmpty(rawErrorMessage))
            rawErrorMessage = "Validation.Invalid";

        var displayName = GetDisplayName(property);

        var localizedTemplate = _localizer[rawErrorMessage];

        if (localizedTemplate.ResourceNotFound)
            return rawErrorMessage;

        var attribute = property?
            .GetCustomAttributes<ValidationAttribute>()
            .FirstOrDefault(x => x.ErrorMessage == rawErrorMessage);

        return attribute switch
        {
            StringLengthAttribute stringLength =>
                string.Format(localizedTemplate.Value, displayName, stringLength.MaximumLength),

            _ => string.Format(localizedTemplate.Value, displayName)
        };
    }

    private string GetDisplayName(PropertyInfo? property)
    {
        if (property is null)
            return string.Empty;

        var displayAttribute = property
            .GetCustomAttributes<DisplayAttribute>()
            .FirstOrDefault();

        var keyToLocate = displayAttribute?.Name ?? $"Validation.{property.Name}";

        var localizedName = _localizer[keyToLocate];

        return localizedName.ResourceNotFound ? property.Name : localizedName.Value;
    }
}