using MessagesGenerator.Services;
using RabbitMQ.Client;

namespace MessagesGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            { 
                HostName = "95.163.241.218", 
                Port = Protocols.DefaultProtocol.DefaultPort, 
                UserName = "root", 
                Password = "root",
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true,
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            MessagesSender messagesSender = new(channel, TimeSpan.FromSeconds(1));

            Console.ReadKey();
        }
    }
}