using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace PortfolioHub.Web.Infra;

internal static class PendingMigrations
{
    public static void ApplyPendingMigrations(this WebApplication app, IList<Assembly> assemblies)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContextTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(DbContext).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                .ToList();

            foreach (var dbContextType in dbContextTypes)
            {
                var db = scope
                    .ServiceProvider
                    .GetRequiredService(dbContextType) as DbContext;

                if (db is not null && db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                    Log.Information($"Applied pending migrations for {dbContextType.Name}.");
                }
                else
                {
                    Log.Information($"No pending migrations for {dbContextType.Name}.");
                }
            }
        }
    }
}
