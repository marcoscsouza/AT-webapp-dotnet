using Azure.Storage.Queues;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class QueueService : IQueueService
    {
        private readonly QueueServiceClient _queueServiceClient;
        private const string _queueName = "queue-update-last-view";

        public QueueService(string storageAccount)
        {
            _queueServiceClient = new QueueServiceClient(storageAccount);
        }

        public async Task SendAsync(string messageText)
        {
            var queueClient = _queueServiceClient.GetQueueClient(_queueName);

            await queueClient.CreateIfNotExistsAsync();

            await queueClient.SendMessageAsync(messageText);
        }
    }
}
