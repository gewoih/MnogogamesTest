using Newtonsoft.Json;
using RabbitMQ.Client;
using SharedLibrary;
using SharedLibrary.Enums;
using SharedLibrary.Models;
using SharedLibrary.Utils;
using System.Text;

namespace MessagesGenerator.Services
{
    internal sealed class MessagesSenderService
    {
        private readonly TimeSpan _sendingInterval;
        private readonly IModel _rabbitmqModel;
        private Timer? _timer;
        public event Action<KeyValuePair<Message, Priority>>? OnMessageSended;

        public MessagesSenderService(IModel rabbitMqModel, TimeSpan sendingInterval)
        {
            _rabbitmqModel = rabbitMqModel;
            _sendingInterval = sendingInterval;
        }

        public void StartMessagesSending()
        {
            _timer = new(MessagesSenderHandler, null, TimeSpan.FromSeconds(0), _sendingInterval);
        }

        public void StopMessagesSending()
        {
            _timer?.Dispose();
        }

        private void MessagesSenderHandler(object? obj)
        {
            var generatedString = Randomizer.GetRandomString();
            var generatedPriority = Randomizer.GetRandomEnumValue<Priority>();
            Message generatedMessage = new(generatedString);

            SendMessageToRabbiqMQ(generatedMessage, generatedPriority);
        }

        private void SendMessageToRabbiqMQ(Message message, Priority priority)
        {
            var properties = _rabbitmqModel.CreateBasicProperties();
            properties.Priority = (byte)priority;

            string? serializedMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            _rabbitmqModel.BasicPublish(
                exchange: string.Empty,
                routingKey: MainSettings.Default.RabbitMQMessagesQueueName,
                basicProperties: properties,
                body: body);

            OnMessageSended?.Invoke(new(message, priority));
        }
    }
}
