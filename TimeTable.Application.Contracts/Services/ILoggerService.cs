using System;
using System.Threading.Tasks;

namespace TimeTable.Application.Contracts.Services
{
    public interface ILoggerService
    {
        Task LogInformation(string info);
        Task LogError(Exception ex);
    }
}
