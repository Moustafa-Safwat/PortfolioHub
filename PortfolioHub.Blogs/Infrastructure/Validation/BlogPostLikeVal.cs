using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Infrastructure.Validation;

internal sealed class BlogPostLikeVal
    : IEntityTypeConfiguration<BlogPostLike>
{
    public void Configure(EntityTypeBuilder<BlogPostLike> builder)
    {
        builder.ToTable("BlogPostLikes", "blogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.BlogPostId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.IsLiked)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.LikedAtUtc)
            .IsRequired();

        builder.Property(x => x.UnLikedAtUtc)
            .IsRequired(false);

        builder.HasOne(x => x.BlogPost)
            .WithMany(x => x.BlogPostLikes)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // One state record per user and blog.
        // A user unlikes and re-likes by updating the same row rather
        // than inserting another row.
        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.UserId
        })
        .IsUnique()
        .HasDatabaseName("UX_BlogPostLikes_BlogPostId_UserId");

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_BlogPostLikes_UserId");

        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.IsLiked
        })
        .HasDatabaseName("IX_BlogPostLikes_BlogPostId_IsLiked");
    }
}