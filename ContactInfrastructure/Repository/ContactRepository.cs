using ContactCore.Entity;
using ContactCore.Interfaces;
using ContactInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactInfrastructure.Repository;

public class ContactRepository : IContactRepository
{
    private readonly ContactDbContext _context;

    public ContactRepository(ContactDbContext context)
    {
        _context = context;
    }

    public async Task<Contact?> GetByIdAsync(int id)
    {
        return await _context.Contacts.FindAsync(id);
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        return await _context.Contacts.ToListAsync();
    }

    public async Task<IEnumerable<Contact>> GetByDDDAsync(string ddd)
    {
        return await _context.Contacts
            .Where(c => c.DDD == ddd)
            .ToListAsync();
    }

    public async Task<Contact> AddAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task UpdateAsync(Contact contact)
    {
        contact.UpdatedAt = DateTime.UtcNow;
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var contact = await GetByIdAsync(id);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }
}
