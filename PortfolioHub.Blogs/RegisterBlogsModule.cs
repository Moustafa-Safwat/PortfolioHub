using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PortfolioHub.Blogs;

public static class RegisterBlogsModule
{
    public static IServiceCollection AddBlogsModule(this IServiceCollection service,
      IConfiguration configuration, IList<Assembly> assemblies)
    {

        return service;
    }
}
