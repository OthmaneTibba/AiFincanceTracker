using AiFinanceTracker.Server.Functions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AiFinanceTracker.Server.Functions.Functions
{
    public class GetCategoryItemExpenseAnalytics
    {
        private readonly ILogger<GetCategoryItemExpenseAnalytics> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public GetCategoryItemExpenseAnalytics(ILogger<GetCategoryItemExpenseAnalytics> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        [Function("GetCategoryItemExpenseAnalytics")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {

            string? transactionType = req.Query["transactionType"];
            string? startDate = req.Query["startDate"];
            string? endDate = req.Query["endDate"];
            if (string.IsNullOrEmpty(transactionType) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                throw new ArgumentNullException("transaction type and startDate and endDate are required");
            return new OkObjectResult(await _transactionRepository.GetCategoryItemExpenseAnalytics(startDate, endDate, transactionType));
        }
    }
}
