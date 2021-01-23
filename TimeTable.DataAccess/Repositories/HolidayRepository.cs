using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class HolidayRepository : BaseRepository<HolidayEntity>, IHolidayRepository
    {
        protected override DbSet<HolidayEntity> DbEntity => dbContext.Holidays;

        public HolidayRepository(ITimeTableDbContext dbContext) : base(dbContext)
        { }

        public override async Task<HolidayEntity> GetAsync(int id)
        {
            return await dbContext.Holidays
                .Include(x => x.PersonRequesting)
                .Include(x => x.ConfirmingPerson)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
