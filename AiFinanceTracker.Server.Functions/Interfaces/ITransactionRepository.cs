
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
        public Task<IEnumerable<Transaction>> GetTransactionsByDateAsync(string startDate, string endDate); 
        public Task<IEnumerable<TotalTransactionAnalytics>> GetTotalTransactionAnalytics(string startDate,string endDate, string transactionType);
    }
}
