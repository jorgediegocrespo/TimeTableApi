using Microsoft.EntityFrameworkCore;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class PersonRepository : BaseRepository<PersonEntity>, IPersonRepository
    {
        protected override DbSet<PersonEntity> DbEntity => dbContext.People;

        public PersonRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }
    }
}
