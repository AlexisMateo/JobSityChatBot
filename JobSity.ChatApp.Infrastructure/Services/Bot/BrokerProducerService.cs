using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JobSity.ChatApp.Core.Interfaces.Bot;
using JobSity.ChatApp.Core.Entities.Bot;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System;

namespace JobSity.ChatApp.Infrastructure.Services.Bot
{
    public class BrokerProducerService : IBrokerProducerService
    {
         private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<BrokerProducerService> _logger;
        private readonly RabbitMQInfo _rabbitInfo;


        public BrokerProducerService(
            ILogger<BrokerProducerService> logger,
            IOptions<RabbitMQInfo> rabbitInfo
        )
        {
            _logger = logger;
            _rabbitInfo = rabbitInfo.Value;

            _connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitInfo.HostName,
                UserName = _rabbitInfo.UserName,
                Password = _rabbitInfo.Password
            };
        }
        
        public void SendMessage(string queueName, string stockCode)
        {
            using(var connection = _connectionFactory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    try
                    {
                         channel.QueueDeclare(
                            queue: queueName,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                    var body = Encoding.UTF8.GetBytes(stockCode);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: properties,
                                         body: body);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                    }
                }
            }
        }
        
    }
}