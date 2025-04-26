using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Core.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<T> GetAsync(int i);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task DeleteAsync(int i);
        Task UpdateAsync(T entity);

    }
}
