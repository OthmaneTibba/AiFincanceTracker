using AiFinanceTracker.Server.Functions.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace AiFinanceTracker.Server.Functions
{
    public class ExceptionHandler : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (AiResponseException e)
            {
                context.GetHttpContext().Response.StatusCode = 400;
                await context.GetHttpContext().Response.WriteAsJsonAsync(new Dictionary<string, object>()
               {
                   { "statudCode" , 400 },
                   {"message" , e.Message }
               });
            }
            catch (CosmosException e)
            {
                context.GetHttpContext().Response.StatusCode = (int)e.StatusCode;
                if ((int)e.StatusCode == 404)
                {
                    await context.GetHttpContext().Response.WriteAsJsonAsync(new Dictionary<string, object>()
               {
                   { "statudCode" ,  (int)e.StatusCode },
                   {"message" , "Item not found" }
               });
                }
                else
                {
                    await context.GetHttpContext().Response.WriteAsJsonAsync(new Dictionary<string, object>()
               {
                   { "statudCode" ,  (int)e.StatusCode },
                   {"message" , "Database transaction error" }
               });
                }
            }
            catch(NotSupportedException e)
            {
                context.GetHttpContext().Response.StatusCode = 400;
                await context.GetHttpContext().Response.WriteAsJsonAsync(new Dictionary<string, object>()
               {
                   { "statudCode" , 400 },
                   {"message" , e.Message }
               });
            }
            catch (Exception e)
            {

                context.GetHttpContext().Response.StatusCode = 500;
                await context.GetHttpContext().Response.WriteAsJsonAsync(new Dictionary<string, object>()
               {
                   { "statudCode" , 500 },
                   {"message" , "Server error" }
               });
            }
        }
    }
}
