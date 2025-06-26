using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Projects.Infrastructure.Context;

namespace PortfolioHub.Projects.Domain.Entities;

internal class ProjectVal : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(DbSchemaConstants.DEFAULT_NAME_LENGTH);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(DbSchemaConstants.DEFAULT_Description_LENGTH);

        builder.Property(p => p.VideoUrl)
            .HasMaxLength(DbSchemaConstants.DEFAULT_URL_LENGTH);

        builder.Property(p => p.CreatedDate)
            .IsRequired();

        builder.HasMany(p => p.Images)
            .WithOne(g => g.Project)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Skills)
            .WithMany(s => s.Projects);

        builder.HasMany(p => p.Links)
            .WithOne(l => l.Project)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.IsFeatured)
            .IsRequired();

        builder.HasIndex(p => p.IsFeatured)
            .IsUnique(false)
            .IsClustered(false);
    }
}