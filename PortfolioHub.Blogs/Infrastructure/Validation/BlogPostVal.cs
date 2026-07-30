using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Infrastructure.Validation;

internal sealed class BlogPostVal
    : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("BlogPosts", "blogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.CoverImageUrl)
            .IsRequired();

        builder.Property(x => x.Slug)
            .HasMaxLength(250)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2_000)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.PublishedAtUtc);

        // Keep slug unique across all posts, including deleted posts.
        // This prevents an old URL from unexpectedly pointing to a new post.
        builder.HasIndex(x => x.Slug)
            .IsUnique()
            .HasDatabaseName("UX_BlogPosts_Slug");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_BlogPosts_Status");

        builder.HasIndex(x => x.PublishedAtUtc)
            .HasDatabaseName("IX_BlogPosts_PublishedAt");

        builder.HasIndex(x => x.CreatedAtUtc)
            .HasDatabaseName("IX_BlogPosts_CreatedAtUtc");

        builder.HasIndex(x => x.CreatedBy)
            .HasDatabaseName("IX_BlogPosts_CreatedBy");

        builder.HasIndex(x => x.IsDeleted)
            .HasDatabaseName("IX_BlogPosts_IsDeleted");

        // Relationships owned by the Blogs module.
        builder.HasMany(x => x.BlogPostLikes)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.BlogPostViews)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.BlogPostAuthors)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.BlogPostBlocks)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.BlogComments)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.BlogReferences)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // BlogPostTag currently behaves as the Tag entity itself,
        // so this configuration creates an implicit many-to-many join table.
        builder.HasMany(x => x.BlogPostTags)
            .WithMany(x => x.BlogPosts)
            .UsingEntity<Dictionary<string, object>>(
                "BlogPostTagLinks",
                right => right
                    .HasOne<BlogPostTag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<BlogPost>()
                    .WithMany()
                    .HasForeignKey("BlogPostId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.ToTable("BlogPostTagLinks", "blogs");

                    join.HasKey("BlogPostId", "TagId");

                    join.HasIndex("TagId")
                        .HasDatabaseName("IX_BlogPostTagLinks_TagId");
                });

        // Tell EF Core to mutate the backing fields rather than trying
        // to replace the IReadOnlyCollection properties.
        builder.Navigation(x => x.BlogPostLikes)
            .HasField("_blogPostLikes")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.BlogPostViews)
            .HasField("_blogPostViews")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.BlogPostAuthors)
            .HasField("_blogPostAuthors")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.BlogPostTags)
            .HasField("_blogPostTags")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.BlogPostBlocks)
            .HasField("_blogPostBlocks")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.BlogComments)
            .HasField("_blogComments")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(x => x.BlogReferences)
            .HasField("_blogReferences")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Global soft-delete filter.
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}