using JobSity.ChatApp.Core.Interfaces.Bot;
using JobSity.ChatApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using JobSity.ChatApp.Core.Entities.Bot;
using System.Text;
using System.IO;

namespace JobSity.ChatApp.Infrastructure.Services.Bot
{
    public class BrokerConsumerService : IBrokerConsumerService
    {
        private readonly ConnectionFactory _connectionFactory;

        private IConnection _connection;

        private IModel _channel;
        private readonly RabbitMQInfo _rabbitInfo;
        private readonly StockQueues _stockQueue;
        private readonly IConfiguration _configuration;
        private readonly IBrokerService _brokerService;
        private string _stockApi;


        public BrokerConsumerService(
            ILogger<BrokerConsumerService> logger, 
            IOptions<RabbitMQInfo> rabbitInfo,
            IOptions<StockQueues> stockQueue,
            IConfiguration configuration,
            IBrokerService brokerService)
        {
            _rabbitInfo = rabbitInfo.Value;
            _stockQueue = stockQueue.Value;

            _connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitInfo.HostName,
                UserName = _rabbitInfo.UserName,
                Password = _rabbitInfo.Password
            };

            _connection = _connectionFactory.CreateConnection();

            _configuration = configuration;
            _brokerService = brokerService;
            

            _stockApi = _configuration.GetSection("StockApi").Value;


        }
        public void RegisterQueueForChatBot()
        {

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _stockQueue.StockRequest,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                var properties = ea.BasicProperties;

                var replyProps = _channel.CreateBasicProperties();

                replyProps.CorrelationId = properties.CorrelationId;
  
                var stockInfo = GetStockInfo(message);

                var responseBytes = Encoding.UTF8.GetBytes(stockInfo.ToString());

                _channel.BasicPublish(exchange: "", routingKey: properties.ReplyTo,
                basicProperties: replyProps, body: responseBytes);

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                
            };

            _channel.BasicConsume(queue: _stockQueue.StockRequest,
                                autoAck: false,
                                consumer: consumer);
        }

        private string GetStockInfo(string message)
        {
            string response = string.Empty;

            StringBuilder stockInfo = new StringBuilder();

            if(!string.IsNullOrWhiteSpace(message))
            {
                _stockApi = string.Format(_stockApi, message);

                var stocks = _brokerService.GetStockQuote(_stockApi).Result;

                foreach(var stock in stocks)
                {
                    stockInfo.AppendLine($"{stock.Symbol} quote is ${stock.Close} per share ");    
                }

                response = stockInfo.ToString();

            }

            response = string.IsNullOrEmpty(response) ? "-*-Please check your StockCode-*-" : response;

            return response;
        }
    }
}