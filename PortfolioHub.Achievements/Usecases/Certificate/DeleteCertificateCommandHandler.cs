using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed class DeleteCertificateCommandHandler(
    IEntityRepo<Domain.Certificate> certificateRepo,
    ILogger logger
    ) : IRequestHandler<DeleteCertificateCommand, Result>
{
    public async Task<Result> Handle(DeleteCertificateCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Attempting to delete certificate with Id: {CertificateId}", request.Id);

        var deleteResult = await certificateRepo.DeleteAsync(request.Id, cancellationToken);
        if (!deleteResult.IsSuccess)
        {
            logger.Warning("Failed to delete certificate with Id: {CertificateId}. Reason: {Reason}", request.Id, deleteResult.Errors);
            return deleteResult;
        }

        var effectedRowsResult = await certificateRepo.SaveChangesAsync(cancellationToken);
        if (!effectedRowsResult.IsSuccess)
        {
            logger.Error("Failed to save changes after deleting certificate with Id: {CertificateId}. Reason: {Reason}", request.Id, effectedRowsResult.Errors);
            return effectedRowsResult;
        }

        logger.Information("Successfully deleted certificate with Id: {CertificateId}", request.Id);
        return Result.SuccessWithMessage("The certificate entry was deleted successfully.");
    }
}