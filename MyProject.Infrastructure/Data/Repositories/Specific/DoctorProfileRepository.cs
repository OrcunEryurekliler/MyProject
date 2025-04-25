using MyProject.Core.Entities;
using MyProject.Core.Enums;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data.Repositories.Generic;
using MyProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class DoctorProfileRepository : Repository<DoctorProfile>, IDoctorProfileRepository
{
    private readonly AppDbContext _context;

    public DoctorProfileRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorProfile>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _context.Set<DoctorProfile>().Where(d => ids.Contains(d.Id)).ToListAsync();
    }
}
