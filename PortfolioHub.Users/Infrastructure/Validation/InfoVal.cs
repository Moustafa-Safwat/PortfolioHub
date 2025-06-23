using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Users.Domain.Entities;

namespace PortfolioHub.Users.Infrastructure.Validation;

internal sealed class InfoVal : IEntityTypeConfiguration<Info>
{
    public void Configure(EntityTypeBuilder<Info> builder)
    {
        builder.ToTable("Infos");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.InfoKey).IsRequired().HasMaxLength(100);
        builder.Property(i => i.InfoValue).IsRequired().HasMaxLength(500);
        builder.HasIndex(i => i.InfoKey)
               .IsUnique(false)
               .IsClustered(false);
    }
}
