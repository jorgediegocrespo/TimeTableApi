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

        public virtual async Task AddAsync(CompanyEntity entity)
        {
            await DbEntity.AddAsync(entity);
        }

        public Task<CompanyEntity> GetAsync() => DbEntity.FirstOrDefaultAsync();

        public virtual Task UpdateAsync(CompanyEntity entity)
        {
            DbEntity.Update(entity);
            return Task.CompletedTask;
        }
    }
}
