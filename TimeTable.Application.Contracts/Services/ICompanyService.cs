using System.Threading.Tasks;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Business.Models;

namespace TimeTable.Application.Contracts.Services
{
    public interface ICompanyService : IBaseService
    {
        Task<Company> GetAsync();
        Task UpdateAsync(Company businessModel);
    }
}
