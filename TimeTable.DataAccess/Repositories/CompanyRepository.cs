using Microsoft.EntityFrameworkCore;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories;
using TimeTable.DataAccess.Repositories.Base;

namespace TimeTable.DataAccess.Repositories
{
    public class CompanyRepository : BaseRepository<CompanyEntity>, ICompanyRepository
    {
        protected override DbSet<CompanyEntity> DbEntity => dbContext.Companies;

        public CompanyRepository(TimeTableDbContext dbContext) : base(dbContext)
        { }
    }
}
