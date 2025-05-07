using System.Linq;
using AutoMapper;
using MyProject.Application.DTO;
using MyProject.Application.DTOs;
using MyProject.Application.Helpers;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Generic;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

public class AppointmentService : Service<Appointment>, IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorProfileRepository _doctorProfileRepository;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository appointmentRepository,
                              IDoctorProfileRepository doctorProfileRepository,
                              IMapper mapper)
        : base(appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
        _doctorProfileRepository = doctorProfileRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsyncByPatient(int id)
    {
        return await _appointmentRepository.GetAllAsyncByPatient(id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAsyncByDoctor(int id)
    {
        return await _appointmentRepository.GetAllAsyncByDoctor(id);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date)
    {
        return await _appointmentRepository.GetAllAsync(x =>
            x.DoctorProfileId == doctorId && x.StartTime.Date == date.Date);
    }

    public async Task<IEnumerable<DoctorDto>> GetAvailableDoctorsAsync(int specializationId, DateTime date)
    {
        var doctors = await _doctorProfileRepository.GetDoctorProfilesBySpecialization(specializationId);
        var availableDoctors = new List<DoctorDto>();
        var slots = SlotGenerator.GenerateDailySlots();

        foreach (var doctor in doctors)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByDoctorAndDateAsync(doctor.Id, date);
            if (appointments.Count() < slots.Count)
            {
                availableDoctors.Add(new DoctorDto
                {
                    Id = doctor.Id,
                    FullName = doctor.User.Name,
                    SpecializationName = doctor.Specialization.Name
                });
            }
        }

        return availableDoctors;
    }

    public async Task<IEnumerable<AppointmentSlotDto>> GetAvailableTimeslotsAsync(int doctorId, DateTime date)
    {
        var allSlots = SlotGenerator.GenerateDailySlots();  // List<TimeSpan>
        var appointments = await _appointmentRepository
                                 .GetAppointmentsByDoctorAndDateAsync(doctorId, date);
        var bookedTimes = appointments
                            .Select(a => a.StartTime.TimeOfDay)
                            .ToHashSet();

        return allSlots.Select((slot, idx) => new AppointmentSlotDto
        {
            Id = idx,
            SlotTime = slot,
            IsAvailable = !bookedTimes.Contains(slot)
        })
        .ToList();
    }




}
