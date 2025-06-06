using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace PortfolioHub.Users.Usecases.User.Create;

internal sealed class CreateUserCommandHandler (
    UserManager<IdentityUser> userManager,
    ILogger logger
    )
    : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        IdentityUser identityUser = new IdentityUser
        {
            UserName = request.UserName,
            Email = request.Email,
        };
        var identityResult =await userManager.CreateAsync(identityUser, request.Password);

        if (!identityResult.Succeeded)
        {
            logger.Information("Failed to create user {@UserName} with email {@Email}. Errors: {@Errors}",
                request.UserName, request.Email, identityResult.Errors.Select(e => e.Description));
            return Result.Error(new ErrorList(identityResult.Errors.Select(e => e.Description).ToArray()));
        }

        logger.Information("User {@UserName} with email {@Email} has been created successfully",
            request.UserName, request.Email);
        return Result.Success(Guid.Parse(identityUser.Id));
    }
}