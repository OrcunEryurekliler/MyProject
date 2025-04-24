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
using MyProject.Core.Interfaces;

namespace MyProject.Application.Services.Specific
{
    public class AppointmentService: Service<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        public AppointmentService(IAppointmentRepository repository, IMapper mapper) : base(repository)
        {
            _appointmentRepository = repository;
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

        public async Task<IEnumerable<AppointmentDto>> GetAllByPatientDtoAsync(int patientId)
        {
            var entities = await _appointmentRepository.GetAllAsyncByPatient(patientId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(entities);
            
        }

        public Task<AppointmentDto> GetDtoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AppointmentDto> CreateDtoAsync(CreateAppointmentDto dto, int patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllByDoctorDtoAsync(int doctorId)
        {
            var entities = await _appointmentRepository.GetAllAsyncByPatient(doctorId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(entities);
        }
    }
}
