using Ardalis.Result;
using MediatR;

namespace ValidBuild.Sharedkernal.Domain.CQRS;

/// <summary>
/// Defines a handler for processing queries that return a result.
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the query.</typeparam>
public interface IQueryHandler<TQuery, TResult>
    : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQuery<TResult>
{
}
