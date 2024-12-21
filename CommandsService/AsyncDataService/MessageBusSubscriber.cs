using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CommandsService.EventProcessing;

namespace CommandsService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            InitalizeRabbitMQ();
        }

        private void InitalizeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare a fanout exchange
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            // Create a non-durable queue
            _queueName = _channel.QueueDeclare().QueueName;

            // Bind the queue to the exchange
            _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

            Console.WriteLine("--> Listening on the Message Bus...");

            // Handle connection shutdown
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start the consumer in the background
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);

            // Event handler for receiving messages
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("--> Event Received!");

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"--> Message Received: {message}");

                _eventProcessor.ProcessEvent(message);
            };


            // Consume messages from the queue
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
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
