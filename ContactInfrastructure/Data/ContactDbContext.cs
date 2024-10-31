using ContactCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace ContactInfrastructure.Data;

public class ContactDbContext : DbContext
{
    private readonly string _connectionString;

    public ContactDbContext()
    {
        
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
