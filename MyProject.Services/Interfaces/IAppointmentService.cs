using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Application.DTOs;

namespace MyProject.Application.Interfaces
{
    public interface IAppointmentService : IService<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAllAsyncByPatient(int Id);
        Task<IEnumerable<Appointment>> GetAllAsyncByDoctor(int Id);
        Task<IEnumerable<AppointmentDto>> GetAllByPatientDtoAsync(int patientId);
        Task<IEnumerable<AppointmentDto>> GetAllByDoctorDtoAsync(int doctorId);
        Task<AppointmentDto> CreateAppointmentDtoAsync(CreateAppointmentDto dto, int patientId);
        Task<IEnumerable<TimeSpan>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date);

    }
}
