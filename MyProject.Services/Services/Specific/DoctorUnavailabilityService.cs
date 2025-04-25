using MyProject.Application.Services.Generic;
using MyProject.Core.Entities;
using MyProject.Core.Enums;
using MyProject.Core.Interfaces;

public class DoctorUnavailabilityService : Service<DoctorUnavailability> , IDoctorUnavailabilityService
{
    private readonly MyProject.Core.Interfaces.IDoctorUnavailabilityRepository _repository;

    public DoctorUnavailabilityService(MyProject.Core.Interfaces.IDoctorUnavailabilityRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<int>> GetAvailableDoctorIdsBySpecializationAsync(Specialization specialization, DateTime startDate, DateTime endDate)
    {
        return await _repository.GetAvailableDoctorIdsBySpecializationAsync(specialization, startDate, endDate);
    }
}
