using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class TimeRecordRepository : BaseRepository<TimeRecordEntity>, ITimeRecordRepository
    {
        protected override DbSet<TimeRecordEntity> DbEntity => dbContext.TimeRecords;

        public TimeRecordRepository(ITimeTableDbContext dbContext) : base(dbContext)
        { }

        public override async Task<TimeRecordEntity> GetAsync(int id)
        {
            return await dbContext.TimeRecords
                .Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
