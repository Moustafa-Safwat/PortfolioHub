using Ardalis.Result;
using PortfolioHub.Blogs.Domain.Entities;

namespace PortfolioHub.Blogs.Domain.Interfaces;

internal interface IReadOnlyBlogsRepo
{
    Task<Result<BlogPost>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<BlogPost>>> GetAllAsync(int pageNumber, int pageSize, List<Guid>? tagIds = null,
        string? search = null, Guid userId = default, bool isFeatured = false, CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<int>> GetTotalCount(CancellationToken cancellationToken = default);
}

internal interface IBlogsRepo : IReadOnlyBlogsRepo
{
    Task<Result> AddAsync(BlogPost blog, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(BlogPost blog, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}
