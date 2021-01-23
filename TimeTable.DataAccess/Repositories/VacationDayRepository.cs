using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class VacationDayRepository : BaseRepository<VacationDayEntity>, IVacationDayRepository
    {
        protected override DbSet<VacationDayEntity> DbEntity => dbContext.VacationDays;

        public VacationDayRepository(ITimeTableDbContext dbContext) : base(dbContext)
        { }

        public override async Task<VacationDayEntity> GetAsync(int id)
        {
            return await dbContext.VacationDays
                .Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
