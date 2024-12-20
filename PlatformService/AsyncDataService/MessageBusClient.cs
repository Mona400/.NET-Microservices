using Microsoft.EntityFrameworkCore.Metadata;
using PlatformService.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;
        public MessageBusClient(IConfiguration configuration, IConnection connection, RabbitMQ.Client.IModel channel)
        {
            _configuration = configuration;
            _connection = connection;
            _channel = channel;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _channel.ModelShutdown += RabbitMQ_ConnectionShutdown;
            }
            catch (Exception ex)
            {

            }

        }
        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            var message = JsonSerializer.Serialize(platformPublishDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("---> RabbatMQ Is Open");
            }
            else
            {
                Console.WriteLine("---> RabbatMQ Is Close");
            }
        }
        private void SendMessage(string messageType)
        {
            var body = Encoding.UTF8.GetBytes(messageType);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"we have send message");
        }
        private void Disposed()
        {
            Console.WriteLine("Message Bus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("-->RabbiteMQ Connection shutdown");
        }
    }
}
