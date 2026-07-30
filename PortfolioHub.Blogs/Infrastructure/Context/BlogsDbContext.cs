using Microsoft.EntityFrameworkCore;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Infrastructure.Context;

internal class BlogsDbContext
(
    DbContextOptions<BlogsDbContext> options
) : DbContext(options)
{
    public DbSet<BlogPost> BlogPost => Set<BlogPost>();
    public DbSet<BlogPostAuthor> BlogPostAuthors => Set<BlogPostAuthor>();
    public DbSet<BlogPostBlock> BlogPostBlocks => Set<BlogPostBlock>();
    public DbSet<BlogPostComment> BlogPostComments => Set<BlogPostComment>();
    public DbSet<BlogPostLike> BlogPostLikes => Set<BlogPostLike>();
    public DbSet<BlogPostReferences> BlogPostReferences => Set<BlogPostReferences>();
    public DbSet<BlogPostTag> BlogPostTags => Set<BlogPostTag>();
    public DbSet<BlogPostView> BlogPostViews => Set<BlogPostView>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(BlogsDbContext).Assembly);
        builder.HasDefaultSchema(DbSchemaConstants.BLOGS_SCHEMA);
        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 6);
    }
}
