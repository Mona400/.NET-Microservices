﻿using Microsoft.EntityFrameworkCore.Metadata;
using PlatformService.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using IModel = RabbitMQ.Client.IModel;

namespace PlatformService.AsyncDataService
{
    
        public class MessageBusClient : IMessageBusClient
        {
            private readonly IConfiguration _configuration;
            private readonly IConnection _connection;
            private readonly IModel _channel;

            public MessageBusClient(IConfiguration configuration, IConnection connection, IModel channel)
            {
                _configuration = configuration;
                _connection = connection;
                _channel = channel;

                try
                {
                    _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                    _channel.ModelShutdown += RabbitMQ_ConnectionShutdown;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not connect to RabbitMQ: {ex.Message}");
                }
            }

            public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
            {
                var message = JsonSerializer.Serialize(platformPublishDto);
                if (_connection.IsOpen)
                {
                    Console.WriteLine("---> RabbitMQ Is Open");
                    SendMessage(message);
                }
                else
                {
                    Console.WriteLine("---> RabbitMQ Is Closed");
                }
            }

            private void SendMessage(string messageType)
            {
                var body = Encoding.UTF8.GetBytes(messageType);
                _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
                Console.WriteLine("Message sent to RabbitMQ");
            }

            public void Dispose()
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
                Console.WriteLine("--> RabbitMQ Connection Shutdown");
            }
        }
    }

