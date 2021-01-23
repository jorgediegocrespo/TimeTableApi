using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class PersonRepository : BaseRepository<PersonEntity>, IPersonRepository
    {
        protected override DbSet<PersonEntity> DbEntity => dbContext.People;

        public PersonRepository(ITimeTableDbContext dbContext) : base(dbContext)
        { }

        public override async Task<PersonEntity> GetAsync(int id)
        {
            return await dbContext.People
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
