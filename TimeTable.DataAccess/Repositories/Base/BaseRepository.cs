using Microsoft.EntityFrameworkCore;
using TimeTable.DataAccess.Contracts.Entities.Base;
using TimeTable.DataAccess.Contracts.Repositories.Base;

namespace TimeTable.DataAccess.Repositories.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : class, IBaseEntity
    {
        protected readonly TimeTableDbContext dbContext;
        protected abstract DbSet<T> DbEntity { get; }

        public BaseRepository(TimeTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
