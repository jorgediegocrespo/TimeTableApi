using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TimeTable.Application.Contracts.Services;

namespace TimeTable.Application.Services
{
    public class LoggerService : ILoggerService
    {
        //TODO Use app insights
        public Task LogError(Exception ex)
        {
            Debug.WriteLine(ex?.Message);
            return Task.CompletedTask;
        }

        public Task LogInformation(string info)
        {
            Debug.WriteLine(info);
            return Task.CompletedTask;
        }
    }
}
