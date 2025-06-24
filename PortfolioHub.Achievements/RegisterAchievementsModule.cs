using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioHub.Achievements.Infrastructure.Context;
using PortfolioHub.Projects.Infrastructure.EFRepository;
using PortfolioHub.SharedKernal.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Achievements;

public static class RegisterAchievementsModule
{
    public static IServiceCollection AddAchievementsModule(this IServiceCollection service,
     IConfiguration configuration, IList<Assembly> assemblies)
    {
        service.AddSqlServer<AchievementsDbContext>(configuration.GetConnectionString("AchievementsDb"));

        var entityTypes = typeof(RegisterAchievementsModule).Assembly
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

        assemblies.Add(typeof(RegisterAchievementsModule).Assembly);
        return service;
    }
}
