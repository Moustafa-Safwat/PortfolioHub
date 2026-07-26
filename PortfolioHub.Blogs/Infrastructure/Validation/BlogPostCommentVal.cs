using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Infrastructure.Validation;

internal sealed class BlogPostCommentVal
    : IEntityTypeConfiguration<BlogPostComment>
{
    public void Configure(
        EntityTypeBuilder<BlogPostComment> builder)
    {
        builder.ToTable("BlogPostComments", "blogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.BlogPostId)
            .IsRequired();

        // Scalar reference to the Users module.
        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasMaxLength(10_000)
            .IsRequired();

        builder.HasOne(x => x.BlogPost)
            .WithMany(x => x.BlogComments)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Self-referencing relationship using a shadow property because
        // ParentCommentId is missing from the entity.
        builder.Property<Guid?>("ParentCommentId");

        builder.HasMany(x => x.Replies)
            .WithOne()
            .HasForeignKey("ParentCommentId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Replies)
            .HasField("_replies")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => x.BlogPostId)
            .HasDatabaseName("IX_BlogPostComments_BlogPostId");

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_BlogPostComments_UserId");

        builder.HasIndex("ParentCommentId")
            .HasDatabaseName(
                "IX_BlogPostComments_ParentCommentId");

        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.CreatedAtUtc
        })
        .HasDatabaseName(
            "IX_BlogPostComments_BlogPostId_CreatedAtUtc");

        // The current List<Guid> mentions collection is not a normalized
        // relational relationship, so it is excluded from mapping.
        builder.Ignore(x => x.Mentions);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}