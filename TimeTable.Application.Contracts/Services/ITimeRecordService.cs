using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Business.Models;

namespace TimeTable.Application.Contracts.Services
{
    public interface ITimeRecordService : IBaseService
    {
        Task<IEnumerable<ReadingTimeRecord>> GetAllAsync();
        Task<IEnumerable<ReadingTimeRecord>> GetAllOwnAsync();
        Task<ReadingTimeRecord> GetAsync(int id);
        Task<ReadingTimeRecord> GetOwnAsync(int id);
        Task<int> AddAsync(CreatingTimeRecord businessModel);
        Task UpdateAsync(UpdatingTimeRecord businessModel);
        Task DeleteAsync(int id);
    }
}
