using Ardalis.Result;

namespace PortfolioHub.SharedKernal.Domain.Interfaces;

/// <summary>
/// Unit of Work pattern interface for managing database transactions and changes.
/// This interface is generic and can work with any DbContext.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    Task<Result> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    Task<Result> CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task<Result> RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets whether a transaction is currently active.
    /// </summary>
    bool HasActiveTransaction { get; }
}

/// <summary>
/// Generic Unit of Work interface for module-specific implementations.
/// Each module can define its own strongly-typed UnitOfWork interface.
/// Example: IAccountUnitOfWork : IUnitOfWork<UsersDbContext>
/// </summary>
public interface IUnitOfWork<out TContext> : IUnitOfWork
{
    /// <summary>
    /// Gets the underlying DbContext for advanced scenarios.
    /// Should be used sparingly to maintain abstraction.
    /// </summary>
    TContext Context { get; }
}