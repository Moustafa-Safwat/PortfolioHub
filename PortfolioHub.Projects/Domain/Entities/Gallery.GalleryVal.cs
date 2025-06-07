using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Projects.Infrastructure.Context;

namespace PortfolioHub.Projects.Domain.Entities;

internal class GalleryVal : IEntityTypeConfiguration<Gallery>
{
    public void Configure(EntityTypeBuilder<Gallery> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.ImageUrl)
            .IsRequired()
            .HasMaxLength(DbSchemaConstants.DEFAULT_URL_LENGTH);

        builder.Property(g => g.Order);

        builder.HasOne(g => g.Project)
            .WithMany(p => p.Images)
            .OnDelete(DeleteBehavior.NoAction);
    }
}