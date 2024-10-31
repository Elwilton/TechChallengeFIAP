using ContactCore.Entity;

namespace ContactCore.Interfaces;

public interface IContactRepository
{
    Task<Contact?> GetByIdAsync(int id);
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<IEnumerable<Contact>> GetByDDDAsync(string ddd);
    Task<Contact> AddAsync(Contact contact);
    Task UpdateAsync(Contact contact);
    Task DeleteAsync(int id);
}
