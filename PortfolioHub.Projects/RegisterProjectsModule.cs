using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PortfolioHub.Projects.Infrastructure.Context;

namespace PortfolioHub.Projects;

public static class RegisterProjectsModule
{
    public static IServiceCollection AddProjectsModule(this IServiceCollection service,
        IConfiguration configuration, IList<Assembly> assemblies)
    {
        service.AddSqlServer<ProjectsDbContext>(configuration.GetConnectionString("ProjectsDb"));

        assemblies.Add(typeof(RegisterProjectsModule).Assembly);
        return service;
    }
}
