

using AiFinanceTracker.Server.Functions.Interfaces;
using AiFinanceTracker.Server.Functions.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;


namespace AiFinanceTracker.Server.Functions
{
    public static class DependencyInjectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }

        public static void AddCosmosDbClient(this IServiceCollection services, string connectionString) {

            services.AddSingleton(sp => new CosmosClient(connectionString));
        }
    }
}
