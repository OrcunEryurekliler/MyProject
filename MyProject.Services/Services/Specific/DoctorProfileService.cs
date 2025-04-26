using MyProject.Application.Interfaces;
using MyProject.Application.Services.Generic;
using MyProject.Core.Entities;
using MyProject.Core.Enums;
using MyProject.Core.Interfaces;

public class DoctorProfileService : Service<DoctorProfile> , IDoctorProfileService
{
    private readonly IDoctorProfileRepository _repository;

    public DoctorProfileService(IDoctorProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DoctorProfile>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _repository.GetByIdsAsync(ids);
    }
}
