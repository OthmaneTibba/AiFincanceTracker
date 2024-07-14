namespace AiFinanceTracker.Server.Functions.Interfaces
{
    public interface IStorageService
    {
        public Task<string> SaveFileAsync(Stream stream, string fileName);

        public Task DeleteFileAsync(string fileName);
    }
}
