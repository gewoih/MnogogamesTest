using RabbitMQ.Client;
using SharedLibrary;

namespace MessagesHandler.Services
{
    /// <summary>
    /// Service responsible for handling messages from RabbitMQ at the specified interval.
    /// </summary>
    public sealed class MessagesHandlerService
    {
        private readonly TimeSpan _handlingInterval;
        private readonly IModel _rabbitmqModel;
        private Timer? _timer;

        /// <summary>
        /// Event that will be invoked when the message is successfully received.
        /// </summary>
        public event Action<BasicGetResult>? OnMessageReceived;

        /// <param name="handlingInterval">The interval for handling messages from the RabbitMQ</param>
        public MessagesHandlerService(IModel rabbitMqModel, TimeSpan handlingInterval)
        {
            _rabbitmqModel = rabbitMqModel;
            _handlingInterval = handlingInterval;
            OnMessageReceived += MessagesHandlerService_OnNewMessageReceived;
        }

        /// <summary>
        /// Starts messages handling from RabbitMQ at the interval specified in constructor.
        /// </summary>
        public void StartMessagesHandling()
        {
            _timer = new(HandleNewMessage, null, TimeSpan.FromSeconds(0), _handlingInterval);
        }

        /// <summary>
        /// Stops handling messages from RabbitMQ.
        /// </summary>
        public void StopMessagesHandling()
        {
            _timer?.Dispose();
        }

        private void HandleNewMessage(object? obj)
        {
            var result = _rabbitmqModel.BasicGet(MainSettings.Default.RabbitMQMessagesQueueName, false);

            if (result is not null)
            {
                OnMessageReceived?.Invoke(result);
            }
        }

        private void MessagesHandlerService_OnNewMessageReceived(BasicGetResult basicGetResult)
        {
            _rabbitmqModel.BasicAck(basicGetResult.DeliveryTag, false);
        }
    }
}
