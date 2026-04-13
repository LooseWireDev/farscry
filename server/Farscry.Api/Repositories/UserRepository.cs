using Farscry.Api.Data;
using Farscry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Farscry.Api.Repositories;

public class UserRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
