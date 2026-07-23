using Ardalis.Result;
using PortfolioHub.Users.Domain.Entities.Users;

namespace PortfolioHub.Users.Domain.Interfaces;

internal interface IValidUser
{
    Task<Result<ApplicationUser>> IsValidUserAsync(ApplicationUser user, CancellationToken cancellationToken);
}
