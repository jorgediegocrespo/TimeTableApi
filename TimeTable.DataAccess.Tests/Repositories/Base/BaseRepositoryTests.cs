using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TimeTable.DataAccess.Tests.Repositories.Base
{
    public class BaseRepositoryTests
    {
        protected TimeTableDbContext GetInMemoryTimeTableDbContext(string dbContextName)
        {
            DbContextOptions<TimeTableDbContext> options = new DbContextOptionsBuilder<TimeTableDbContext>()
                .UseInMemoryDatabase(dbContextName)
                .Options;

            var dbContext = CreateDatabaseContext(options);
            //await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        }

        protected async Task<TimeTableDbContext> GetLocalDbTimeTableContext(string dbContextName)
        {
            string connectionString = $"Server=(localdb)\\mssqllocaldb;Database={dbContextName};Integrated Security=true";
            DbContextOptions<TimeTableDbContext> options = new DbContextOptionsBuilder<TimeTableDbContext>()
                .UseSqlServer(connectionString, options => options.UseNetTopologySuite())
                .Options;

            var dbContext = CreateDatabaseContext(options);
            await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        }

        private TimeTableDbContext CreateDatabaseContext(DbContextOptions<TimeTableDbContext> options)
        {
            return new TimeTableDbContext(options);
        }
    }
}
