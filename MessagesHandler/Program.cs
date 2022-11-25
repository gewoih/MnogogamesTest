using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessagesHandler
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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            };

            while (true)
            {
                channel.BasicConsume(queue: "messagesWithPriorities",
                                     autoAck: false,
                                     consumer: consumer);

                Thread.Sleep(1000);
            }
        }
    }
}