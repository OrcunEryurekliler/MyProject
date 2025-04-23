using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Interfaces
{
    public interface IAppointmentService : IService<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAllAsyncByPatient(int Id);
        Task<IEnumerable<Appointment>> GetAllAsyncByDoctor(int Id);
    }
}
