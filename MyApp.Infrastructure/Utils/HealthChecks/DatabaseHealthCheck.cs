using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyApp.Infrastructure.Utils.HealthChecks;

/// <summary>
/// Health check for the database connection.
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseHealthCheck"/> class.
    /// </summary>
    /// <param name="configuration">The configuration to retrieve the connection string.</param>
    /// <exception cref="ArgumentNullException">Thrown when the connection string is null or empty.</exception>
    public DatabaseHealthCheck(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DatabaseSettings");
        if (string.IsNullOrEmpty(_connectionString))
            throw new ArgumentNullException(nameof(_connectionString), "ConnectionString string is missing ");
    }

    /// <summary>
    /// Checks the health of the database connection.
    /// </summary>
    /// <param name="context">The context in which the health check is being run.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the health check.</param>
    /// <returns>A <see cref="HealthCheckResult"/> indicating the health of the database connection.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        await sqlConnection.OpenAsync(cancellationToken);
        using var command = sqlConnection.CreateCommand();
        command.CommandText = "SELECT 1";
        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Health check failed", ex);
        }
    }
}
