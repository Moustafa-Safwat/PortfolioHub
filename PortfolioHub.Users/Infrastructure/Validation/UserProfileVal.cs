using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Infrastructure.Context;

namespace ValidBuild.Account.Infrastructure.Validation;

internal class UserProfileVal : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles", DbSchemaConstants.USERS_SCHEMA);

        builder.HasKey(up => up.Id);

        builder.Property(up => up.Id)
            .IsRequired();

        builder.Property(up => up.UserId)
            .IsRequired();

        builder.Property(up => up.JobTitle)
            .HasMaxLength(200);

        builder.Property(up => up.CompanyName)
            .HasMaxLength(300);

        builder.Property(up => up.Language)
            .HasMaxLength(50);

        builder.Property(up => up.Country)
            .HasMaxLength(100);

        builder.Property(up => up.Address1)
            .HasMaxLength(500);

        builder.Property(up => up.Address2)
            .HasMaxLength(500);

        builder.Property(up => up.BirthDate)
            .IsRequired(false); // nullable

        builder.Property(up => up.CreatedAt)
            .IsRequired();

        builder.Property(up => up.UpdatedAt)
            .IsRequired(false); // nullable

        // Configure one-to-one relationship with ApplicationUser
        builder.HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Create index on UserId for better query performance
        builder.HasIndex(up => up.UserId)
            .IsUnique();
    }
}
