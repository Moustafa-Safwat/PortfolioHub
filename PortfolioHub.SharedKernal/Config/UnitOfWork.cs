using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace ValidBuild.Sharedkernal.Infrastructure;

/// <summary>
/// Generic Unit of Work implementation that can work with any DbContext.
/// Each module should create its own derived class for dependency injection.
/// </summary>
public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    protected readonly TContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    public UnitOfWork(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets the underlying DbContext. Use sparingly to maintain abstraction.
    /// </summary>
    public TContext Context => _context;

    /// <summary>
    /// Indicates whether a transaction is currently active.
    /// </summary>
    public bool HasActiveTransaction => _transaction != null;

    public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(result);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Result.Error(new ErrorList([
                "A concurrency conflict occurred while saving changes. The data may have been modified by another user.",
                ex.Message
            ]));
        }
        catch (DbUpdateException ex)
        {
            return Result.Error(new ErrorList([
                "An error occurred while saving changes to the database.",
                ex.InnerException?.Message ?? ex.Message
            ]));
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList([
                "An unexpected error occurred while saving changes.",
                ex.Message
            ]));
        }
    }

    public async Task<Result> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
            {
                return Result.Error("A transaction is already in progress.");
            }

            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList(["Failed to begin transaction.", ex.Message]));
        }
    }

    public async Task<Result> CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction == null)
            {
                return Result.Error("No transaction is in progress.");
            }

            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;

            return Result.Success();
        }
        catch (Exception ex)
        {
            await RollbackTransactionAsync(cancellationToken);
            return Result.Error(new ErrorList(["Failed to commit transaction.", ex.Message]));
        }
    }

    public async Task<Result> RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction == null)
            {
                return Result.Error("No transaction is in progress.");
            }

            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(new ErrorList(["Failed to rollback transaction.", ex.Message]));
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
        _disposed = true;
    }
}
