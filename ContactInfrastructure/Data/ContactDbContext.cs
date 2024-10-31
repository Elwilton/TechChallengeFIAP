using ContactCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ContactInfrastructure.Data;

public class ContactDbContext : DbContext
{
    private readonly string _connectionString;

    public ContactDbContext()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public ContactDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) 
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactDbContext).Assembly);
    }
}
