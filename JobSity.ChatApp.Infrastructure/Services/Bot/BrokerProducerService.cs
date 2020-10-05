using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JobSity.ChatApp.Core.Interfaces.Bot;
using JobSity.ChatApp.Core.Events;
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
        private readonly IConnection _connection;
         private readonly IModel _channel;
        private readonly string _callbackQueue;
        private readonly EventingBasicConsumer _consumer;
        private readonly IBasicProperties _properties;

        public event EventHandler<StockEventArgs> QueueCallBack;

        public void OnQueueCallBack(StockEventArgs e)
        {
            EventHandler<StockEventArgs> handler = QueueCallBack;
            if (null != handler) handler(this, e);
        }
        

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

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _callbackQueue = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            _properties = _channel.CreateBasicProperties();

            RegisterRabbitConfig();
            
        }

        private void RegisterRabbitConfig()
        {
            
            var correlationId = Guid.NewGuid().ToString();
            _properties.CorrelationId = correlationId;
            _properties.ReplyTo = _callbackQueue;

            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                
                var stockEventArg = new StockEventArgs
                {
                    StockInfo = response
                };

                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    OnQueueCallBack(stockEventArg);
                }
            };
        }
        
        public void SendMessage(string queueName, string content)
        {
           
            try
            {

                var body = Encoding.UTF8.GetBytes(content);

                _channel.BasicPublish(exchange: "",
                                        routingKey: queueName,
                                        basicProperties: _properties,
                                        body: body);

                _channel.BasicConsume(
                    consumer: _consumer,
                    queue: _callbackQueue,
                    autoAck: true);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
   
        }
        
    }
}