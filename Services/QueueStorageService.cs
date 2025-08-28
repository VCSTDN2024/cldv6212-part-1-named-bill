using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace azuresolution1.Services
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(string queueName, string connectionString)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<string> ReceiveMessageAsync()
        {
            var response = await _queueClient.ReceiveMessageAsync();
            if (response.Value != null)
            {
                await _queueClient.DeleteMessageAsync(response.Value.MessageId, response.Value.PopReceipt);
                return response.Value.Body.ToString();
            }
            return null;
        }
    }
}