using Microsoft.EntityFrameworkCore.Storage;
using PortfolioHub.Projects.Infrastructure.Context;

namespace PortfolioHub.Projects.Infrastructure;

internal sealed class UnitOfWork(
    ProjectsDbContext context
    ) : IUnitOfWork
{
    private IDbContextTransaction? transaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction == null) throw new InvalidOperationException("No transaction started.");
        await transaction.CommitAsync(cancellationToken);
        await transaction.DisposeAsync();
        transaction = null;
    }
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction == null) throw new InvalidOperationException("No transaction started.");
        await transaction.RollbackAsync(cancellationToken);
        await transaction.DisposeAsync();
        transaction = null;
    }
    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
    public void Dispose()
    {
        transaction?.Dispose();
        context.Dispose();
    }
}