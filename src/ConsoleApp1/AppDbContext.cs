using Microsoft.EntityFrameworkCore;

internal class AppDbContext : DbContext
{
    public DbSet<History> Histories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = @"Server=127.0.0.1;Database=poc;Uid=root;Pwd=root";
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(s => Console.WriteLine(s));
        optionsBuilder.UseMySql(connectionString,
            serverVersion,
            options => options.UseMicrosoftJson());
    }
}
