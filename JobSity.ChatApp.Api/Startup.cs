using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobSity.ChatApp.Core.Interfaces.Chat;
using JobSity.ChatApp.Core.Interfaces.Repositories;
using JobSity.ChatApp.Core.Services;
using JobSity.ChatApp.Infrastructure.Repositories;
using JobSity.ChatApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JobSity.ChatApp.Core.Entities.Bot;
using JobSity.ChatApp.Core.Interfaces.Bot;
using JobSity.ChatApp.Infrastructure.Services.Bot;
using Microsoft.AspNetCore.Http;

namespace JobSity.ChatApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authority = Configuration.GetSection("IdentityInfo:Authority").Value;
            var audience = Configuration.GetSection("IdentityInfo:Audience").Value;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", configuration => {
                    configuration.Authority = authority;
                    configuration.Audience = audience;
                    configuration.RequireHttpsMetadata = false;

                    configuration.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context => {
                            
                            var accessToken = context.Request.Query["token"];

                            var path = context.HttpContext.Request.Path;

                            if(!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;

                        }
                    }; 

                });

            services.AddControllers();
            services.AddSignalR();
            services.AddSingleton<ChatHubService>();

            var allowedCorsDomains = Configuration.GetSection("AllowedCorsDomains").Value?.Split(",");

            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    builder.WithOrigins(allowedCorsDomains)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var dBConnectionString = Configuration.GetSection("ConnectionStrings:FinancialChatDb").Value;

            services.AddDbContext<FinancialChatDbContext>(options => {
                options.UseSqlServer(dBConnectionString);
            });

            services.AddTransient(typeof(IRepository<>), typeof(FinancialChatBaseRepository<>));
            services.AddTransient<IChatRoomService, ChatRoomService>();

            services.AddTransient<IBrokerProducerService, BrokerProducerService>();
            
            services.Configure<RabbitMQInfo>(Configuration.GetSection("RabbitMQ"));
            services.Configure<StockQueues>(Configuration.GetSection("StockQueues"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            var allowedCorsDomains = Configuration.GetSection("AllowedCorsDomains").Value?.Split(",");

            app.UseCors(
                options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Web API - ChatHub");
                });

                endpoints.MapHub<ChatHubService>("/chatHub");
            });
        }
    }
}
