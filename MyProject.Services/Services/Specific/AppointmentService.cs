using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Application.Helpers;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Generic;
using MyProject.Core.Entities;
using MyProject.Core.Enums;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Services.Specific
{
    public class AppointmentService: Service<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorProfileService _doctorService;
        private readonly IPatientProfileService _patientService;
        private readonly IMapper _mapper;
        public AppointmentService(IAppointmentRepository repository,
                                  IDoctorProfileService doctorProfileService,
                                  IPatientProfileService patientProfileService,
                                  IMapper mapper) : base(repository)
        {
            _appointmentRepository = repository;
            _doctorService = doctorProfileService;
            _patientService = patientProfileService;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Appointment>> GetAllAsyncByPatient(int Id)
        {
            return await _appointmentRepository.GetAllAsyncByPatient(Id);
        }
        public async Task<IEnumerable<Appointment>> GetAllAsyncByDoctor(int Id)
        {
            return await _appointmentRepository.GetAllAsyncByDoctor(Id);
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            var doctorAppointments = await _appointmentRepository.GetAllAsync(x =>x.DoctorProfileId == doctorId && x.StartTime == date);
            return doctorAppointments;
        }
        public async Task<IEnumerable<DoctorDto>> GetAvailableDoctorsAsync(int specializationId, DateTime date)
        {
            var doctors = await _doctorService.GetDoctorsBySpecializationAsync(specializationId);
            var availableDoctors = new List<DoctorDto>();

            foreach (var doctor in doctors)
            {
                var appointments = await _appointmentRepository.GetAppointmentsByDoctorAndDateAsync(doctor.Id, date);
                var slots = SlotGenerator.GenerateDailySlots();

                if (appointments.Count() < slots.Count) // en az 1 slot boş
                {
                    availableDoctors.Add(doctor); // zaten DoctorDto
                }
            }

            return availableDoctors;
        }




        public async Task<IEnumerable<TimeSpan>> GetAvailableTimeslotsAsync(int doctorId, DateTime date)
        {
            // Burada sabah 9-12:30 ve öğlen 13:30-17:30 slot üret
            var slots = new List<TimeSpan>();

            for (var time = new TimeSpan(9, 0, 0); time < new TimeSpan(12, 30, 0); time += TimeSpan.FromMinutes(30))
                slots.Add(time);

            for (var time = new TimeSpan(13, 30, 0); time < new TimeSpan(17, 30, 0); time += TimeSpan.FromMinutes(30))
                slots.Add(time);

            return slots;
        }
    }
}
