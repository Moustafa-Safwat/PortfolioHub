using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PortfolioHub.Achievements.Usecases.Certificate;

namespace PortfolioHub.Achievements.Endpoints.Certificate;
internal sealed class Delete(
    ISender sender
    ) : Endpoint<DeleteCertificateRequest, Result>
{
    public override void Configure()
    {
        Delete("/certificate/{Id}");
        Roles("admin");
    }
    public override async Task HandleAsync(DeleteCertificateRequest req, CancellationToken ct)
    {
        var deleteCertificateCommand = new DeleteCertificateCommand(Guid.Parse(req.Id));

        var deleteResult = await sender.Send(deleteCertificateCommand, ct);

        if (!deleteResult.IsSuccess)
        {
            await SendAsync(deleteResult, StatusCodes.Status400BadRequest, ct);
            return;
        
        }
        var successObj = Result.SuccessWithMessage("The certificate entry was deleted successfully.");

        await SendAsync(successObj, StatusCodes.Status200OK, ct);
    }
}
