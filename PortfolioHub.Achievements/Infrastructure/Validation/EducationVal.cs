using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Achievements.Domain;

namespace PortfolioHub.Achievements.Infrastructure.Validation;

internal sealed class EducationVal : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Degree)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Institution)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.FieldOfStudy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired(false);
    }
}
