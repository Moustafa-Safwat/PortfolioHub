using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Infrastructure.Validation;

internal sealed class BlogPostAuthorVal
    : IEntityTypeConfiguration<BlogPostAuthor>
{
    public void Configure(EntityTypeBuilder<BlogPostAuthor> builder)
    {
        builder.ToTable("BlogPostAuthors", "blogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.BlogPostId)
            .IsRequired();

        // This is only a scalar external-module reference.
        // Do not configure HasOne<User>() here.
        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(x => x.BlogPost)
            .WithMany(x => x.BlogPostAuthors)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Since BlogPostAuthor is soft deletable, the filtered index
        // allows a deleted author to be added again later.
        // This filter syntax is SQL Server specific.
        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.UserId
        })
        .IsUnique()
        .HasFilter($"[{nameof(BlogPost.IsDeleted)}] = 0")
        .HasDatabaseName(
            "UX_BlogPostAuthors_BlogPostId_UserId_Active");

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_BlogPostAuthors_UserId");

        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.Role,
            x.IsDeleted
        })
        .HasDatabaseName(
            "IX_BlogPostAuthors_BlogPostId_Role_IsDeleted");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}