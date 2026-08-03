using PortfolioHub.Users.Domain.Interfaces;
using Serilog;
using System.Reflection;

namespace ValidBuild.Web.Infrastructure;

internal static class DbSeeder
{
    public static async Task DbSeedData(this WebApplication app, IList<Assembly> assemblies)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbSeederTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IDbSeeder).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                .ToList();
            foreach (var dbSeederType in dbSeederTypes)
            {
                var seeder = scope
                    .ServiceProvider
                    .GetRequiredService(dbSeederType) as IDbSeeder;
                if (seeder is not null)
                {
                    await seeder.SeedAsync();
                    Log.Information($"Seeded data for {dbSeederType.Name}.");
                }
            }
        }
    }
}
