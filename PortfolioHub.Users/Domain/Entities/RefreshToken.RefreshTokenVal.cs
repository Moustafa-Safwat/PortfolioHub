namespace PortfolioHub.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class RefreshTokenVal : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.UserId).IsRequired().HasMaxLength(450);
        builder.HasIndex(rt => rt.HasedToken).IsClustered(false).IsUnique();
        builder.Property(rt => rt.HasedToken).IsRequired().HasMaxLength(512);
        builder.Property(rt => rt.Device).IsRequired().HasMaxLength(100);
        builder.Property(rt => rt.IpAddress).IsRequired().HasMaxLength(45);
        builder.Property(rt => rt.ExpiresAt).IsRequired();
        builder.Property(rt => rt.CreatedAt).IsRequired();
        builder.Property(rt => rt.RevokedAt).IsRequired(false);

        builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId);
    }
}