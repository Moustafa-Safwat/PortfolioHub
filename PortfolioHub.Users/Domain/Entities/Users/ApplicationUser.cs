using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace PortfolioHub.Users.Domain.Entities.Users;

internal class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string ProfileImageUrl { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    public UserProfile? UserProfile { get; set; } = null!;
    public UserSecurity? UserSecurity { get; set; } = null!;

    // Parameterless constructor for EF Core
    public ApplicationUser() { }

    public ApplicationUser(string email, string firstName, string lastName, string profileImageUrl = "")
    {
        Email = Guard.Against.NullOrEmpty(email);
        FirstName = Guard.Against.NullOrEmpty(firstName);
        LastName = Guard.Against.NullOrEmpty(lastName);
        ProfileImageUrl = Guard.Against.Null(profileImageUrl);
        CreatedAt = DateTime.UtcNow;
    }

    public void SetUsername(string username)
    {
        UserName = Guard.Against.NullOrWhiteSpace(username);
        NormalizedUserName = username.ToUpperInvariant();
    }

    // Update Methods
    public void UpdateFirstName(string firstName)
    {
        FirstName = Guard.Against.NullOrEmpty(firstName);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLastName(string lastName)
    {
        LastName = Guard.Against.NullOrEmpty(lastName);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfileImageUrl(string profileImageUrl)
    {
        ProfileImageUrl = Guard.Against.NullOrEmpty(profileImageUrl);
        UpdatedAt = DateTime.UtcNow;
    }

    public void VerifyEmail()
    {
        EmailConfirmed = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
