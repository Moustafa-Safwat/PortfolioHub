using Ardalis.Result;
using MediatR;

namespace PortfolioHub.Users.Usecases.Role;

internal sealed record AddRoleCommand(
    string Name
    ) : IRequest<Result<Guid>>;
