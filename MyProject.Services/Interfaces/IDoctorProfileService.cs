using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Application.DTOs;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data.Repositories.Generic;

namespace MyProject.Application.Interfaces
{
    public interface IDoctorProfileService : IService<DoctorProfile>
    {
        Task<IEnumerable<DoctorProfile>> GetByIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<DoctorDto>> GetDoctorsBySpecializationAsync(int specializationId);
        

    }
}
