using Microsoft.EntityFrameworkCore;
using MyApp.Core.Entitties;
using System.Reflection;

namespace MyApp.Infrastructure.Data;

/// <summary>
/// Represents the database context for the application.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the Products table.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types
    /// exposed in <see cref="DbSet{TEntity}"/> properties on this context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }



    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity<int>
                || e.Entity is BaseEntity<long> ||
                (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));
        entries.ToList().ForEach(entry =>
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    ((BaseEntity<int>)entry.Entity).LastModifiedDate = DateTime.Now;
                    ((BaseEntity<int>)entry.Entity).LastModifiedBy = "1"; //TODO : Replace with current user

                    break;


                case EntityState.Added:
                    ((BaseEntity<int>)entry.Entity).CreatedDate = DateTime.Now;
                    ((BaseEntity<int>)entry.Entity).LastModifiedBy = "1"; //TODO : Replace with current user
                    break;
                default:
                    break;
            }
        });
        return base.SaveChangesAsync(cancellationToken);
    }
}
