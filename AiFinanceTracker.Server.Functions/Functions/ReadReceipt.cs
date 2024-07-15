using AiFinanceTracker.Server.Functions.Dtos;
using AiFinanceTracker.Server.Functions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;


namespace AiFinanceTracker.Server.Functions.Functions
{
    public class ReadReceipt(
            ITransactionRepository transactionRepository
        )
    {
     

        [Function("ReadReceipt")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            var formData = await req.ReadFormAsync();
            IFormFile? file = formData.Files["receiptFile"];

            if (file is null)
                return new BadRequestObjectResult("receipt cannot be null");
            var createTransactionDto = new ReadReceiptRequestDto(file);
            return new OkObjectResult(await transactionRepository.ReadReciept(createTransactionDto));
        }
    }
}
