using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace PortfolioHub.Users.Infrastructure.Context;

internal class UsersDbContext(
    DbContextOptions<UsersDbContext> options
    )
    : IdentityDbContext(options)
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        builder.HasDefaultSchema(DbSchemaConstants.Users_SCHEMA);
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 6);
    }
}
