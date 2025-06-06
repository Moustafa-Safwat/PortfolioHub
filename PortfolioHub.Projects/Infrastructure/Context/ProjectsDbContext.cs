using Microsoft.EntityFrameworkCore;
using PortfolioHub.Projects.Domain;

namespace PortfolioHub.Projects.Infrastructure.Context;

internal class ProjectsDbContext(
    DbContextOptions<ProjectsDbContext> options
    ) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Gallery> Galleries => Set<Gallery>();
    public DbSet<TechanicalSkills> TechnicalSkills => Set<TechanicalSkills>();
    public DbSet<Links> Links => Set<Links>();
    public DbSet<LinkProvider> LinkProviders => Set<LinkProvider>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ProjectsDbContext).Assembly);
        builder.HasDefaultSchema(DbSchemaConstants.Projects_SCHEMA);
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 6);
    }
}
