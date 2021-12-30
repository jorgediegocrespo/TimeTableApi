using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.Contracts.Repositories
{
    public interface ITimeRecordRepository
    {
        Task<bool> ExistsOverlappingAsync(int id, DateTimeOffset dateTime);
        Task<IEnumerable<TimeRecordEntity>> GetAllAsync();
        Task<IEnumerable<TimeRecordEntity>> GetAllAsync(int personId);
        Task<TimeRecordEntity> GetAsync(int id);
        Task<TimeRecordEntity> GetAsync(int id, int personId);
        Task AddAsync(TimeRecordEntity entity);
        Task UpdateAsync(TimeRecordEntity entity);
        Task DeleteAsync(int id);
    }
}
