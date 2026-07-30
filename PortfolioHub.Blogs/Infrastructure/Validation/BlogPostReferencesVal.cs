using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Infrastructure.Validation;

internal sealed class BlogPostReferencesVal
    : IEntityTypeConfiguration<BlogPostReferences>
{
    public void Configure(
        EntityTypeBuilder<BlogPostReferences> builder)
    {
        builder.ToTable("BlogPostReferences", "blogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.BlogPostId)
            .IsRequired();

        builder.Property(x => x.Url)
            .HasMaxLength(2_048)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.Label)
            .HasMaxLength(250)
            .IsRequired();

        builder.HasOne(x => x.BlogPost)
            .WithMany(x => x.BlogReferences)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.BlogPostId)
            .HasDatabaseName(
                "IX_BlogPostReferences_BlogPostId");

        // Prevent the same active URL from being added more than once
        // to the same blog post.
        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.Url
        })
        .IsUnique()
        .HasFilter($"[{nameof(BlogPostReferences.IsDeleted)}] = 0")
        .HasDatabaseName(
            "UX_BlogPostReferences_BlogPostId_Url_Active");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}