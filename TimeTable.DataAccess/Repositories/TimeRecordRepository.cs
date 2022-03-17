using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Extensions;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class TimeRecordRepository : BaseRepository<TimeRecordEntity>, ITimeRecordRepository
    {
        public TimeRecordRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }

        protected override DbSet<TimeRecordEntity> DbEntity => dbContext.TimeRecords;

        public async Task<bool> ExistsOverlappingAsync(int id, int personId, DateTimeOffset dateTime) => 
            await DbEntity.Where(x =>
                x.Id != id &&
                x.PersonId == personId &&
                DateTimeOffset.Compare(x.StartDateTime, dateTime) <= 0 &&
                x.EndDateTime.HasValue &&
                DateTimeOffset.Compare(x.EndDateTime.Value, dateTime) >= 0)
            .AnyAsync();

        public async Task<int> GetTotalRecordsAsync() => await DbEntity.CountAsync();

        public async Task<int> GetTotalRecordsAsync(int personId) => await DbEntity.CountAsync(x => x.PersonId == personId);

        public async Task<IEnumerable<TimeRecordEntity>> GetAllAsync(int pageSize, int pageNumber)
        {
            var result = await DbEntity.OrderBy(x => x.StartDateTime).Paginate(pageSize, pageNumber).ToListAsync();
            dbContext.ChangeTracker.Clear();

            return result;
        }

        public async Task<IEnumerable<TimeRecordEntity>> GetAllAsync(int personId, int pageSize, int pageNumber)
        {
            var result = await DbEntity.Where(x => x.PersonId == personId).OrderBy(x => x.StartDateTime).Paginate(pageSize, pageNumber).ToListAsync();
            dbContext.ChangeTracker.Clear();

            return result;
        }

        public async Task<TimeRecordEntity> GetAsync(int id)
        {
            var result = await DbEntity.FirstOrDefaultAsync(x => x.Id == id);
            dbContext.ChangeTracker.Clear();

            return result;
        }

        public async Task<TimeRecordEntity> GetAsync(int id, int personId)
        {
            var result = await DbEntity.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
            dbContext.ChangeTracker.Clear();

            return result;
        }

        public async Task AddAsync(TimeRecordEntity entity)
        {
            await DbEntity.AddAsync(entity);
        }

        public Task<TimeRecordEntity> AttachAsync(int id, byte[] rowVersion)
        {
            var entity = new TimeRecordEntity { Id = id, RowVersion = rowVersion };
            DbEntity.Attach(entity);

            return Task.FromResult(entity);
        }

        public async Task DeleteAsync(int id, byte[] rowVersion)
        {
            await DbEntity.RemoveConcurrently(id, rowVersion);
        }
    }
}
