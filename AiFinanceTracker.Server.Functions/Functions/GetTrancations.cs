using AiFinanceTracker.Server.Functions.Interfaces;
using AiFinanceTracker.Server.Functions.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AiFinanceTracker.Server.Functions.Functions
{
    public class GetTrancations
    {
        private readonly ILogger<GetTrancations> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public GetTrancations(ILogger<GetTrancations> logger , ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        [Function("GetTrancations")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
           
            return new OkObjectResult(await _transactionRepository.GetAllTransactionsAsync());
        }
    }
}
