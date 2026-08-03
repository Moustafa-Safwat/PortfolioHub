using Ardalis.Result;
using MediatR;

namespace ValidBuild.Sharedkernal.Domain.CQRS;

/// <summary>
/// Represents a query interface that returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the query.</typeparam>
public interface IQuery<TResult> : IRequest<Result<TResult>>
{

}
