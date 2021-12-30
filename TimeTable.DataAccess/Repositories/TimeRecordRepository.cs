using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class TimeRecordRepository : BaseRepository<TimeRecordEntity>, ITimeRecordRepository
    {
        public TimeRecordRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }

        protected override DbSet<TimeRecordEntity> DbEntity => dbContext.TimeRecords;

        public async Task<bool> ExistsOverlappingAsync(int id, DateTimeOffset dateTime) =>
            await DbEntity.AnyAsync(x =>
                x.Id != id &&
                x.StartDateTime.UtcDateTime <= dateTime.UtcDateTime &&
                x.EndDateTime != null &&
                x.EndDateTime.UtcDateTime >= dateTime.UtcDateTime);

        public async Task<IEnumerable<TimeRecordEntity>> GetAllAsync() => await DbEntity.ToListAsync();

        public async Task<IEnumerable<TimeRecordEntity>> GetAllAsync(int personId) => await DbEntity.Where(x => x.PersonId == personId).ToListAsync();

        public async Task<TimeRecordEntity> GetAsync(int id) => await DbEntity.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<TimeRecordEntity> GetAsync(int id, int personId) => await DbEntity.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);

        public async Task AddAsync(TimeRecordEntity entity)
        {
            await DbEntity.AddAsync(entity);
        }

        public Task UpdateAsync(TimeRecordEntity entity)
        {
            DbEntity.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entityToRemove = await DbEntity.SingleAsync(x => x.Id == id);
            DbEntity.Remove(entityToRemove);
        }
    }
}
