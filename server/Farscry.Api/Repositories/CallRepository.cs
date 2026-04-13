
using Farscry.Api.Data;
using Farscry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Farscry.Api.Repositories;

public class CallRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task CreateCallAsync(Call call)
    {
        _context.Calls.Add(call);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Call>> GetAllCallsAsync()
    {
        return await _context.Calls.ToListAsync();
    }

    public async Task<Call?> GetCallByIdAsync(Guid id)
    {
        return await _context.Calls.FirstOrDefaultAsync(call => call.Id == id);
    }

    public async Task UpdateCallAsync(Call call)
    {
        _context.Calls.Update(call);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCallAsync(Call call)
    {
        _context.Calls.Remove(call);
        await _context.SaveChangesAsync();
    }
}
