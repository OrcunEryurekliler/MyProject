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
        Task AddAsync(T entity);
        Task Delete(int i);
        Task Update(T entity);

    }
}
