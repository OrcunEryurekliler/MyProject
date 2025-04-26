using MyProject.Core.Entities;
using MyProject.Core.Enums;

namespace MyProject.Core.Interfaces
{
    public interface IDoctorProfileRepository : IRepository<DoctorProfile>
    {
        Task<IEnumerable<DoctorProfile>> GetByIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<DoctorProfile>> GetDoctorProfilesBySpecialization(Specialization specialization);
    }
}
