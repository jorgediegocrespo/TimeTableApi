using System.Threading.Tasks;
using TimeTable.Business.Models;

namespace TimeTable.Application.Contracts.Services
{
    public interface ICompanyService 
    {
        Task<BasicReadingCompany> GetAsync();
    }
}
