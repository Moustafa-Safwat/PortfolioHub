namespace PortfolioHub.SharedKernal.Domain.Interfaces;

public interface IUpdateAuditable
{
    DateTime? UpdatedAtUtc { get; }

    Guid? UpdatedBy { get; }
}
