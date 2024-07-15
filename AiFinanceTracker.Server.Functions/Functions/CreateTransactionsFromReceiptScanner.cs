using AiFinanceTracker.Server.Functions.Dtos;
using AiFinanceTracker.Server.Functions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AiFinanceTracker.Server.Functions.Functions
{
    public class CreateTransactionsFromReceiptScanner
    {
        private readonly ILogger<CreateTransactionsFromReceiptScanner> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public CreateTransactionsFromReceiptScanner(
            ILogger<CreateTransactionsFromReceiptScanner> logger,
            ITransactionRepository transactionRepository
            )
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        [Function("CreateTransactions")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var createTransactionDto = JsonConvert.DeserializeObject<CreateTransactionDto>(body);
            return new OkObjectResult(await _transactionRepository
                .CreateTransactionFromReceipt(createTransactionDto));
        }
    }
}
