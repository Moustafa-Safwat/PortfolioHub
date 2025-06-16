namespace PortfolioHub.Users.Domain.Entities;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;
using PortfolioHub.SharedKernal.Domain.Entities;

internal class RefreshToken :BaseEntity
{
    public string UserId { get; private set; } = null!;
    public string HasedToken { get; private set; } = null!;
    public string Device { get; private set; } = null!;
    public string IpAddress { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    // Navigation property
    public IdentityUser? User { get; private set; } = null!;

    // Parameterless constructor for EF Core
    public RefreshToken() { }
    public RefreshToken(Guid id, string userId, string hasedToken,
        string device, string ipAddress, DateTime expiresAt,
        DateTime createdAt)
    {
        Id = Guard.Against.Default(id);
        UserId = Guard.Against.NullOrWhiteSpace(userId);
        HasedToken = Guard.Against.NullOrWhiteSpace(hasedToken);
        Device = Guard.Against.NullOrWhiteSpace(device);
        IpAddress = Guard.Against.NullOrWhiteSpace(ipAddress);
        ExpiresAt = Guard.Against.OutOfSQLDateRange(expiresAt);
        CreatedAt = Guard.Against.OutOfSQLDateRange(createdAt);
    }

    public void Revoke()
    {
        RevokedAt = Guard.Against.OutOfSQLDateRange(DateTime.UtcNow);
    }
}
