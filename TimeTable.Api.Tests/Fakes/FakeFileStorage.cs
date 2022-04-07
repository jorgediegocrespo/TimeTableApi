using System;
using System.Threading.Tasks;
using TimeTable.Application.Contracts.Services;

namespace TimeTable.Api.Tests.Fakes
{
    public class FakeFileStorage : IFileStorage
    {
        public Task DeleteFileAsync(string container, string path)
        {
            return Task.CompletedTask;
        }

        public Task<string> SaveFileAsync(byte[] content, string extension, string container, string contentType)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        public Task<string> UpdateFileAsync(byte[] content, string extension, string container, string path, string contentType)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
    }
}
