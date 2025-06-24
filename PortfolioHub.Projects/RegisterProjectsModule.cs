using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioHub.Projects.Domain.Entities;
using PortfolioHub.Projects.Domain.Interfaces;
using PortfolioHub.Projects.Infrastructure;
using PortfolioHub.Projects.Infrastructure.Context;
using PortfolioHub.Projects.Infrastructure.EFRepository;
using PortfolioHub.SharedKernal.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;

namespace PortfolioHub.Projects;

public static class RegisterProjectsModule
{
    public static IServiceCollection AddProjectsModule(this IServiceCollection service,
        IConfiguration configuration, IList<Assembly> assemblies)
    {
        service.AddSqlServer<ProjectsDbContext>(configuration.GetConnectionString("ProjectsDb"));

        service.AddScoped<IProjectsRepo, EFProjectRepo>();
        service.AddScoped<IUnitOfWork, UnitOfWork>();

        var entityTypes = typeof(RegisterProjectsModule).Assembly
            .GetTypes()
            .Where(t => t.IsClass
            && !t.IsAbstract
            && typeof(BaseEntity).IsAssignableFrom(t)
            && t.Name != nameof(Project));

        foreach (var entityType in entityTypes)
        {
            var repoInterface = typeof(IEntityRepo<>).MakeGenericType(entityType);
            var repoImplementation = typeof(EFEntityRepo<>).MakeGenericType(entityType);
            service.AddScoped(repoInterface, repoImplementation);
        }

        assemblies.Add(typeof(RegisterProjectsModule).Assembly);
        return service;
    }
}
