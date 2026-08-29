using System.ComponentModel.DataAnnotations;

namespace CompanyProfile.Api.Filters;

public sealed class ValidationFilter<T> : IEndpointFilter
    where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var model = context.Arguments
            .OfType<T>()
            .FirstOrDefault();

        if (model is null)
        {
            return await next(context);
        }

        var validationContext = new ValidationContext(model);

        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(
            model,
            validationContext,
            validationResults,
            validateAllProperties: true);

        if (!isValid)
        {
            var errors = validationResults
                .SelectMany(result =>
                {
                    var members = result.MemberNames.Any()
                        ? result.MemberNames
                        : new[] { string.Empty };

                    return members.Select(member => new
                    {
                        Member = member,
                        Error = result.ErrorMessage ?? "Invalid value."
                    });
                })
                .GroupBy(x => x.Member)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(x => x.Error)
                        .Distinct()
                        .ToArray());

            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}

