using Ardalis.Result;
using FluentValidation.Results;

namespace PortfolioHub.Web.Infra;

public static class ValidationFailureExtensions
{
    public static Result ToArdalisResult(this IEnumerable<ValidationFailure> failures)
    {
        ArgumentNullException.ThrowIfNull(failures);

        var errors = failures
            .Where(failure => failure is not null)
            .Select(ToArdalisValidationError)
            .ToArray();

        return errors.Length == 0
            ? Result.Success()
            : Result.Invalid(errors);
    }

    public static ValidationError ToArdalisValidationError(this ValidationFailure failure)
    {
        ArgumentNullException.ThrowIfNull(failure);

        return new ValidationError
        {
            Identifier = string.IsNullOrWhiteSpace(failure.PropertyName)
                ? "General"
                : failure.PropertyName,

            ErrorMessage = string.IsNullOrWhiteSpace(failure.ErrorMessage)
                ? "The supplied value is invalid."
                : failure.ErrorMessage,

            ErrorCode = string.IsNullOrWhiteSpace(failure.ErrorCode)
                ? "ValidationError"
                : failure.ErrorCode,

            Severity = failure.Severity switch
            {
                FluentValidation.Severity.Info =>
                    ValidationSeverity.Info,

                FluentValidation.Severity.Warning =>
                    ValidationSeverity.Warning,

                _ => ValidationSeverity.Error
            }
        };
    }
}
