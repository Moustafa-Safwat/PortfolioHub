namespace PortfolioHub.SharedKernal.Domain.Interfaces;

public interface ICreationAuditable
{
    DateTime CreatedAtUtc { get; }

    Guid CreatedBy { get; }
}
