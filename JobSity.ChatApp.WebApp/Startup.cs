using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JobSity.ChatApp.Core.Interfaces.Identity;
using JobSity.ChatApp.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace JobSity.ChatApp.WebApp
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
            var chatWebIdentityInfo = Configuration.GetSection("ChatWebIdentityInfo");

             services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddSeq(Configuration.GetSection("Seq"));
                    });


            services.AddAuthentication(config =>{
                config.DefaultScheme = "Cookies";
                config.DefaultChallengeScheme = "oidc";

            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", config =>{
                config.SignInScheme = "Cookies";

                config.Authority = chatWebIdentityInfo.GetSection("Authority").Value;
                config.RequireHttpsMetadata = false;

                config.ClientId = chatWebIdentityInfo.GetSection("ClientId").Value;
                config.ClientSecret = chatWebIdentityInfo.GetSection("ClientSecret").Value;
                
                config.SaveTokens = true;

                config.GetClaimsFromUserInfoEndpoint = true;

                config.ResponseType = "code";
                
            }) ;

            services.AddHttpClient();
            services.AddHttpClient<IIdentityManagerService, IdentityManagerService>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            // }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
