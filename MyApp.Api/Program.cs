
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MyApp.Api.Extensions;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Utils.HealthChecks;

namespace MyApp.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure Infrastructure layer Services    
            builder.Services.AddInfrastructureServices(builder.Configuration);
            // Configure Application Layer Services  
            builder.Services.AddApplicationService(builder.Configuration);

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("AllowAngularApp", builderPolicy =>
                {
                    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
                    if (allowedOrigins != null)
                    {
                        builderPolicy.WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        //.AllowCredentials(); // Add this line to allow credentials
                    }
                });
            });

            var app = builder.Build();

            await app.MigrateDatabaseAsync<AppDbContext>();
            await app.SeedDatabaseAsync<AppDbContext>(async (appdbcontext, ServiceProvider) =>
            {
                ILogger<AppDbContextSeed> logger = ServiceProvider.GetRequiredService<ILogger<AppDbContextSeed>>();
                await AppDbContextSeed.SeedAsync(appdbcontext, logger);
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Add Global Exception Handling Middleware
            app.UseGlobalExceptionHanding();

            app.UseCors("AllowAngularApp");
            app.UseRouting();
            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = HealthCheckExtensions.WriteHealthCheckResponse
            });

            app.Run();
        }
    }
}
