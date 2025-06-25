using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Achievements.Usecases.Certificate;

namespace PortfolioHub.Achievements.Endpoints.Certificate;
internal sealed class Get(
    ISender sender
    ) : Endpoint<GetCertificateReq, Result<IEnumerable<CertificateGetDto>>>
{
    public override void Configure()
    {
        Get("/certificate");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCertificateReq req, CancellationToken ct)
    {
        var getCertificateQuery = new GetCertificateQuery(req.Page, req.PageSize);

        var getCertificateResult = await sender.Send(getCertificateQuery, ct);

        if (!getCertificateResult.IsSuccess)
        {
            await SendAsync(getCertificateResult, StatusCodes.Status400BadRequest, ct);
            return;
        }

        await SendAsync(getCertificateResult, StatusCodes.Status200OK, ct);
    }
}
