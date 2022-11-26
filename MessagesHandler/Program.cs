using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using SharedLibrary;
using MessagesHandler.Services;
using Newtonsoft.Json;
using SharedLibrary.Models;

namespace MessagesHandler
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

            MessagesHandlerService handler = new(channel, MainSettings.Default.HandlingMessagesIntervalInMilliseconds);
            handler.OnNewMessageReceived += OnNewMessageReceived;
            handler.StartMessagesHandling();

            Console.ReadKey();
        }

        private static void OnNewMessageReceived(BasicGetResult basicGetResult)
        {
            var body = basicGetResult.Body.ToArray();
            var encodedData = Encoding.UTF8.GetString(body);

            var message = JsonConvert.DeserializeObject<Message>(encodedData);


            Console.WriteLine($"Received new message: {message} {Environment.NewLine}");
        }
    }
}