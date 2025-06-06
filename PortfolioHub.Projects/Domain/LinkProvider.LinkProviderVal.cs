using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Projects.Infrastructure.Context;

namespace PortfolioHub.Projects.Domain;

internal class LinkProviderVal : IEntityTypeConfiguration<LinkProvider>
{
    public void Configure(EntityTypeBuilder<LinkProvider> builder)
    {
        builder.HasKey(lp => lp.Id);

        builder.Property(lp => lp.Name)
            .IsRequired()
            .HasMaxLength(DbSchemaConstants.DEFAULT_NAME_LENGTH);

        builder.Property(lp => lp.BaseUrl)
            .IsRequired()
            .HasMaxLength(DbSchemaConstants.DEFAULT_URL_LENGTH);

        builder.HasMany(lp => lp.Links)
            .WithOne(l => l.LinkProvider)
            .OnDelete(DeleteBehavior.NoAction);
    }
}