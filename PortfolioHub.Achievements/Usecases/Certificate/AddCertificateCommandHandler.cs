using Ardalis.Result;
using MediatR;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using Serilog;

namespace PortfolioHub.Achievements.Usecases.Certificate;

internal sealed class AddCertificateCommandHandler(
    IEntityRepo<Domain.Certificate> certificateRepo,
    ILogger logger
    ) : IRequestHandler<AddCertificateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddCertificateCommand request, CancellationToken cancellationToken)
    {
        logger.Information("Handling AddCertificateCommand for Name: {Name}, Issuer: {Issuer}, Date: {Date}", request.Name, request.Issuer, request.Date);

        var certificate = new Domain.Certificate
            (
                Guid.NewGuid(),
                request.Name,
                request.Issuer,
                request.Date
            );

        logger.Debug("Created Certificate entity with Id: {Id}", certificate.Id);

        var addResult = await certificateRepo.AddAsync(certificate, cancellationToken);
        if (!addResult.IsSuccess)
        {
            logger.Error("Failed to add Certificate entity. Errors: {Errors}", addResult.Errors);
            return addResult;
        }

        var effectedRowsResult = await certificateRepo.SaveChangesAsync(cancellationToken);
        if (!effectedRowsResult.IsSuccess)
        {
            logger.Error("Failed to save changes after adding Certificate. Errors: {Errors}", effectedRowsResult.Errors);
            return effectedRowsResult;
        }

        logger.Information("Successfully added Certificate with Id: {Id}", certificate.Id);
        return Result<Guid>.Success(certificate.Id);
    }
}