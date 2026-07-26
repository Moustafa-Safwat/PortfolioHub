using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioHub.Blogs.Infrastructure.Context;
using System.Reflection;

namespace PortfolioHub.Blogs;

public static class RegisterBlogsModule
{
    public static IServiceCollection AddBlogsModule(this IServiceCollection service,
      IConfiguration configuration, IList<Assembly> assemblies)
    {
        service.AddSqlServer<BlogsDbContext>(configuration.GetConnectionString("BlogsDb"));

        assemblies.Add(typeof(RegisterBlogsModule).Assembly);
        return service;
    }
}
