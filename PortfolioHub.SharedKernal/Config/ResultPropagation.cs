using Ardalis.Result;

namespace PortfolioHub.SharedKernal.Config;

public static class ResultPropagation
{
    public static Result<U> PropagateFailure<T, U>(this Result<T> result) =>
        result.Status switch
        {
            ResultStatus.Error => Result<U>.Error(new ErrorList(result.Errors)),
            ResultStatus.Forbidden => Result<U>.Forbidden(result.Errors.ToArray()),
            ResultStatus.Unauthorized => Result<U>.Unauthorized(result.Errors.ToArray()),
            ResultStatus.Invalid => Result<U>.Invalid(result.ValidationErrors),
            ResultStatus.NotFound => Result<U>.NotFound(result.Errors.ToArray()),
            ResultStatus.NoContent => Result<U>.NoContent(),
            ResultStatus.Conflict => Result<U>.Conflict(result.Errors.ToArray()),
            ResultStatus.CriticalError => Result<U>.CriticalError(result.Errors.ToArray()),
            ResultStatus.Unavailable => Result<U>.Unavailable(result.Errors.ToArray()),
            _ => throw new InvalidOperationException($"Unexpected ResultStatus: {result.Status}")
        };

    public static Result PropagateFailure<T>(this Result<T> result) =>
       result.Status switch
       {
           ResultStatus.Error => Result.Error(new ErrorList(result.Errors)),
           ResultStatus.Forbidden => Result.Forbidden(result.Errors.ToArray()),
           ResultStatus.Unauthorized => Result.Unauthorized(result.Errors.ToArray()),
           ResultStatus.Invalid => Result.Invalid(result.ValidationErrors),
           ResultStatus.NotFound => Result.NotFound(result.Errors.ToArray()),
           ResultStatus.NoContent => Result.NoContent(),
           ResultStatus.Conflict => Result.Conflict(result.Errors.ToArray()),
           ResultStatus.CriticalError => Result.CriticalError(result.Errors.ToArray()),
           ResultStatus.Unavailable => Result.Unavailable(result.Errors.ToArray()),
           _ => throw new InvalidOperationException($"Unexpected ResultStatus: {result.Status}")
       };
}
