using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using TimeTable.Application.Contracts.Services;

namespace TimeTable.Application.Services
{
    public class BlobAzureStorage : IFileStorage
    {
        private readonly string blobAzureConnectionString;

        public BlobAzureStorage(IConfiguration configuration)
        {
            blobAzureConnectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string container, string contentType)
        {
            if (content == null)
                return null;

            BlobContainerClient client = new BlobContainerClient(blobAzureConnectionString, container);
            await client.CreateIfNotExistsAsync();
            await client.SetAccessPolicyAsync(PublicAccessType.Blob);

            BlobClient blob = client.GetBlobClient($"{Guid.NewGuid()}{extension}");
            BlobHttpHeaders blobHttpHeader = new BlobHttpHeaders() { ContentType = contentType };
            BlobUploadOptions blobUploadOptions = new BlobUploadOptions() { HttpHeaders = blobHttpHeader };

            await blob.UploadAsync(new BinaryData(content), blobUploadOptions);            
            return blob.Uri.ToString();
        }

        public async Task<string> UpdateFileAsync(byte[] content, string extension, string container, string path, string contentType)
        {
            await DeleteFileAsync(container, path);
            return await SaveFileAsync(content, extension, container, contentType);
        }

        public async Task DeleteFileAsync(string container, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            BlobContainerClient client = new BlobContainerClient(blobAzureConnectionString, container);
            await client.CreateIfNotExistsAsync();
            BlobClient blob = client.GetBlobClient(Path.GetFileName(path));
            await blob.DeleteIfExistsAsync();
        }
    }
}
