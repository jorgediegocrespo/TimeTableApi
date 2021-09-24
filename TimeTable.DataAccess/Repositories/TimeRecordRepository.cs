using Microsoft.EntityFrameworkCore;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class TimeRecordRepository : BaseRepository<TimeRecordEntity>, ITimeRecordRepository
    {
        protected override DbSet<TimeRecordEntity> DbEntity => dbContext.TimeRecords;

        public TimeRecordRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }
    }
}
