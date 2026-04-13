using Farscry.Api.Data;
using Farscry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Farscry.Api.Repositories;

public class FamilyRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task CreateFamilyAsync(Family family)
    {
        _context.Families.Add(family);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Family>> GetAllFamiliesAsync()
    {
        return await _context.Families.ToListAsync();
    }

    public async Task<Family?> GetFamilyByIdAsync(Guid id)
    {
        return await _context.Families.FirstOrDefaultAsync(family => family.Id == id);
    }

    public async Task UpdateFamilyAsync(Family family)
    {
        _context.Families.Update(family);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFamilyAsync(Family family)
    {
        _context.Families.Remove(family);
        await _context.SaveChangesAsync();
    }
}
