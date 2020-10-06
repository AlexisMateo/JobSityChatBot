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
            System.AppDomain.CurrentDomain.UnhandledException += GlobalExceptionHandler;
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var iConfiguration = hostContext.Configuration;

                    services.AddHostedService<Worker>();
                    services.AddHttpClient<IBrokerService,BrokerService>();

                    services.AddTransient<IBrokerConsumerService, BrokerConsumerService>();

                    services.Configure<RabbitMQInfo>(iConfiguration.GetSection("RabbitMQ"));
                    
                    services.Configure<StockQueues>(iConfiguration.GetSection("StockQueues"));

                });
        private static void GlobalExceptionHandler(object sender, UnhandledExceptionEventArgs e) {
             
             using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
             
             var logger = loggerFactory.CreateLogger<Program>();

            logger.LogError($"APPID : Bot, Error : { e.ToString()}");
        }
    }

   
}
