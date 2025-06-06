using FastEndpoints;
using MediatR;
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
        var createUserCommand = new CreateUserCommand(req.UserName,
              req.Email,
              req.Password);

        var createUserResult = await sender.Send(createUserCommand, ct);

        if (createUserResult.IsSuccess)
        {
            await SendCreatedAtAsync<Create>($"/user/{createUserResult.Value}",
                createUserResult.Value,
                cancellation: ct);
        }
        else
        {
            await SendErrorsAsync();
        }
    }
}
