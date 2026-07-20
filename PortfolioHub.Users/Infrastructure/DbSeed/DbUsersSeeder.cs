using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Usecases.User.Create;

namespace ValidBuild.Account.Infrastructure.DbSeed;

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

    private async Task SeedSystemUserAsync()
    {
        var systemUserEmail = configuration["SystemUser:Email"] ?? throw new NullReferenceException("Missing 'SystemUser:Email' configuration");
        var systemUser = await userManager.FindByEmailAsync(systemUserEmail);
        if (systemUser is null)
        {
            var createUserCommand = new CreateUserCommand
            (
                Email: systemUserEmail,
                Password: configuration["SystemUser:Password"] ?? throw new NullReferenceException("Missing 'SystemUser:Password' configuration"),
                FirstName: configuration["SystemUser:FirstName"] ?? throw new NullReferenceException("Missing 'SystemUser:FirstName' configuration"),
                LastName: configuration["SystemUser:LastName"] ?? throw new NullReferenceException("Missing 'SystemUser:LastName' configuration"),
                Role: nameof(ApplicationUserRoles.System).ToLower(),
                "MSafwatHub"
            );
            var userResult = await sender.Send(createUserCommand);
            if (!userResult.IsSuccess)
                throw new InvalidOperationException($"{nameof(ApplicationUserRoles.System).ToLower()} can't be created");

            var userId = userResult.Value;

            var applicationUserResult = await userSecurityRepo.GetUserWithSecurityByIdAsync(userId);
            if (!applicationUserResult.IsSuccess)
                throw new InvalidOperationException($"Can't find object for application user for {nameof(ApplicationUserRoles.System).ToLower()} user");

            var applicationUser = applicationUserResult.Value;
            // verify email
            var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser!);
            await userManager.ConfirmEmailAsync(applicationUser!, token);
            applicationUser.VerifyEmail();
            // mark as system user
            applicationUser.UserSecurity?.SetAsSystemUser();

            await userSecurityRepo.SaveChangesAsync();
        }
    }
}
