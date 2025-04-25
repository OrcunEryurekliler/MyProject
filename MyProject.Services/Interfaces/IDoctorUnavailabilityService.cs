using MyProject.Core.Entities;
using MyProject.Core.Enums;
using MyProject.Core.Interfaces;

public interface IDoctorUnavailabilityService : IService<DoctorUnavailability>
{
    Task<IEnumerable<int>> GetAvailableDoctorIdsBySpecializationAsync(Specialization specialization, DateTime startDate, DateTime endDate);
}
