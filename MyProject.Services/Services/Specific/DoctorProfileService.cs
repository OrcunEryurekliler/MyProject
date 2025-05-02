using MyProject.Application.DTOs;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Generic;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

public class DoctorProfileService : Service<DoctorProfile>, IDoctorProfileService
{
    private readonly IDoctorProfileRepository _doctorRepository;

    public DoctorProfileService(IDoctorProfileRepository doctorRepository)
        : base(doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<IEnumerable<DoctorProfile>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _doctorRepository.GetByIdsAsync(ids);
    }

    public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecializationAsync(int specializationId)
    {
        var doctors = await _doctorRepository.GetDoctorProfilesBySpecialization(specializationId);

        return doctors.Select(doctor => new DoctorDto
        {
            Id = doctor.Id,
            FullName = doctor.User.Name,
            SpecializationName = doctor.Specialization.Name
        });
    }
}
