using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using PortfolioHub.Users.Domain.Entities.Users;
using ValidBuild.Sharedkernal.Domain.CQRS;

namespace PortfolioHub.Users.Usecases.User.GetByIds;

internal sealed class GetUsersByIdQueryHandler
(
    UserManager<ApplicationUser> userManager
) : IQueryHandler<GetUsersByIdQuery, IEnumerable<GetUserDto>>
{
    public async Task<Result<IEnumerable<GetUserDto>>> Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new List<GetUserDto>();
        foreach (var userId in request.UserIds)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                continue; // skip user if not exists
            var getUserDto = new GetUserDto(user.Id, user.FirstName, user.LastName, user.UserName!, user.Email!);
            result.Add(getUserDto);
        }

        if (!result.Any())
            return Result.NotFound();

        return result;
    }
}
