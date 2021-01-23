using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class BankDayRepository : BaseRepository<BankDayEntity>, IBankDayRepository
    {
        protected override DbSet<BankDayEntity> DbEntity => dbContext.BankDays;

        public BankDayRepository(ITimeTableDbContext dbContext) : base(dbContext)
        { }

        public override async Task<BankDayEntity> GetAsync(int id)
        {
            return await dbContext.BankDays
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
