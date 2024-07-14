using AiFinanceTracker.Server.Functions.Interfaces;
using AiFinanceTracker.Server.Functions.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace AiFinanceTracker.Server.Functions.Functions
{
    public class DeleteTransaction(
        ITransactionRepository transactionRepository
        )
    {
       

        [Function("DeleteTransaction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequest req)
        {
            string? transactionId = req.Query[nameof(transactionId)];
            if(string.IsNullOrEmpty(transactionId)) return new BadRequestObjectResult("transactionId cannot be null");

            if(await transactionRepository.DeleteTransactionAsync(transactionId))
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestObjectResult("cannot delete");
            }
            
        }
    }
}
