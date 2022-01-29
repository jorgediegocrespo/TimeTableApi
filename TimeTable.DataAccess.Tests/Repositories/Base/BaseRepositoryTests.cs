using Microsoft.EntityFrameworkCore;

namespace TimeTable.DataAccess.Tests.Repositories.Base
{
    public class BaseRepositoryTests
    {
        protected TimeTableDbContext GetTimeTableDbContext(string dbContextName)
        {
            DbContextOptions<TimeTableDbContext> options = new DbContextOptionsBuilder<TimeTableDbContext>()
                .UseInMemoryDatabase(dbContextName)
                .Options;

            return new TimeTableDbContext(options);
        }
    }
}
