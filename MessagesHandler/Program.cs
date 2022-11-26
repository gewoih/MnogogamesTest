using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using SharedLibrary;

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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            };

            while (true)
            {
                channel.BasicConsume(queue: MainSettings.Default.RabbitMQMessagesQueueName,
                                     autoAck: false,
                                     consumer: consumer);

                Thread.Sleep(1000);
            }
        }
    }
}