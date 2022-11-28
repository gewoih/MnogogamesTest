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

        /// <summary>
        /// Event that will be invoked when the message is  successfully sent.
        /// </summary>
        public event Action<KeyValuePair<Message, Priority>>? OnMessageSended;

        /// <param name="sendingInterval">The interval for sending messages to the RabbitMQ</param>
        public MessagesSenderService(IModel rabbitMqModel, TimeSpan sendingInterval)
        {
            _rabbitmqModel = rabbitMqModel;
            _sendingInterval = sendingInterval;
        }

        /// <summary>
        /// Starts sending messages to RabbitMQ at the interval specified in constructor.
        /// </summary>
        public void StartMessagesSending()
        {
            _timer = new(MessagesSenderHandler, null, TimeSpan.FromSeconds(0), _sendingInterval);
        }

        /// <summary>
        /// Stops sending messages to RabbitMQ.
        /// </summary>
        public void StopMessagesSending()
        {
            _timer?.Dispose();
        }

        private void MessagesSenderHandler(object? obj)
        {
            var generatedString = Randomizer.GetRandomString();
            var generatedPriority = Randomizer.GetRandomEnumValue<Priority>();
            var generatedMessage = new Message(generatedString);

            SendMessageToRabbitMQ(generatedMessage, generatedPriority);
        }

        private void SendMessageToRabbitMQ(Message message, Priority priority)
        {
            var properties = _rabbitmqModel.CreateBasicProperties();
            properties.Priority = (byte)priority;

            var serializedMessage = JsonConvert.SerializeObject(message);
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
