using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data.Repositories.Generic;

namespace MyProject.Infrastructure.Data.Repositories.Specific
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        
        public AppointmentRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Appointment>> GetAllAsyncByDoctor(int id)
        {
            return await _dbContext.Appointments
                                   .Include(a => a.PatientProfile)
                                   .Include(a => a.DoctorProfile)
                                   .Where(a => a.DoctorProfileId == id)
                                   .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAsyncByPatient(int id)
        {
            return await _dbContext.Appointments
                                   .Include(a => a.PatientProfile)
                                   .Include(a => a.DoctorProfile)
                                   .Where(a => a.PatientProfileId == id)
                                   .ToListAsync();
        }

        public async Task<List<TimeSpan>> GetTakenSlotsAsync(int doctorId, DateTime date)
        {
            return await _dbContext.Appointments
                .Where(x => x.DoctorProfileId == doctorId && x.StartTime.Date == date.Date)
                .Select(x => x.StartTime.TimeOfDay)
                .ToListAsync();
        }

    }
}
