using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MyApp.Core.Entitties;

namespace MyApp.Infrastructure.Data;

public class AppDbContextSeed
{

    public static async Task SeedAsync(AppDbContext appDbContext, ILogger<AppDbContextSeed> logger)
    {

        try
        {
            if (!await appDbContext.Products.AnyAsync())
            {
                logger.LogInformation("Seeding initial data for Prodcuts ...");
                await appDbContext.Products.AddRangeAsync(GetPreconfiguredProdcuts());
                await appDbContext.SaveChangesAsync();
                logger.LogInformation("Seeding initial data for Prodcuts completed ...");
                return;
            }
            else
            {
                logger.LogInformation("Database already contains data. Seeding skipped.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");

            throw;
        }

    }


    private static IEnumerable<Product> GetPreconfiguredProdcuts()
    {
        return new List<Product>()
        {
             new Product() { Name = "Samsung Galaxy S20", Description = "Samsung Galaxy S20 5G 128GB", Price = 999.99m , CreatedBy = "1", CreatedDate = DateAndTime.Now},
            new Product() { Name = "Samsung Galaxy S20 Ultra", Description = "Samsung Galaxy S20 Ultra 5G 128GB", Price = 1299.99m , CreatedBy = "1", CreatedDate = DateAndTime.Now},
            new Product() { Name = "Samsung Galaxy Note 20", Description = "Samsung Galaxy Note 20 5G 128GB", Price = 999.99m, CreatedBy = "1", CreatedDate = DateAndTime.Now },
            new Product() { Name = "Samsung Galaxy Note 20 Ultra", Description = "Samsung Galaxy Note 20 Ultra 5G 128GB", Price = 1299.99m, CreatedBy = "1", CreatedDate = DateAndTime.Now },
            new Product() { Name = "Samsung Galaxy Z Fold 2", Description = "Samsung Galaxy Z Fold 2 5G 256GB", Price = 1999.99m, CreatedBy = "1", CreatedDate = DateAndTime.Now },
            new Product() { Name = "Samsung Galaxy Z Flip", Description = "Samsung Galaxy Z Flip 5G 256GB", Price = 1449.99m , CreatedBy = "1", CreatedDate = DateAndTime.Now},
            new Product() { Name = "Samsung Galaxy A71", Description = "Samsung Galaxy A71 128GB", Price = 499.99m , CreatedBy = "1", CreatedDate = DateAndTime.Now},
            new Product() { Name = "Samsung Galaxy A51", Description = "Samsung Galaxy A51 128GB", Price = 399.99m , CreatedBy = "1", CreatedDate = DateAndTime.Now},
            new Product() { Name = "Samsung Galaxy A21", Description = "Samsung Galaxy A21 32GB", Price = 249.99m , CreatedBy = "1", CreatedDate = DateAndTime.Now},
            new Product() { Name = "Samsung Galaxy A11", Description = "Samsung Galaxy A11 32GB", Price = 179.99m , CreatedBy = "1", CreatedDate = DateAndTime.Now}
        };
    }
}

