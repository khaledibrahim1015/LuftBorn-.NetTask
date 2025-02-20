using Microsoft.EntityFrameworkCore;

namespace MyApp.Api.Extensions
{
    public static class HostExtensions
    {
        /// <summary>
        /// Migrates the database associated with the specified DbContext.
        /// </summary>
        /// <typeparam name="TContext">The type of the DbContext.</typeparam>
        /// <param name="host">The IHost instance.</param>
        /// <returns>The IHost instance.</returns>
        public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host)
           where TContext : DbContext
        {
            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            ILogger<TContext> logger = services.GetRequiredService<ILogger<TContext>>();
            TContext context = services.GetRequiredService<TContext>();

            try
            {
                logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

                // Only apply migrations
                if (context.Database.GetPendingMigrations().Any())
                {
                    logger.LogInformation("Applying pending migrations...");
                    await context.Database.MigrateAsync();
                }
                else
                {
                    logger.LogInformation("No pending migrations.");
                }

                logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while migrating the database for context {typeof(TContext).Name}");
                throw;  // Re-throw the exception to avoid silent failures.
            }


            return host;
        }


        public static async Task<IHost> SeedDatabaseAsync<TContext>(this IHost host, Func<TContext, IServiceProvider, Task> seeder) where TContext : DbContext
        {
            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;
            ILogger<TContext> logger = services.GetRequiredService<ILogger<TContext>>();
            TContext context = services.GetRequiredService<TContext>();
            try
            {
                logger.LogInformation($"Seeding database associated with context {typeof(TContext).Name}");
                await seeder(context, services);
                logger.LogInformation($"Seeded database associated with context {typeof(TContext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while seeding the database for context {typeof(TContext).Name}");
                throw;
            }
            return host;
        }

    }
}
