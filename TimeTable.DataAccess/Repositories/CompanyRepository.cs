using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class CompanyRepository : BaseRepository<CompanyEntity>, ICompanyRepository
    {
        protected override DbSet<CompanyEntity> DbEntity => dbContext.Companies;

        public CompanyRepository(ITimeTableDbContext dbContext) : base(dbContext)
        { }

        public override async Task<CompanyEntity> GetAsync(int id)
        {
            return await dbContext.Companies
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
