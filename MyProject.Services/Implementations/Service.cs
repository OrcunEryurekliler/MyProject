using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data.Repositories.Generic;

namespace MyProject.Services.Implementations
{
    public class Service<T> : IService<T> where T : class
    {
        protected readonly IRepository<T> _repository;
        public Service(Repository<T> repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task Delete(int i)
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

        public async Task Update(T entity)
        {
             _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
