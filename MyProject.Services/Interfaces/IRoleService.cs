using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Models;

namespace MyProject.Services.Interfaces
{
    public interface IRoleService 
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> GetAsync(int id);
        Task<Role> CreateAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task DeleteAsync(int id);
    }
}
