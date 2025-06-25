using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Achievements.Domain;

internal class Certificate : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Issuer { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }

    public Certificate(Guid id, string name, string issuer, DateTime date)
    {
        Id = Guard.Against.Default(id);
        Name = Guard.Against.NullOrEmpty(name);
        Issuer = Guard.Against.NullOrEmpty(issuer);
        Date = Guard.Against.OutOfSQLDateRange(date);
    }
}
