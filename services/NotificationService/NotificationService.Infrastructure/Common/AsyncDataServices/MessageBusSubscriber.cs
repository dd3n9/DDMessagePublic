using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationService.Application.EventProcessing;
using NotificationService.Infrastructure.Common.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService.Infrastructure.Common.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly RabbitMQSettings _rabbitmqSettings;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _messageQueueName;
        private string _schedulerQueueName;

        public MessageBusSubscriber(IOptions<RabbitMQSettings> rabbitmqSettings,
            IEventProcessor eventProcessor)
        {
            _rabbitmqSettings = rabbitmqSettings.Value;
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitmqSettings.HostName,
                Port = _rabbitmqSettings.Port
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "messageExchange", type: ExchangeType.Topic);
            _messageQueueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _messageQueueName, 
                                exchange: "messageExchange",
                                routingKey: "*.Notification");

            _channel.ExchangeDeclare(exchange: "schedulerExchange", type: ExchangeType.Fanout);
            _schedulerQueueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _schedulerQueueName,
                                exchange: "schedulerExchange",
                                routingKey: "");

            Console.WriteLine("--> Listening on the Message Bus...");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _messageQueueName, autoAck: true, consumer: consumer);

            _channel.BasicConsume(queue: _schedulerQueueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}
