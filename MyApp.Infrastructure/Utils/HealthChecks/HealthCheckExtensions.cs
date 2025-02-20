using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;

namespace MyApp.Infrastructure.Utils.HealthChecks;

public class HealthCheckExtensions
{

    public static Task WriteHealthCheckResponse(HttpContext context, HealthReport result)
    {
        string json = JsonSerializer.Serialize(
          new
          {
              Status = result.Status.ToString(),
              Duration = result.TotalDuration,
              Info = result.Entries.Select(pair => new
              {
                  pair.Key,
                  pair.Value.Description,
                  pair.Value.Duration,
                  Status = Enum.GetName(typeof(HealthStatus), pair.Value.Status),
                  Error = pair.Value.Exception?.Message,
                  Data = pair.Value.Data.Select(p => new { p.Key, Value = p.Value.ToString() })
              })
          },
          new JsonSerializerOptions
          {
              WriteIndented = true,
              PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
              DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
          });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        return context.Response.WriteAsync(json);
    }

}
