using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Enums;

namespace MyProject.Core.Interfaces
{
    public interface IDoctorProfileRepository : IRepository<DoctorProfile>
    {
        Task<IEnumerable<DoctorProfile>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
