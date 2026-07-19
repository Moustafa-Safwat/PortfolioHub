using Ardalis.GuardClauses;
using PortfolioHub.SharedKernal.Domain.Entities;

namespace PortfolioHub.Users.Domain.Entities.Users;

internal class UserSecurity : BaseEntity
{
    public Guid UserId { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedBy { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? LastPasswordChangedAt { get; private set; }
    public bool MustChangePassword { get; private set; }
    public bool IsSystemUser { get; private set; }
    public bool IsLockedByAdmin { get; private set; }
    public string AdminLockReason { get; private set; } = string.Empty;

    // Navigation Property
    public ApplicationUser? User { get; set; } = null!;

    // EF Constructor
    public UserSecurity() { }
    public UserSecurity(Guid id, Guid userId)
    {
        Id = Guard.Against.Default(id);
        UserId = Guard.Against.Default(userId);
        IsDeleted = false;
        MustChangePassword = false;
        IsSystemUser = false;
        IsLockedByAdmin = false;
    }


    public void Delete(Guid deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = Guard.Against.Default(deletedBy);
    }

    public void SetAsSystemUser()
    {
        IsSystemUser = true;
    }

    public void LockByAdmin(string reason)
    {
        IsLockedByAdmin = true;
        AdminLockReason = Guard.Against.NullOrWhiteSpace(reason);
    }
    public void UnlockByAdmin()
    {
        IsLockedByAdmin = false;
        AdminLockReason = string.Empty;
    }

    public void ChangePassword()
    {
        LastPasswordChangedAt = DateTime.UtcNow;
        MustChangePassword = false;
    }
    public void RequirePasswordChange()
    {
        MustChangePassword = true;
    }
    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

}