using Ardalis.Result;
using MediatR;
using PortfolioHub.Achievements.Domain;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed record AddCertificateCommand(
    string Name,
    string Issuer,
    DateTime Date
    ) : IRequest<Result<Guid>>;

internal sealed class AddCertificateCommandHandler (
    IEntityRepo<Domain.Certificate> certificateRepo
    ): IRequestHandler<AddCertificateCommand, Result<Guid>>
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

        await certificateRepo.AddAsync(certificate, cancellationToken);
        var effectedRowsResult = await certificateRepo.SaveChangesAsync(cancellationToken);
        if (!effectedRowsResult.IsSuccess)
        {
            return effectedRowsResult;
        }
        return Result<Guid>.Success(certificate.Id);
    }
}