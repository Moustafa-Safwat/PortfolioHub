using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioHub.Blogs.Infrastructure.Context;
using PortfolioHub.Blogs.Infrastructure.DbSeed;
using PortfolioHub.Blogs.Infrastructure.EFRepository;
using PortfolioHub.SharedKernal.Domain.Entities;
using PortfolioHub.SharedKernal.Domain.Interfaces;
using System.Reflection;

namespace PortfolioHub.Blogs;

public static class RegisterBlogsModule
{
    public static IServiceCollection AddBlogsModule(this IServiceCollection service,
      IConfiguration configuration, IList<Assembly> assemblies)
    {
        service.AddSqlServer<BlogsDbContext>(configuration.GetConnectionString("BlogsDb"));

        service.AddScoped<DbBlogTagSeeder>();

        var entityTypes = typeof(RegisterBlogsModule).Assembly
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


        assemblies.Add(typeof(RegisterBlogsModule).Assembly);
        return service;
    }
}
