
using AiFinanceTracker.Server.Functions.Interfaces;

namespace AiFinanceTracker.Server.Functions.Services
{
    public class BlobStorageService(string connectionString) : IStorageService
    {
        public Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveFileAsync(Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
