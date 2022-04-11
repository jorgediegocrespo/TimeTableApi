using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using TimeTable.Application.Contracts.Services;

namespace TimeTable.Application.Services
{
    public class LocalStorageService : IFileStorageService
    {
        private readonly IWebPathsService webPathsService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocalStorageService(IWebPathsService webPathsService, IHttpContextAccessor httpContextAccessor)
        {
            this.webPathsService = webPathsService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string container, string contentType)
        {
            string fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(webPathsService.GetWebRootPath(), container);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(path, content);

            string url = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            return Path.Combine(url, container, fileName).Replace("\\", "/");
        }

        public async Task<string> UpdateFileAsync(byte[] content, string extension, string container, string path, string contentType)
        {
            await DeleteFileAsync(container, path);
            return await SaveFileAsync(content, extension, container, contentType);
        }

        public Task DeleteFileAsync(string container, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Task.CompletedTask;

            string fileName = Path.GetFileName(path);
            string filePath = Path.Combine(webPathsService.GetWebRootPath(), container, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}
