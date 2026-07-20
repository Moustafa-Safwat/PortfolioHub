using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Usecases.User.Create;

namespace PortfolioHub.Users.Endpoints.User;

internal class Create(ISender sender)
    : Endpoint<CreateUserReq>
{
    public override void Configure()
    {
        Post("/user");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserReq req, CancellationToken ct)
    {
        // Create the command with validated role
        var createUserCommand = new CreateUserCommand(
            Email: req.Email,
            Password: req.Password,
            FirstName: req.FirstName,
            LastName: req.LastName,
            Role: nameof(ApplicationUserRoles.User).ToLower(),
            CompanyName: req.CompanyName
        );

        var createUserResult = await sender.Send(createUserCommand, ct);

        if (createUserResult.IsSuccess)
        {
            var userId = createUserResult.Value;
            await SendCreatedAtAsync<Create>(
                $"/users/{userId}",
                createUserResult,
                cancellation: ct);
        }
        else
        {
            await SendAsync(createUserResult, StatusCodes.Status400BadRequest, ct);
        }
    }
}
