using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Users.Domain.Entities;

internal sealed class Info : BaseEntity
{
    public string InfoKey { get; private set; } = string.Empty;
    public string InfoValue { get; private set; } = string.Empty;

    // Parameterless constructor for EF Core
    public Info() { }
    public Info(Guid id, string infoKey, string infoValue)
    {
        Id = Guard.Against.Default(id);
        InfoKey = Guard.Against.NullOrEmpty(infoKey);
        InfoValue = Guard.Against.NullOrEmpty(infoValue);
    }

    public void UpdateInfo(string infoKey, string infoValue)
    {
        UpdateInfoKey(infoKey);
        UpdateInfoValue(infoValue);
    }
    public void UpdateInfoKey(string infoKey)
        => InfoKey = Guard.Against.NullOrEmpty(infoKey);
    public void UpdateInfoValue(string infoValue)
        => InfoValue = Guard.Against.NullOrEmpty(infoValue);
}
