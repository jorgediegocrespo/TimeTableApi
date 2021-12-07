using System.Threading.Tasks;
using TimeTable.Business.Models;

namespace TimeTable.Application.Contracts.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterUserInfo userInfo);
        Task<string> LoginAsync(LoginUserInfo userInfo);
        string GetContextUserId();
        Task<int?> GetContextPersonIdAsync();
        string GetContextUserName();
        string GetContextUserEmail();
        Task DeleteAsync(string userId);
    }
}
