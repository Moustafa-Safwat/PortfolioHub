using Microsoft.EntityFrameworkCore;
using PortfolioHub.Notification.Domain.Entities;

namespace PortfolioHub.Notification.Infrastructure.Context;

internal class OutboxDbContext(
DbContextOptions<OutboxDbContext> options
) : DbContext(options)
{
    public DbSet<EmailOutBox> EmailOutBoxes => Set<EmailOutBox>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OutboxDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DbSchemaConstants.EMAILSENDING_SCHEMA);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 6);
    }
}
