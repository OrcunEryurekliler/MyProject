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
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;

        public UserService (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> CreateAsync(User user)
        {
            await _userRepository.CreateAsync(user);
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            return user ?? throw new ArgumentNullException(nameof(user));

        }

        public async Task<User> UpdateAsync(User user)
        {
            return await _userRepository.UpdateAsync(user);
        }
    }
}
