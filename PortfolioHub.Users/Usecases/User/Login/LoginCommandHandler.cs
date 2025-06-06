using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed class LoginCommandHandler(
    UserManager<IdentityUser> userManager,
    ILogger logger,
    CreateUserToken createUserToken
    ) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            logger.Error("User {@UserName} not found", request.UserName);
            return Result.NotFound($"User {request.UserName} not found");
        }
        var isPassValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPassValid)
        {
            logger.Error("Invalid password for user {@UserName}", request.UserName);
            return Result.Unauthorized("Invalid password");
        }
        // Additional logic for successful login can be added here
        var tokenResult = await createUserToken.CreateToken(user, cancellationToken);
        if (tokenResult.IsSuccess)
        {
            logger.Information("User {@UserId} logged in successfully", user.Id);
            return Result.Success(tokenResult.Value);
        }
        else
        {
            return tokenResult;
        }
    }
}
