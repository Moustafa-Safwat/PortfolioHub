using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioHub.Projects.Infrastructure.EFRepository;
using PortfolioHub.SharedKernal.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using PortfolioHub.Users.Domain.Entities;
using PortfolioHub.Users.Domain.Interfaces;
using PortfolioHub.Users.Infrastructure.Context;
using PortfolioHub.Users.Infrastructure.EFRepository;
using PortfolioHub.Users.Usecases.User.Login;

namespace PortfolioHub.Users;

public static class RegisterUsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection service,
        IConfiguration configuration, IList<Assembly> assemblies)
    {
        service.AddSqlServer<UsersDbContext>(configuration.GetConnectionString("UsersDb"));
        service.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
            .AddEntityFrameworkStores<UsersDbContext>();

        service.AddScoped<JwtService>();
        service.AddSingleton<TokenHasher>();
        service.AddScoped<IRefreshTokenRepo, EFRefreshTokenRepo>();
        service.AddScoped<IInfoRepo, EFInfoRepository>();

        var entityTypes = typeof(RegisterUsersModule).Assembly
           .GetTypes()
           .Where(t => t.IsClass
           && !t.IsAbstract
           && typeof(BaseEntity).IsAssignableFrom(t));

        foreach (var entityType in entityTypes)
        {
            var repoInterface = typeof(IEntityRepo<>).MakeGenericType(entityType);
            var repoImplementation = typeof(EFEntityRepo<>).MakeGenericType(entityType);
            service.AddScoped(repoInterface, repoImplementation);
        }

        assemblies.Add(typeof(RegisterUsersModule).Assembly);
        return service;
    }
}
