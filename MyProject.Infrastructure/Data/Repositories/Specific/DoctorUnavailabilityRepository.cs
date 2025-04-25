using MyProject.Core.Entities;
using MyProject.Core.Enums;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data.Repositories.Generic;
using MyProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class DoctorUnavailabilityRepository : Repository<DoctorUnavailability>, IDoctorUnavailabilityRepository
{
    private readonly AppDbContext _context;

    public DoctorUnavailabilityRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<bool> AnyAsync(Expression<Func<DoctorUnavailability, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<int>> GetAvailableDoctorIdsBySpecializationAsync(Specialization specialization, DateTime startDate, DateTime endDate)
    {
        // 1. Belirtilen branşta olan tüm doktorların ID’leri
        var allDoctorIds = await _context.DoctorProfiles
            .Where(d => d.Specialization == specialization)
            .Select(d => d.Id)
            .ToListAsync();

        // 2. Bu tarih aralığında uygun olmayan doktor ID'lerini bul
        var unavailableDoctorIds = await _context.DoctorUnavailabilities
            .Where(d => d.OffWorkDate >= startDate && d.OffWorkDate <= endDate)
            .Select(d => d.DoctorProfileId)
            .Distinct()
            .ToListAsync();

        // 3. Müsait olan doktorlar = Tüm doktorlar - uygun olmayanlar
        var availableDoctorIds = allDoctorIds.Except(unavailableDoctorIds);

        return availableDoctorIds;
    }
}
