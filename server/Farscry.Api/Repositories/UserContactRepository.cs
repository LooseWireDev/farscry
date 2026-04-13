using Farscry.Api.Data;
using Farscry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Farscry.Api.Repositories;

public class UserContactRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task CreateUserContactAsync(UserContact userContact)
    {
        _context.UserContacts.Add(userContact);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserContact>> GetAllUserContactsAsync()
    {
        return await _context.UserContacts.ToListAsync();
    }

    public async Task<UserContact?> GetUserContactByIdAsync(Guid id)
    {
        return await _context.UserContacts.FirstOrDefaultAsync(userContact => userContact.Id == id);
    }

    public async Task UpdateUserContactAsync(UserContact userContact)
    {
        _context.UserContacts.Update(userContact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserContactAsyn(UserContact userContact)
    {
        _context.UserContacts.Remove(userContact);
        await _context.SaveChangesAsync();
    }
}
