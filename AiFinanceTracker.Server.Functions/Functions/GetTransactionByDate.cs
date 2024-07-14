using AiFinanceTracker.Server.Functions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AiFinanceTracker.Server.Functions.Functions
{
    public class GetTransactionByDate
    {
        private readonly ILogger<GetTransactionByDate> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionByDate(ILogger<GetTransactionByDate> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        [Function("GetTransactionByDate")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            string? startDate = req.Query["startDate"];
            string? endDate = req.Query["endDate"];

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                throw new ArgumentNullException("startDate and endDate must not be null");
            return new OkObjectResult(await _transactionRepository.GetTransactionsByDateAsync(startDate, endDate));
        }
    }
}
