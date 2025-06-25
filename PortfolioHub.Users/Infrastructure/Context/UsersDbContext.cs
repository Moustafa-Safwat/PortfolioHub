using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PortfolioHub.Users.Domain.Entities;

namespace PortfolioHub.Users.Infrastructure.Context;

internal class UsersDbContext(
    DbContextOptions<UsersDbContext> options
    )
    : IdentityDbContext(options)
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Info> Infos => Set<Info>();
    public DbSet<ProfessionalSkill> ProfessionalSkills => Set<ProfessionalSkill>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        builder.HasDefaultSchema(DbSchemaConstants.Users_SCHEMA);

        // Seed roles: Guest and Admin
        SeedDefaultRoles(builder);

        // Configure Identity tables
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 6);
    }

    private void SeedDefaultRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = "46223b84-be0f-495b-9361-0f20ccb032a2",
                        Name = "guest",
                        NormalizedName = "GUEST",
                        ConcurrencyStamp = "1"
                    },
                    new IdentityRole
                    {
                        Id = "d9fca67b-4cc0-48bb-858f-8704249a8eb8",
                        Name = "admin",
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = "2"
                    }
                );
    }
}
