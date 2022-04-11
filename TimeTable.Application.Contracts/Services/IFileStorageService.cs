using System.Threading.Tasks;

namespace TimeTable.Application.Contracts.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(byte[] content, string extension, string container, string contentType);
        Task<string> UpdateFileAsync(byte[] content, string extension, string container, string path, string contentType);
        Task DeleteFileAsync(string container, string path);
    }
}
