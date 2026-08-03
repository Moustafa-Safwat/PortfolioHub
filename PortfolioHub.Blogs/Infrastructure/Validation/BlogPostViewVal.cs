using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Infrastructure.Validation;

internal sealed class BlogPostViewVal
    : IEntityTypeConfiguration<BlogPostView>
{
    public void Configure(EntityTypeBuilder<BlogPostView> builder)
    {
        builder.ToTable("BlogPostViews", "blogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.BlogPostId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.FirstViewedAtUtc)
            .IsRequired();

        builder.Property(x => x.LastViewAtUtc)
            .IsRequired(false);

        builder.Property(x => x.ViewCount)
            .HasDefaultValue(1)
            .IsRequired();

        builder.HasOne(x => x.BlogPost)
            .WithMany(x => x.BlogPostViews)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // One aggregated view record for every blog/user pair.
        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.UserId
        })
        .IsUnique()
        .HasDatabaseName("UX_BlogPostViews_BlogPostId_UserId");

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_BlogPostViews_UserId");

        builder.HasIndex(x => x.BlogPostId)
            .HasDatabaseName("IX_BlogPostViews_BlogPostId");

        // SQL Server check constraint prevents invalid persisted values.
        builder.ToTable(
            "BlogPostViews",
            "blogs",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "CK_BlogPostViews_ViewCount",
                    $"[{nameof(BlogPostView.ViewCount)}] >= 1");
            });
    }
}