using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using MediatR;
using PortfolioHub.Users.Usecases.Role;

namespace PortfolioHub.Users.Endpoints.Role;

internal sealed class Add(
    ISender sender
    ) : Endpoint<AddRoleReq, AddRoleRes>
{
    public override void Configure()
    {
        Post("/role");
        Roles("admin");
    }

    public override async Task HandleAsync(AddRoleReq req, CancellationToken ct)
    {
        var command = new AddRoleCommand(req.Name);
        var result = await sender.Send(command, ct);

        if (!result.IsSuccess)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var response = new AddRoleRes(result.Value, req.Name);
        await SendOkAsync(response, ct);
    }
}
