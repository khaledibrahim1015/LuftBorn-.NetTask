using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyApp.Infrastructure.Configuration;

namespace MyApp.Infrastructure.Data;


// Note : Not Use For Current 
/// <summary>
/// Factory class to create DbContextOptions for AppDbContext.
/// </summary>




public interface IAppDbContextFactory
{
    AppDbContext CreateDbContext();
}

public class AppDbContextFactory : IAppDbContextFactory
{
    private readonly DbContextOptions<AppDbContext> _options;

    public AppDbContextFactory(IOptions<AppSettings> options)
    {
        var appSettings = options.Value;
        var connectionString = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
            ? appSettings.ConnectionString
            : Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        _options = optionsBuilder.Options;
    }

    public AppDbContext CreateDbContext()
    {
        return new AppDbContext(_options);
    }
}


public class AppDbContextOptionFactory
{
    /// <summary>
    /// Gets or sets the application settings.
    /// </summary>
    public AppSettings AppSettings { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContextOptionFactory"/> class.
    /// </summary>
    /// <param name="options">The application settings options.</param>
    public AppDbContextOptionFactory(IOptions<AppSettings> options)
        => AppSettings = options.Value;

    /// <summary>
    /// Gets the DbContextOptions for AppDbContext.
    /// </summary>
    /// <returns>The DbContextOptions for AppDbContext.</returns>
    public DbContextOptions<AppDbContext> GetDbContextOptions()
    {
        var connectionString = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")) ?
                                 AppSettings.ConnectionString : Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return optionsBuilder.Options;
    }
}