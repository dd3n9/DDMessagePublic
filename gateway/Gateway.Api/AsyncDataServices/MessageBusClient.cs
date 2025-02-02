using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using Gateway.Api.Models.DTOs;

namespace Gateway.Api.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdowm;
                Console.WriteLine("--> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void PublishNewUser(UserPublishedDto userPublishedDto)
        {
            var message = JsonSerializer.Serialize(userPublishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RebbitMQ Connection Open , sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connections closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                               routingKey: "",
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
