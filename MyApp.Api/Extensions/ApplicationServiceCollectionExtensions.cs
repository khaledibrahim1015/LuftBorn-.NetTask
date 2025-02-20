using MyApp.Application.Queries;

namespace MyApp.Api.Extensions;

/// <summary>
/// Extension methods for adding application services to the IServiceCollection.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Adds application services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The IConfiguration instance for configuration settings.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(mCfg =>
        {
            mCfg.RegisterServicesFromAssemblies(typeof(GetAllProductQuery).Assembly);
        });

        return services;
    }
}
