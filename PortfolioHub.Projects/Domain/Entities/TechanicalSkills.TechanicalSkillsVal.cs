using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Projects.Infrastructure.Context;

namespace PortfolioHub.Projects.Domain.Entities;

internal class TechanicalSkillsVal : IEntityTypeConfiguration<TechanicalSkills>
{
    public void Configure(EntityTypeBuilder<TechanicalSkills> builder)
    {
        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.Name)
            .IsRequired()
            .HasMaxLength(DbSchemaConstants.DEFAULT_NAME_LENGTH);

        builder.HasMany(ts => ts.Projects)
            .WithMany(p => p.Skills);
    }
}