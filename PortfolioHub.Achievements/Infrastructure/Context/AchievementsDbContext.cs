using Microsoft.EntityFrameworkCore;
using PortfolioHub.Achievements.Domain;

namespace PortfolioHub.Achievements.Infrastructure.Context;

internal class AchievementsDbContext(
    DbContextOptions<AchievementsDbContext> options
    ) : DbContext(options)
{
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Certificate> Certificates => Set<Certificate>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AchievementsDbContext).Assembly);
        builder.HasDefaultSchema(DbSchemaConstants.Achievements_SCHEMA);
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 6);
    }
}
