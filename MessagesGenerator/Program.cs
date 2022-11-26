using MessagesGenerator.Services;
using Microsoft.VisualBasic;
using RabbitMQ.Client;
using SharedLibrary;

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

            Console.ReadKey();
        }
    }
}