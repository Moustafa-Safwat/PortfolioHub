using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Infrastructure.Validation;

internal sealed class BlogPostBlockVal
    : IEntityTypeConfiguration<BlogPostBlock>
{
    public void Configure(EntityTypeBuilder<BlogPostBlock> builder)
    {
        builder.ToTable(
            "BlogPostBlocks",
            "blogs",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "CK_BlogPostBlocks_Order",
                    $"[{nameof(BlogPostBlock.Order)}] >= 0");
            });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.BlogPostId)
            .IsRequired();

        builder.Property(x => x.Text)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(x => x.BlockType)
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.Url)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(x => x.MimeType)
            .HasMaxLength(150)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(x => x.CodeTitle)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(x => x.CodeLanguage)
            .HasMaxLength(100)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(x => x.TextAlign)
            .HasMaxLength(20)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(x => x.Order)
            .IsRequired();

        builder.HasOne(x => x.BlogPost)
            .WithMany(x => x.BlogPostBlocks)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // No two active blocks should occupy the same position.
        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.Order
        })
        .IsUnique()
        .HasFilter($"[{nameof(BlogPostBlock.IsDeleted)}] = 0")
        .HasDatabaseName(
            "UX_BlogPostBlocks_BlogPostId_Order_Active");

        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.BlockType
        })
        .HasDatabaseName(
            "IX_BlogPostBlocks_BlogPostId_BlockType");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}