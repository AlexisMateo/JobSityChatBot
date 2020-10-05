using JobSity.ChatApp.Bot.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using JobSity.ChatApp.Bot.Models;
using System.Text;
using System.IO;

namespace JobSity.ChatApp.Bot.Services
{
    public class BrokerConsumerService : IBrokerConsumerService
    {
        private readonly ConnectionFactory _connectionFactory;

        private IConnection _connection;

        private IModel _channel;
        private readonly RabbitMQInfo _rabbitInfo;
        private readonly IConfiguration configuration1;
        private readonly _brokerService;

        public BrokerConsumerService(
            ILogger<BrokerConsumerService> logger, 
            IOptions<RabbitMQInfo> rabbitInfo,
            IConfiguration configuration,
            IBrokerService brokerService)
        {
            _rabbitInfo = rabbitInfo.Value;

            _connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitInfo.HostName,
                UserName = _rabbitInfo.UserName,
                Password = _rabbitInfo.Password
            };

            _connection = _connectionFactory.CreateConnection();

            _configuration = configuration;
            _brokerService = brokerService;

        }
        public void RegisterQueueForChatBot()
        {

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _rabbitInfo.Queue,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                bool isAllSuccess = true;
                
                if(!string.IsNullOrWhiteSpace(message))
                {
                    _brockerService.GetStockQuote();
                }

                if(_channel.IsOpen)
                {
                    if(isAllSuccess){
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    else{
                        _channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                    }
                }
            };

            _channel.BasicConsume(queue: _rabbitInfo.Queue,
                                autoAck: false,
                                consumer: consumer);
        }
    }
}