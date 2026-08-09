using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Usecases.User.Create;

namespace ValidBuild.Account.Infrastructure.DbSeed;

internal sealed record AuthUser(
    string Email,
    string Password,
    string FristName,
    string LastName,
    ApplicationUserRoles Role);

internal class DbUsersSeeder
(
    RoleManager<IdentityRole<Guid>> roleManager,
    UserManager<ApplicationUser> userManager,
    IUserSecurityRepo userSecurityRepo,
    ISender sender,
    IConfiguration configuration
) : IDbSeeder
{
    public async Task SeedAsync()
    {
        await SeedUsersRolesAsync();
        await SeedSystemUserAsync();
    }

    private async Task SeedUsersRolesAsync()
    {
        var roles = Enum.GetNames(typeof(ApplicationUserRoles));

        foreach (var roleName in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = roleName.ToLower(),
                    NormalizedName = roleName.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });
            }
        }
    }

    private IList<AuthUser> GetAuthUsers()
    {
        var systemAuthUser = new AuthUser
        (
            Email: configuration["SystemUser:Email"] ?? throw new NullReferenceException("Missing 'SystemUser:Email' configuration"),
            Password: configuration["SystemUser:Password"] ?? throw new NullReferenceException("Missing 'SystemUser:Password' configuration"),
            FristName: configuration["SystemUser:FirstName"] ?? throw new NullReferenceException("Missing 'SystemUser:FirstName' configuration"),
            LastName: configuration["SystemUser:LastName"] ?? throw new NullReferenceException("Missing 'SystemUser:LastName' configuration"),
            Role: ApplicationUserRoles.System
        );

        var adminAuthUser = new AuthUser
         (
            Email: configuration["Auth:AdminEmail"] ?? throw new NullReferenceException("Missing 'AdminUser:Email' configuration"),
            Password: configuration["Auth:AdminPassword"] ?? throw new NullReferenceException("Missing 'AdminUser:Password' configuration"),
            FristName: configuration["Auth:AdminFirstName"] ?? throw new NullReferenceException("Missing 'AdminUser:FirstName' configuration"),
            LastName: configuration["Auth:AdminLastName"] ?? throw new NullReferenceException("Missing 'AdminUser:LastName' configuration"),
            Role: ApplicationUserRoles.Admin
         );

        return new List<AuthUser>()
        {
            systemAuthUser,
            adminAuthUser
        };

    }

    private async Task SeedSystemUserAsync()
    {
        var authUsers = GetAuthUsers();

        foreach (var authUser in authUsers)
        {
            var user = await userManager.FindByEmailAsync(authUser.Email);

            if (user is null)
            {   // ONLE Create users if not exists
                var authUserRole = authUser.Role.ToString().ToLower();

                var createUserCommand = new CreateUserCommand
                (
                    Email: authUser.Email,
                    Password: authUser.Password,
                    FirstName: authUser.FristName,
                    LastName: authUser.LastName,
                    Role: authUserRole,
                    CompanyName: "MSafwatHub",
                    VerifyEmail: false
                );

                var userResult = await sender.Send(createUserCommand);
                if (!userResult.IsSuccess)
                    throw new InvalidOperationException($"{authUserRole} can't be created");

                var userId = userResult.Value;

                var applicationUserResult = await userSecurityRepo.GetUserWithSecurityByIdAsync(userId);
                if (!applicationUserResult.IsSuccess)
                    throw new InvalidOperationException($"Can't find object for application user for {authUserRole} user");

                var applicationUser = applicationUserResult.Value;
                // verify email
                var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser!);
                await userManager.ConfirmEmailAsync(applicationUser!, token);
                applicationUser.VerifyEmail();
                if (authUser.Role == ApplicationUserRoles.System)
                {
                    // mark as system user
                    applicationUser.UserSecurity?.SetAsSystemUser();
                }

                await userSecurityRepo.SaveChangesAsync();
            }
        }

    }
}
