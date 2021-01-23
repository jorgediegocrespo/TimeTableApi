using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities.Base;

namespace TimeTable.DataAccess.Contracts.Repositories.Base
{
    public interface IRepository<T> where T : IBaseWithIdEntity
    {
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task DeleteAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<T> AddAsync(T entity);
    }
}
