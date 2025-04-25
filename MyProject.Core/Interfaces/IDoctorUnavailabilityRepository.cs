using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Enums;

namespace MyProject.Core.Interfaces
{
    public interface IDoctorUnavailabilityRepository : IRepository<DoctorUnavailability>
    {
        Task<IEnumerable<int>> GetAvailableDoctorIdsBySpecializationAsync(Specialization specialization, DateTime startDate, DateTime endDate);
        Task<bool> AnyAsync(Expression<Func<DoctorUnavailability, bool>> predicate);
    }
}
