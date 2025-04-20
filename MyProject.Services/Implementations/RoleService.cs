using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Interfaces;
using MyProject.Core.Models;
using MyProject.Infrastructure.Data.Repositories.EfRepository;
using MyProject.Services.Interfaces;

namespace MyProject.Services.Implementations
{
    public class RoleService : IRoleService
    {
        public readonly IRoleRepository _roleRepository;

        public RoleService (IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<Role> CreateAsync(Role role)
        {
            await _roleRepository.CreateAsync(role);
            return role;
        }

        public async Task DeleteAsync(int id)
        {
            await _roleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<Role> GetAsync(int id)
        {
            var role = await _roleRepository.GetAsync(id);
            return role ?? throw new ArgumentNullException(nameof(role));

        }

        public async Task<Role> UpdateAsync(Role role)
        {
            return await _roleRepository.UpdateAsync(role);
        }
    }
}
