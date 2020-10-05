using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JobSity.ChatApp.Bot.Models;

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
                    
                    services.AddHostedService<Worker>();

                    services.Configure<RabbitMQInfo>(iConfiguration.GetSection("RabbitMQ"));

                });
    }
}
