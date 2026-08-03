using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Infrastructure.Context;

namespace ValidBuild.Account.Infrastructure.Validation;

internal class UserSecurityVal : IEntityTypeConfiguration<UserSecurity>
{
    public void Configure(EntityTypeBuilder<UserSecurity> builder)
    {
        builder.ToTable("UserSecurity", DbSchemaConstants.USERS_SCHEMA);

        builder.HasKey(us => us.Id);

        builder.Property(us => us.UserId)
            .IsRequired();

        builder.Property(us => us.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(us => us.DeletedAt);

        builder.Property(us => us.DeletedBy);

        builder.Property(us => us.LastLoginAt);

        builder.Property(us => us.LastPasswordChangedAt);

        builder.Property(us => us.MustChangePassword)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(us => us.IsSystemUser)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(us => us.IsLockedByAdmin)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(us => us.AdminLockReason)
            .HasMaxLength(1000);

        // Configure relationship with ApplicationUser
        builder.HasOne(us => us.User)
            .WithOne(u => u.UserSecurity)
            .HasForeignKey<UserSecurity>(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Create index on UserId for performance
        builder.HasIndex(us => us.UserId)
            .IsUnique();
    }
}
