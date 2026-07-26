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
    public Task<Result> AddAsync(BlogPost blog, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IReadOnlyList<BlogPost>>> GetAllAsync(int pageNumber, int pageSize, List<Guid>? tagIds = null, string? search = null, bool isFeatured = false, CancellationToken cancellationToken = default)
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

    public Task<Result<int>> GetTotalCount(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(BlogPost blog, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
