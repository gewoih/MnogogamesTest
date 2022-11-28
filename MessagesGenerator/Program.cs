using MessagesGenerator.Services;
using RabbitMQ.Client;
using SharedLibrary;
using SharedLibrary.Enums;
using SharedLibrary.Models;

namespace MessagesGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            { 
                HostName = MainSettings.Default.RabbitMQHostName, 
                Port = Protocols.DefaultProtocol.DefaultPort, 
                UserName = MainSettings.Default.RabbitMQUsername, 
                Password = MainSettings.Default.RabbitMQPassword,
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true,
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            MessagesSenderService messagesSender = new(channel, MainSettings.Default.SendingMessagesIntervalInMilliseconds);
            messagesSender.OnMessageSended += MessagesSender_OnMessageSended;
            messagesSender.StartMessagesSending();

            Console.ReadKey();
        }

        private static void MessagesSender_OnMessageSended(KeyValuePair<Message, Priority> messageWithPriority)
        {
            Console.WriteLine($"Sended message {messageWithPriority.Key} with priority={(int)messageWithPriority.Value} {Environment.NewLine}");
        }
    }
}