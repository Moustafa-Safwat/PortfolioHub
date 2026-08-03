using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.SharedKernal.Domain.Entities;

public abstract class AuditedEntity : CreationAuditedEntity, IUpdateAuditable
{
    public DateTime? UpdatedAtUtc { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }

    public virtual void MarkAsUpdated(Guid updatedBy)
    {
        UpdatedAtUtc = DateTime.UtcNow;
        UpdatedBy = Guard.Against.Default(updatedBy);
    }
}
