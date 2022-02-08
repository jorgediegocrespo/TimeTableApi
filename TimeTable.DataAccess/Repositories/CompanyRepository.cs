using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class CompanyRepository : BaseRepository<CompanyEntity>, ICompanyRepository
    {
        public CompanyRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }

        protected override DbSet<CompanyEntity> DbEntity => dbContext.Companies;

        public async Task<CompanyEntity> GetAsync()
        {
            var result = await DbEntity.FirstOrDefaultAsync();
            dbContext.ChangeTracker.Clear();

            return result;
        }

        public Task<CompanyEntity> AttachAsync(int id, byte[] rowVersion)
        {
            var entity = new CompanyEntity { Id = id, RowVersion = rowVersion };
            DbEntity.Attach(entity);

            return Task.FromResult(entity);
        }
    }
}
