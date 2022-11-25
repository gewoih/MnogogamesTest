using Newtonsoft.Json;
using RabbitMQ.Client;
using SharedLibrary.Enums;
using SharedLibrary.Models;
using System.Text;

namespace MessagesGenerator.Services
{
    internal sealed class MessagesSender
    {
        private readonly TimeSpan _sendingInterval;
        private readonly IModel _rabbitmqModel;

        public MessagesSender(IModel rabbitMqModel, TimeSpan sendingInterval)
        {
            _rabbitmqModel = rabbitMqModel;
            _sendingInterval = sendingInterval;

            Timer timer = new(MessagesSenderHandler, null, TimeSpan.FromSeconds(0), _sendingInterval);
        }

        private void MessagesSenderHandler(object obj)
        {
            string generatedString = MessagesGenerator.GetRandomString();
            var generatedPriority = MessagesGenerator.GetRandomEnumValue<Priority>();
            Message generatedMessage = new(generatedString, generatedPriority);

            SendMessageToRabbiqMQ(generatedMessage, generatedPriority);

            Thread.Sleep(_sendingInterval);
        }

        private void SendMessageToRabbiqMQ(Message message, Priority priority)
        {
            IBasicProperties properties = _rabbitmqModel.CreateBasicProperties();
            properties.Priority = (byte)priority;

            string serializedMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            _rabbitmqModel.BasicPublish(
                exchange: string.Empty,
                routingKey: "messagesWithPriorities",
                basicProperties: properties,
                body: body);
        }
    }
}
