using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JobSity.ChatApp.Core.Interfaces.Bot;

namespace JobSity.ChatApp.Bot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        IBrokerConsumerService _brokerConsumerService;

        public Worker(ILogger<Worker> logger, IBrokerConsumerService brokerConsumerService)
        {
            _logger = logger;
            _brokerConsumerService = brokerConsumerService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _brokerConsumerService.RegisterQueueForChatBot();
            }
            catch (Exception ex )
            {
                _logger.LogError(ex.ToString());
            }

            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1*60*60*1000, stoppingToken);
            }
        }
    }
}
