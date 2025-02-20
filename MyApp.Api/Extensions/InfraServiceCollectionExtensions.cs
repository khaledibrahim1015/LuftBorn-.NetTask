using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyApp.Core.Inrerfaces;
using MyApp.Domain.Inrerfaces;
using MyApp.Infrastructure.Configuration;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Repositories;
using MyApp.Infrastructure.Utils.HealthChecks;

namespace MyApp.Api.Extensions
{
    /// <summary>
    /// Extension methods for adding infrastructure services to the IServiceCollection.
    /// </summary>
    public static class InfraServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the infrastructure services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">The IConfiguration to use for configuring services.</param>
        /// <returns>The IServiceCollection with the added services.</returns>
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            // Configure AppSettings using the "DatabaseSettings" section from the configuration
            services.Configure<AppSettings>(configuration.GetSection("DatabaseSettings"));

            //  Use AddDbContext to properly register EF Core services
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetSection("DatabaseSettings").GetConnectionString("ConnectionString"))
            );

            // Register AppDbContextOptionFactory as a singleton service
            services.AddSingleton<AppDbContextOptionFactory>();
            // Register AppDbContext as a scoped service
            services.AddScoped(serviceprovider =>
            {
                var dbConextOptionFactory = serviceprovider.GetRequiredService<AppDbContextOptionFactory>();
                return new AppDbContext(dbConextOptionFactory.GetDbContextOptions());
            });

            //// Register the DbContext factory
            //services.AddSingleton<IAppDbContextFactory, AppDbContextFactory>();
            //// Register AppDbContext as scoped, using the factory to create instances
            //services.AddScoped(sp =>
            //{
            //    var factory = sp.GetRequiredService<IAppDbContextFactory>();
            //    return factory.CreateDbContext();
            //});


            //services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            //{
            //    var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
            //    var connectionString = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
            //        ? appSettings.ConnectionString
            //        : Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            //    options.UseSqlServer(connectionString);
            //});

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var dbContextOptionsFactory = serviceProvider.GetRequiredService<AppDbContextOptionFactory>();
                options.UseSqlServer(dbContextOptionsFactory.AppSettings.ConnectionString);
            });
            // Register services 
            services.AddScoped(typeof(IAsyncBaseRepository<,>), typeof(RepositoryBase<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();




            // Register Health Check 
            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>(
                        name: "MainDb-Check",
                        failureStatus: HealthStatus.Unhealthy
                        );
            return services;
        }
    }
}
