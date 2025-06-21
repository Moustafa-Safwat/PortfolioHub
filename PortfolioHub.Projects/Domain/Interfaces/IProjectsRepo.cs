using Ardalis.Result;
using PortfolioHub.Projects.Domain.Entities;

namespace PortfolioHub.Projects.Domain.Interfaces;

internal interface IReadOnlyProjectsRepo
{
    Task<Result<Project>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<Project>>> GetAllAsync(int pageNumber, int pageSize, Guid? categoryId = null,
        string? search = null, CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<int>> GetTotalCount(CancellationToken cancellationToken = default);
}
internal interface IProjectsRepo : IReadOnlyProjectsRepo
{
    Task<Result> AddAsync(Project project, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Project project, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}
