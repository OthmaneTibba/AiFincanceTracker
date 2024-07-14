using AiFinanceTracker.Server.Functions.Interfaces;
using AiFinanceTracker.Server.Functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace AiFinanceTracker.Server.Functions.Functions
{
    public class UpdateTransaction
    {
        private readonly ILogger<UpdateTransaction> _logger;
        private readonly ITransactionRepository _transactionRepository;


        public UpdateTransaction(ILogger<UpdateTransaction> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        [Function("UpdateTransaction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function,  "put")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string? transactionId = req.Query[nameof(transactionId)];   
            if(transactionId == null) 
                throw new ArgumentNullException(nameof(transactionId));

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var transaction = JsonConvert.DeserializeObject<Transaction>(body);
            return new OkObjectResult(await _transactionRepository.UpdateTransaction(transactionId, transaction));
        }
    }
}
