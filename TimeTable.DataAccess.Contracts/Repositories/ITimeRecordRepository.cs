using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.Contracts.Repositories
{
    public interface ITimeRecordRepository
    {
        Task<bool> ExistsOverlappingAsync(int id, DateTimeOffset dateTime);

        Task<int> GetTotalRecordsAsync();
        Task<int> GetTotalRecordsAsync(int personId);
        Task<IEnumerable<TimeRecordEntity>> GetAllAsync(int pageSize, int pageNumber);
        Task<IEnumerable<TimeRecordEntity>> GetAllAsync(int pageSize, int pageNumber, int personId);
        Task<TimeRecordEntity> GetAsync(int id);
        Task<TimeRecordEntity> GetAsync(int id, int personId);
        Task AddAsync(TimeRecordEntity entity);
        Task<TimeRecordEntity> AttachAsync(int id, byte[] rowVersion);
        Task DeleteAsync(int id, byte[] rowVersion);
    }
}
