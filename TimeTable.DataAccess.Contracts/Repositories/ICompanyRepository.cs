using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.Contracts.Repositories
{
    public interface ICompanyRepository
    {
        Task<CompanyEntity> GetAsync();
        Task<CompanyEntity> AttachAsync(int id, byte[] rowVersion);
    }
}
