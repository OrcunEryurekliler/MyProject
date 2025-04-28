using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Interfaces
{
    public interface IPatientProfileService : IService<PatientProfile>
    {
        Task<PatientProfile> GetByUserIdAsync(int userId);
    }
}
