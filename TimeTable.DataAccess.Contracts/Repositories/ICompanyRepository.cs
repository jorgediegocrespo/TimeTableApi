using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Repositories.Base;

namespace TimeTable.DataAccess.Contracts.Repositories
{
    public interface ICompanyRepository : IBaseRepository<CompanyEntity>
    {
        Task<CompanyEntity> GetAsync();
        Task UpdateAsync(CompanyEntity entity);
    }
}
