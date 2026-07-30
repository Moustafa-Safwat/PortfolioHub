using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.SharedKernal.Domain.Entities;

public abstract class DeletionEntity : AuditedEntity, ISoftDeletable
{
    public DateTime? DeletedAtUtc { get; protected set; }
    public Guid? DeletedBy { get; protected set; }
    public bool IsDeleted { get; protected set; } = false;

    public virtual void MarkAsDeleted(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;
        DeletedBy = Guard.Against.Default(deletedBy);
    }

    public virtual void MarkAsRestored()
    {
        if (!IsDeleted) return;

        IsDeleted = false;
        DeletedAtUtc = null;
        DeletedBy = null;
    }
}