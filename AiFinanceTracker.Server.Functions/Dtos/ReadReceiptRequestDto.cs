
using Microsoft.AspNetCore.Http;

namespace AiFinanceTracker.Server.Functions.Dtos
{
    public record ReadReceiptRequestDto(
            IFormFile ReceiptFile
        );
   
}
