using System.Threading.Tasks;
using TimeTable.Application.Contracts.Services.Base;
using TimeTable.Business.Models;

namespace TimeTable.Application.Contracts.Services
{
    public interface IPersonService : IBaseService
    {
        Task<PaginatedResponse<ReadingPerson>> GetAllAsync(PaginationRequest request);
        Task<ReadingPerson> GetAsync(int id);
        Task<ReadingPerson> GetOwnAsync();
        Task<int> AddAsync(CreatingPerson businessModel);
        Task UpdateAsync(UpdatingPerson businessModel);
        Task DeleteAsync(int id, byte[] rowVersion);
        Task DeleteOwnAsync(byte[] rowVersion);
    }
}
