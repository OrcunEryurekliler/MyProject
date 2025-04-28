using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyProject.Application.DTOs;
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
        private readonly IDoctorProfileRepository _doctorProfileRepository;
        private readonly IPatientProfileService _patientProfileService;
        private readonly IMapper _mapper;
        public AppointmentService(IAppointmentRepository repository,
                                  IDoctorProfileRepository doctorProfileRepository,
                                  IPatientProfileService patientProfileService,
                                  IMapper mapper) : base(repository)
        {
            _appointmentRepository = repository;
            _doctorProfileRepository = doctorProfileRepository;
            _patientProfileService = patientProfileService;
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

        Task<IEnumerable<AppointmentDto>> IAppointmentService.GetAllByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DoctorProfile>> GetAvailableDoctorsAsync(Specialization specialization, DateTime date)
        {
            var doctors = await _doctorProfileRepository.GetDoctorProfilesBySpecialization(specialization);
            return doctors;
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

        public async Task<int> GetPatientIdByUserId(int userId)
        {
            var user = await _patientProfileService.GetByUserIdAsync(userId);
            return user.Id;
        }
    }
}
