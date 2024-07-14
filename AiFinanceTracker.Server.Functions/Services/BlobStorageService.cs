
using AiFinanceTracker.Server.Functions.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AiFinanceTracker.Server.Functions.Services
{
    public class BlobStorageService(string connectionString) : IStorageService
    {
        private readonly string[] validExtension = { ".png", ".jpg", ".jpeg" };
        public async Task DeleteFileAsync(string fileName)
        {
            var container = await GetBlobContainerAsync();
            string name = Path.GetFileName(fileName);
            var blob = container.GetBlobClient(name);
            await blob.DeleteAsync();

        }

        public async Task<string> SaveFileAsync(Stream stream, string fileName)
        {
            var container = await GetBlobContainerAsync();
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            if(!validExtension.Contains(extension))
                throw new NotSupportedException("file not supported"); // NotSupportedException should be catchable via the middleware

            // here we can verify content type && size of the file // TODO LATER
            var uniqueName = $"{nameWithoutExtension}-{Guid.NewGuid()}{extension}";
            var blob = container.GetBlobClient(uniqueName);
            await blob.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = GetContentTypeFromExtension(extension)    
                }
            });
            return blob.Uri.AbsoluteUri;
        }

        private string GetContentTypeFromExtension(string extension)
        {
            return extension switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpg",
                ".jpeg" => "image/jpeg",
                _ => throw new NotSupportedException("content type not supported")
            };
        }

        private async Task<BlobContainerClient> GetBlobContainerAsync()
        {
            var blobClient = new BlobServiceClient(connectionString);
            var container = blobClient.GetBlobContainerClient("test");
            await container.CreateIfNotExistsAsync();
            return container;
        }
    }
}
