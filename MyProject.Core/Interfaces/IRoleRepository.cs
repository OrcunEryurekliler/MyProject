using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Models;

namespace MyProject.Core.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> GetAsync(int id);
        Task<Role> UpdateAsync(Role role);
        Task<Role> CreateAsync(Role role);
        Task DeleteAsync(int id);
    }
}
