
namespace AiFinanceTracker.Server.Functions
{
    public class ApiResponse<T> where T : class
    {
        public required bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Error { get; set; }

    }
}
