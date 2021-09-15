using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTable.Business.Models.Base;

namespace TimeTable.Application.Contracts.Services.Base
{
    public interface IBaseCrudService<BR, DR, C, U>
        where BR : IBasicReadingBusinessModel
        where DR : IDetailedReadingBusinessModel
        where C : ICreationBusinessModel
        where U : IUpdatingBusinessModel
    {
        Task<IEnumerable<BR>> GetAllAsync();
        Task<DR> GetAsync(int id);
        Task<int> AddAsync(C businessModel);
        Task DeleteAsync(int id);
        Task UpdateAsync(U businessModel);
    }
}
