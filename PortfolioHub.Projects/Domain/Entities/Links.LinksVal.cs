using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Projects.Infrastructure.Context;

namespace PortfolioHub.Projects.Domain.Entities;

internal class LinksVal : IEntityTypeConfiguration<Links>
{
    public void Configure(EntityTypeBuilder<Links> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Url)
            .IsRequired()
            .HasMaxLength(DbSchemaConstants.DEFAULT_URL_LENGTH);

        builder.HasOne(l => l.Project)
            .WithMany(p => p.Links)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(l => l.LinkProvider)
            .WithMany(lp => lp.Links)
            .OnDelete(DeleteBehavior.NoAction);
    }
}