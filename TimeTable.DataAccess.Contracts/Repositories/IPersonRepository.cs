using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.Contracts.Repositories
{
    public interface IPersonRepository
    {
        Task<bool> ExistsAsync(int id, string name);
        Task<int> GetTotalRecordsAsync();
        Task<IEnumerable<PersonEntity>> GetAllAsync(int pageSize, int pageNumber);
        Task<PersonEntity> GetAsync(int id);
        Task<PersonEntity> GetAsync(string userId);
        Task AddAsync(PersonEntity entity);
        Task<PersonEntity> AttachAsync(int id, byte[] rowVersion);
        Task DeleteAsync(int id, byte[] rowVersion);
    }
}
