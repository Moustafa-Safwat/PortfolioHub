using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Notification.Domain.Entities;

namespace PortfolioHub.Notification.Infrastructure.Validation;

class EmailOutBoxValidation
: IEntityTypeConfiguration<EmailOutBox>
{
    public void Configure(EntityTypeBuilder<EmailOutBox> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();
    }
}
