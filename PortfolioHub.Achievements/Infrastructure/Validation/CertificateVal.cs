using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Achievements.Domain;

namespace PortfolioHub.Achievements.Infrastructure.Validation;

internal sealed class CertificateVal : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Issuer)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Date)
            .IsRequired();

        builder.ToTable("Certificates");
    }
}
