using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Application.DTOs;
using MyProject.Core.Enums;

namespace MyProject.Application.Interfaces
{
    public interface IAppointmentService : IService<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAllAsyncByPatient(int Id);
        Task<IEnumerable<Appointment>> GetAllAsyncByDoctor(int Id);
        Task<IEnumerable<AppointmentDto>> GetAllByDoctorAndDateAsync(int doctorId, DateTime date);
        //Randevu alma ekranı için gereki olanlar
        Task<IEnumerable<DoctorProfile>> GetAvailableDoctorsAsync(Specialization specialization, DateTime date);
        Task<IEnumerable<TimeSpan>> GetAvailableTimeslotsAsync(int doctorId, DateTime date);
        Task<int> GetPatientIdByUserId(int userId);

    }
}
