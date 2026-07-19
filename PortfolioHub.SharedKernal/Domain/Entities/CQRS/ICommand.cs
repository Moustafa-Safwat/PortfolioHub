using Ardalis.Result;
using MediatR;

namespace ValidBuild.Sharedkernal.Domain.CQRS;

/// <summary>
/// Represents a command that returns a <see cref="Result"/>.
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// Represents a command that returns a <see cref="Result{TResult}"/>.
/// </summary>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ICommand<TResult> : IRequest<Result<TResult>>
{
}