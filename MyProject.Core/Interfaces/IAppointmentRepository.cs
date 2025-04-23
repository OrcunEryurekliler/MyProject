using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Entities;

namespace MyProject.Core.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAllAsyncByPatient(int id);
        Task<IEnumerable<Appointment>> GetAllAsyncByDoctor(int id);
    }
}
