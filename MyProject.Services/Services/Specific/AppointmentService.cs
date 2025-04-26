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
        public AppointmentService(IAppointmentRepository repository,
                                  IMapper mapper) : base(repository)
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

        Task<IEnumerable<AppointmentDto>> IAppointmentService.GetAllByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
