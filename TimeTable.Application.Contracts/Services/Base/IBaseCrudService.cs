using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.Business.Models.Base;

namespace TimeTable.Application.Contracts.Services.Base
{
    public interface IBaseCrudService<T>
        where T : IBaseBusinessModel
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> AddAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> UpdateAsync(T entity);
    }
}
