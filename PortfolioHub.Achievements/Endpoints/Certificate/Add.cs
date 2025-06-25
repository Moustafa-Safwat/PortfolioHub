using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Achievements.Usecases.Certificate;

namespace PortfolioHub.Achievements.Endpoints.Certificate;
internal sealed class Add(
    ISender sender
    ) : Endpoint<AddCertificateRequest, Result<Guid>>
{
    public override void Configure()
    {
        Post("/certificate");
        Roles("admin");
    }

    public override async Task HandleAsync(AddCertificateRequest req, CancellationToken ct)
    {
        var addCertificateCommand = new AddCertificateCommand
            (
                req.Name,
                req.Issuer,
                req.Date
            );
        var addCertificateResult = await sender.Send(addCertificateCommand, ct);

        if (!addCertificateResult.IsSuccess)
        {
            await SendAsync(addCertificateResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(addCertificateResult, StatusCodes.Status201Created, ct);
    }
}
