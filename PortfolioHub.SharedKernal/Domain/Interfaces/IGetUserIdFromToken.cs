namespace PortfolioHub.SharedKernal.Domain.Interfaces;

public interface IGetUserIdFromToken
{
    Guid GetOptionalUserId();
}
