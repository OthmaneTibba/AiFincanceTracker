

using AiFinanceTracker.Server.Functions.Dtos;
using AiFinanceTracker.Server.Functions.Errors;
using AiFinanceTracker.Server.Functions.Interfaces;
using AiFinanceTracker.Server.Functions.Models;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Models;
using System.Text;

namespace AiFinanceTracker.Server.Functions.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly IStorageService _storageService;

        private readonly ImageAnalysisClient _imageAnalysisClient;
        private readonly OpenAIAPI _openAIAPI;
        private readonly Container _container;


        private const string DATABASE_NAME = "AiFinanceTrackerDb";
        private const string CONTAINER_NAME = "Transactions";

        public TransactionRepository(CosmosClient cosmosClient, ImageAnalysisClient imageAnalysisClient, OpenAIAPI openAIAPI, IStorageService storageService)
        {
            _cosmosClient = cosmosClient;
            _imageAnalysisClient = imageAnalysisClient;
            _openAIAPI = openAIAPI;
            _container = _cosmosClient.GetContainer(DATABASE_NAME, CONTAINER_NAME);
            _storageService = storageService;
        }

        public async Task<Transaction> CreateTransactionFromReceipt(
            CreateTransactionDto createTransactionDto)
        {
            var tarnsaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                Category = createTransactionDto.Category,
                Date = createTransactionDto.Date,
                Items = createTransactionDto.Items,
                Merchant = createTransactionDto.Merchant,
                TotalPrice = createTransactionDto.TotalPrice,
                TransactionType = createTransactionDto.TransactionType,
                ReceiptUrl = createTransactionDto.ReceiptUrl,
            };


            var results = await _container.CreateItemAsync(tarnsaction);

            return results;



        }

        public async Task<bool> DeleteTransactionAsync(string transactionId)
        {
            var res = await _container.DeleteItemAsync<Transaction>(transactionId, new PartitionKey(transactionId));
            if(res.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ItemNotFoundException("transaction not found");
            }
            return res.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            var queryText = "SELECT * FROM c";


            var queryDef = new QueryDefinition(queryText);
              

            var itterator = _container.GetItemQueryIterator<Transaction>(queryDef);

            var res = await itterator.ReadNextAsync();
            return res.Resource;
        }

        public async Task<TransactionResponseDto> ReadReciept(ReadReceiptRequestDto createTransactionFromReceiptDto)
        {
            ImageAnalysisResult analysisResult = await _imageAnalysisClient.AnalyzeAsync(BinaryData
               .FromStream(createTransactionFromReceiptDto.ReceiptFile.OpenReadStream()),
             VisualFeatures.Caption | VisualFeatures.Read,
             new ImageAnalysisOptions { GenderNeutralCaption = true });

            string extractedText = GetTextResultFromOcrImageAnalysisResult(analysisResult);
            string prompt = $@"
                    you role is a text extractor the text between the BLOCK CONTEXT it's going to be extracted from an invoice. if it's not in English translate it to English after that you need to extract all information following this schema.
                    be sure to add all items availlable in the BLOCK CONTEXT
                    {{
                    success : ,// true you we able to extract data from the BLOCK CONTEXT
                    date : // us format,
                    merchant : {{
                        name : 
                    }},
                    category : // USE ONE FROM THIS LIST [Groceries Food & Dining Shopping Utilities Entertainment Transportation Office Supplies & Equipment Professional Services Repairs & Maintenance Rent & Lease Insurance Miscellaneous Other]
                    items : [
                    {{
                        name : , // must be in English 
                        category : ,   // USE ONE FROM THIS LIST [Fruits Vegetables Canned Goods Dairy Meat Fish & Seafood Deli Condiments & Spices Snacks Bread & Bakery Beverages Pasta, Rice & Cereal Baking Frozen Foods Personal Care Health Care Household & Cleaning Supplies Baby Items Pet Care Other]
                        quantity : ,// must be contain only numbers default is 1 if not been provided it in the BLOCK CONTEXT and int type.
                        price : // must be contain only numbers default is 0 if not been provided it in the BLOCK CONTEXT and double type.
                    }}
                    ],
                    total_price : must be a number only float.
                    }}
                    START BLOCK CONTEXT
                        {extractedText}
                    END BLOCK CONTEXT
                    result should be always a json format respecting the schema provided.
                    if there is any information in the SCHEMA does not available in the content inside the BLOCK CONTEXT try to figure it out yourself otherwise set it as UNKONWN
                    response with the schema only.
                 ";


            var aiChat = _openAIAPI.Chat.CreateConversation();
            aiChat.Model = Model.ChatGPTTurbo;
            aiChat.AppendUserInput(prompt);
            var aiResponse = await aiChat.GetResponseFromChatbotAsync();
            var aiResponseDesirialized = JsonConvert.DeserializeObject<AiTransactionResponse>(aiResponse);

            if (aiResponseDesirialized is null)
            {
                throw new AiResponseException("cannot format your data properly");
            }

            if (!aiResponseDesirialized.IsSuccess)
            {
                throw new AiResponseException("no data please try with another receipt or try again!");
            }


            var receiptUrl = await _storageService.SaveFileAsync(
                createTransactionFromReceiptDto.ReceiptFile.OpenReadStream(),
                createTransactionFromReceiptDto.ReceiptFile.FileName);

            

            return new TransactionResponseDto
            {
                Category = aiResponseDesirialized.Category,
                TransactionType = "Expense",
                Date = aiResponseDesirialized.Date,
                Items = aiResponseDesirialized.Items,
                Merchant = aiResponseDesirialized.Merchant,
                TotalPrice = aiResponseDesirialized.totalPrice,
                ReceiptUrl = receiptUrl,
            };
        }

        private string GetTextResultFromOcrImageAnalysisResult(ImageAnalysisResult imageAnalysisResult)
        {
            StringBuilder imageText = new();

            foreach (DetectedTextBlock block in imageAnalysisResult.Read.Blocks)
            {
                foreach (DetectedTextLine line in block.Lines)
                {

                    foreach (DetectedTextWord word in line.Words)
                    {
                        imageText.Append(word.Text + " ");
                    }
                }
            }

            return imageText.ToString().TrimEnd();
        }

        public async Task<Transaction> UpdateTransaction(string transactionId, Transaction transaction)
        {
            Transaction updatedTransaction = await _container.ReplaceItemAsync<Transaction>(transaction, transactionId,new PartitionKey(transactionId));
            return updatedTransaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateAsync(string startDate, string endDate)
        {
            var queryText = "SELECT * FROM c WHERE c.date BETWEEN @startDate AND @endDate";
            var queryDef = new QueryDefinition(queryText)
                .WithParameter("@startDate", startDate)
                .WithParameter("@endDate", endDate);
            var itterator = _container.GetItemQueryIterator<Transaction>(queryDef);

            var res = await itterator.ReadNextAsync();
            return res.Resource;
        }
    }


}
