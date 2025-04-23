using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Generic;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Services.Specific
{
    public class AppointmentService: Service<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentService(IAppointmentRepository repository) : base(repository)
        {
            _appointmentRepository = repository;
        }
               
        public async Task<IEnumerable<Appointment>> GetAllAsyncByPatient(int Id)
        {
            return await _appointmentRepository.GetAllAsyncByPatient(Id);
        }
        public async Task<IEnumerable<Appointment>> GetAllAsyncByDoctor(int Id)
        {
            return await _appointmentRepository.GetAllAsyncByDoctor(Id);
        }

    }
}
