using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.SharedKernal.Domain.Entities;

public abstract class CreationAuditedEntity : BaseEntity, ICreationAuditable
{
    public DateTime CreatedAtUtc { get; protected set; }
    public Guid CreatedBy { get; protected set; }

    protected virtual void MarkAsCreated(Guid createdBy)
    {
        CreatedAtUtc = DateTime.UtcNow;
        CreatedBy = Guard.Against.Default(createdBy);
    }
}
