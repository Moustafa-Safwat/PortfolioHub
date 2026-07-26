using Ardalis.GuardClauses;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Blogs.Domain.Entities;
using PortfolioHub.Blogs.Domain.Interfaces;
using PortfolioHub.Blogs.Infrastructure.Context;

namespace PortfolioHub.Blogs.Infrastructure.EFRepository;

internal sealed class EFBlogPostsRepo
(
    BlogsDbContext dbContext
) : IBlogsRepo
{
    public async Task<Result> AddAsync(BlogPost blog, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(blog);

        try
        {
            var result = await dbContext.BlogPost.AddAsync(blog, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error($"Failed to add project: {ex.Message}");
        }
    }

    public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IReadOnlyList<BlogPost>>> GetAllAsync(int pageNumber, int pageSize, List<Guid>? tagIds = null, string? search = null, Guid userId = default, bool isFeatured = false, CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0)
            return Result.Invalid(new ValidationError("Page number must be greater than zero."));
        if (pageSize <= 0)
            return Result.Invalid(new ValidationError("Page size must be greater than zero."));

        var queryable = dbContext.BlogPost
            .Include(b => b.BlogComments)
            .Include(b => b.BlogPostBlocks)
            .Include(b => b.BlogReferences)
            .Include(b => b.BlogPostTags)
            .Include(b => b.BlogPostLikes)
            .Include(b => b.BlogPostAuthors)
            .Include(b => b.BlogPostViews)
            .OrderByDescending(b => b.CreatedAtUtc)
            .AsQueryable();

        if (userId == Guid.Empty)
        {
            queryable = queryable.Where(
                blog => blog.Status == BlogStatus.Published);
        }
        else
        {
            queryable = queryable.Where(
                blog =>
                    blog.Status == BlogStatus.Published ||
                    (
                        blog.Status == BlogStatus.Draft &&
                        blog.BlogPostAuthors.Any(
                            author => author.UserId == userId)
                    ));
        }

        if (tagIds?.Any() ?? false)
            queryable = queryable.Where(blog =>
                blog.BlogPostTags.Any(tag => tagIds.Contains(tag.Id)));

        if (!string.IsNullOrEmpty(search))
            queryable = queryable.Where(p => p.Title.Contains(search) || p.Description.Contains(search));

        if (isFeatured)
            queryable = queryable.Where(p => p.IsFeatured == isFeatured);

        var blogs = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<BlogPost>>(blogs);
    }

    public Task<Result<BlogPost>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<int>> GetTotalCount(CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await dbContext.BlogPost.CountAsync(cancellationToken);
            return Result.Success(count);
        }
        catch (Exception ex)
        {
            // Optionally log the exception here
            return Result<int>.Error($"Failed to get total project count: {ex.Message}");
        }
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        if (result > 0)
            return Result.Success();
        else
            return Result.Error("No changes were made to the database.");
    }

    public Task<Result> UpdateAsync(BlogPost blog, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
