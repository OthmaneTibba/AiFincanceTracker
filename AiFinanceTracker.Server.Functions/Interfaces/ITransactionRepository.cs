
using AiFinanceTracker.Server.Functions.Dtos;
using AiFinanceTracker.Server.Functions.Models;

namespace AiFinanceTracker.Server.Functions.Interfaces
{
    public interface ITransactionRepository
    {
        public Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        public Task<TransactionResponseDto> ReadReciept(ReadReceiptRequestDto createTransactionFromReceiptDto);
        public Task<Transaction> CreateTransactionFromReceipt(CreateTransactionDto createTransactionDto);
        public Task<bool> DeleteTransactionAsync(string transactionId);
        public Task<Transaction> UpdateTransaction(string transactionId, Transaction transaction);
    }
}
