using RabbitMQ.Client;
using SharedLibrary;

namespace MessagesHandler.Services
{
    public sealed class MessagesHandlerService
    {
        private readonly TimeSpan _handlingInterval;
        private readonly IModel _rabbitmqModel;
        private Timer _timer;
        public event Action<BasicGetResult> OnNewMessageReceived;


        public MessagesHandlerService(IModel rabbitMqModel, TimeSpan handlingInterval)
        {
            _rabbitmqModel = rabbitMqModel;
            _handlingInterval = handlingInterval;
            OnNewMessageReceived += MessagesHandlerService_OnNewMessageReceived;
        }

        public void StartMessagesHandling()
        {
            _timer = new(HandleNewMessage, null, TimeSpan.FromSeconds(0), _handlingInterval);
        }

        public void StopMessagesHandling()
        {
            _timer?.Dispose();
        }

        private void HandleNewMessage(object? obj)
        {
            var result = _rabbitmqModel.BasicGet(MainSettings.Default.RabbitMQMessagesQueueName, false);
            OnNewMessageReceived(result);

            Thread.Sleep(_handlingInterval);
        }

        private void MessagesHandlerService_OnNewMessageReceived(BasicGetResult basicGetResult)
        {
            _rabbitmqModel.BasicAck(basicGetResult.DeliveryTag, false);
        }
    }
}
