namespace PortfolioHub.SharedKernal.Domain.Interfaces;

public interface ISoftDeletable
{
    bool IsDeleted { get; }

    DateTime? DeletedAtUtc { get; }

    Guid? DeletedBy { get; }
}
