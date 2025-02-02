using MessageService.Application.Common.AsyncDataServices;
using MessageService.Contracts.DTO.Message;
using MessageService.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MessageService.Infrastructure.Common.AsyncDataServices
{
    internal sealed class MessageBusClient : IMessageBusClient
    {
        private readonly RabbitMQSettings _rabbitMqSettings;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IOptions<RabbitMQSettings> rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqSettings.HostName,
                Port = _rabbitMqSettings.Port
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "messageExchange", type: ExchangeType.Topic);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdowm;

                Console.WriteLine("--> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void PublishNewMessageNotification(MessageNotificationPublishedDto publishedDto)
        {
            var message = JsonSerializer.Serialize(publishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open , sending message...");
                SendMessage(message, publishedDto.Event);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connections closed, not sending");
            }
        }

        public void PublishNewMessageScheduled(MessageSchedulerPublishedDto publishedDto)
        {
            var message = JsonSerializer.Serialize(publishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open , sending message...");
                SendMessage(message, publishedDto.Event);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connections closed, not sending");
            }
        }

        private void SendMessage(string message, string routingKey = "")
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "messageExchange",
                                routingKey: routingKey,
                                basicProperties: null,
                                body: body);

            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsClosed)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdowm(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitWQ Connection Shutdown");
        }
    }
}
