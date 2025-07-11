using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed class AddCertificateCommandHandler(
    IEntityRepo<Domain.Certificate> certificateRepo
    ) : IRequestHandler<AddCertificateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddCertificateCommand request, CancellationToken cancellationToken)
    {
        var certificate = new Domain.Certificate
            (
                Guid.NewGuid(),
                request.Name,
                request.Issuer,
                request.Date
            );

        var addResult = await certificateRepo.AddAsync(certificate, cancellationToken);
        if (!addResult.IsSuccess)
            return addResult;

        var effectedRowsResult = await certificateRepo.SaveChangesAsync(cancellationToken);
        if (!effectedRowsResult.IsSuccess)
            return effectedRowsResult;

        return Result<Guid>.Success(certificate.Id);
    }
}