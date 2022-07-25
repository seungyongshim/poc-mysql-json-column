using ConsoleApp3.Actors;
using Domain;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp3;

public class AppDbContext : DbContext
{
    public DbSet<Entity<Human>> Persons { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveConversion<DateTimeUtcConverter>();

        base.ConfigureConventions(configurationBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        OnBeforeSaving();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess,
            cancellationToken);
    }

    private void OnBeforeSaving()
    {
        var entries = ChangeTracker.Entries();
        var utcNow = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            switch ((entry.Entity, entry.State))
            {
                case (ITrackableEntity entity, EntityState.Modified):
                    entity.UpdatedAt = utcNow;
                    entry.Property("CreatedAt").IsModified = false;
                    break;
                case (ITrackableEntity entity, EntityState.Added):
                    entity.CreatedAt = utcNow;
                    entity.UpdatedAt = utcNow;
                    break;
            }
        }
    }
}
