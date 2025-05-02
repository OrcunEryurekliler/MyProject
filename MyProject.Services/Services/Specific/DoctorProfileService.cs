using MyProject.Application.DTOs;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Generic;
using MyProject.Core.Entities;
using MyProject.Core.Enums;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data.Repositories.Specific;

public class DoctorProfileService : Service<DoctorProfile> , IDoctorProfileService
{
    private readonly IDoctorProfileRepository _doctorRepository;
    private readonly IAppointmentService _appointmentService;

    public DoctorProfileService(IDoctorProfileRepository doctorRepository,
                                IAppointmentService appointmentService) : base(doctorRepository)
    {
        _doctorRepository = doctorRepository;
        _appointmentService = appointmentService;
    }

    public async Task<IEnumerable<DoctorProfile>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _doctorRepository.GetByIdsAsync(ids);
    }
    public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecializationAsync(int specializationId)
    {
        var doctors = await _doctorRepository.GetDoctorProfilesBySpecialization(specializationId);

        var doctorDtos = doctors.Select(doctor => new DoctorDto
        {
            Id = doctor.Id,
            FullName = doctor.User.Name,
            SpecializationName = doctor.Specialization.Name
        });

        return doctorDtos;
    }


}
