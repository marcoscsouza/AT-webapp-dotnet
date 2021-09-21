using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functionv5
{
    public static class Function1
    {
        [Function("Function1")]
        public static void Run([QueueTrigger("myqueue-items", Connection = "AzureWebJobsStorage")] string myQueueItem,
            FunctionContext context)
        {
            var logger = context.GetLogger("Function1");
            logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
