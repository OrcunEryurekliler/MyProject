using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyProject.Core.Interfaces;
using MyProject.Core.Models;
using MyProject.Infrastructure.Data.Context;

namespace MyProject.Infrastructure.Data.Repositories.EfRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }
        
        public async Task DeleteAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            _context.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                return role;
            }
            throw new NotImplementedException();
        }

       
    }
}
