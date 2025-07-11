using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed class DeleteCertificateCommandHandler(
    IEntityRepo<Domain.Certificate> certificateRepo
    ) : IRequestHandler<DeleteCertificateCommand, Result>
{
    public async Task<Result> Handle(DeleteCertificateCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await certificateRepo.DeleteAsync(request.Id, cancellationToken);
        if (!deleteResult.IsSuccess)
            return deleteResult;

        var effectedRowsResult = await certificateRepo.SaveChangesAsync(cancellationToken);
        if (!effectedRowsResult.IsSuccess)
            return effectedRowsResult;

        return Result.SuccessWithMessage("The certificate entry was deleted successfully.");
    }
}