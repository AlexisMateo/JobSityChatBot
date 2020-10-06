using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JobSity.ChatApp.Core.Entities.Bot;
using JobSity.ChatApp.Core.Interfaces.Bot;
using JobSity.ChatApp.Infrastructure.Services.Bot;
using Microsoft.Extensions.Logging;

namespace JobSity.ChatApp.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();  
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var iConfiguration = hostContext.Configuration;

                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddSeq(iConfiguration.GetSection("Seq"));
                    });


                    services.AddHostedService<Worker>();
                    services.AddHttpClient<IBrokerService,BrokerService>();

                    services.AddTransient<IBrokerConsumerService, BrokerConsumerService>();

                    services.Configure<RabbitMQInfo>(iConfiguration.GetSection("RabbitMQ"));
                    
                    services.Configure<StockQueues>(iConfiguration.GetSection("StockQueues"));

                });
    }

   
}
