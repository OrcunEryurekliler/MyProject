using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyProject.Core.Interfaces;

namespace MyProject.Infrastructure.Data.Repositories.Generic
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;
        public Repository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T Entity)
        {
            await _dbContext.Set<T>().AddAsync(Entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public void DeleteAsync(T Entity)
        {
            _dbContext.Set<T>().Remove(Entity);
        }

        void IRepository<T>.UpdateAsync(T Entity)
        {
            _dbContext.Set<T>().Update(Entity);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }
    }
}
