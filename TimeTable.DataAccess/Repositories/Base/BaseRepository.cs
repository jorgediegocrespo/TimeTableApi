using Microsoft.EntityFrameworkCore;

namespace TimeTable.DataAccess.Repositories.Base
{
    public abstract class BaseRepository<T>
        where T : class
    {
        protected readonly TimeTableDbContext dbContext;
        protected abstract DbSet<T> DbEntity { get; }

        public BaseRepository(TimeTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
