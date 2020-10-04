using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobSity.ChatApp.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                endpoints.MapHub<ChatHubService>("/chatHub");
            });
        }
    }
}
