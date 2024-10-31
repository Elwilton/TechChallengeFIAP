using ContactCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace ContactInfrastructure.Configuration;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Phone).IsRequired().HasMaxLength(9);
        builder.Property(e => e.DDD).IsRequired().HasMaxLength(2);
    }
}
