using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Users.Domain.Entities;
using PortfolioHub.Users.Domain.Entities.Users;

namespace PortfolioHub.Users.Infrastructure.Context;

internal class UsersDbContext(
    DbContextOptions<UsersDbContext> options
    )
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<UserProfile> UsersProfile => Set<UserProfile>();
    public DbSet<UserSecurity> UserSecurity => Set<UserSecurity>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Info> Infos => Set<Info>();
    public DbSet<ProfessionalSkill> ProfessionalSkills => Set<ProfessionalSkill>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        builder.HasDefaultSchema(DbSchemaConstants.USERS_SCHEMA);

        // Configure Identity tables
        base.OnModelCreating(builder);

        // Override Identity's default table name for ApplicationUser
        builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 6);
    }
}
