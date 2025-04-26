using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data.Repositories.Generic;

namespace MyProject.Application.Services.Generic
{
    public class Service<T> : IService<T> where T : class
    {
        public readonly IRepository<T> _repository;
        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }
        public async Task<bool> AddAsync(T entity)
        {
            try 
            { 
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return true;
            }
            catch
            { return false; }
        }

        public async Task DeleteAsync(int i)
        {
            var entity = await _repository.GetAsync(i);
            _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetAsync(int i)
        {
           return await _repository.GetAsync(i);
        }

        public async Task UpdateAsync(T entity)
        {
             _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
