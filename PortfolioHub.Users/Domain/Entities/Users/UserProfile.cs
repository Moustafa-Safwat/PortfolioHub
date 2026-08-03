using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Users.Domain.Entities.Users;

internal class UserProfile : BaseEntity
{
    public Guid UserId { get; private set; }
    public string JobTitle { get; private set; } = string.Empty;
    public string CompanyName { get; private set; } = string.Empty;
    public string Language { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string Address1 { get; private set; } = string.Empty;
    public string Address2 { get; private set; } = string.Empty;
    public DateTime? BirthDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Property
    public ApplicationUser? User { get; set; } = null!;

    // EF Constructor
    public UserProfile() { }

    public UserProfile(Guid id, Guid userId, string companyName)
    {
        Id = Guard.Against.Default(id);
        UserId = Guard.Against.Default(userId);
        CompanyName = Guard.Against.NullOrEmpty(companyName);
        CreatedAt = DateTime.UtcNow;
    }

    // Update Methods
    public void UpdateJobTitle(string jobTitle)
    {
        JobTitle = Guard.Against.NullOrWhiteSpace(jobTitle);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCompanyName(string companyName)
    {
        CompanyName = Guard.Against.Null(companyName);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLanguage(string language)
    {
        Language = Guard.Against.NullOrWhiteSpace(language);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCountry(string country)
    {
        Country = Guard.Against.NullOrWhiteSpace(country);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress1(string address1)
    {
        Address1 = Guard.Against.NullOrWhiteSpace(address1);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress2(string address2)
    {
        Address2 = Guard.Against.NullOrWhiteSpace(address2);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBirthDate(DateTime birthDate)
    {
        // Validate birth date: must be at least 13 years old and not more than 120 years ago
        var minDate = DateTime.UtcNow.AddYears(-120);
        var maxDate = DateTime.UtcNow.AddYears(-13);

        BirthDate = Guard.Against.OutOfRange(birthDate, nameof(birthDate), minDate, maxDate);
        UpdatedAt = DateTime.UtcNow;
    }
}
