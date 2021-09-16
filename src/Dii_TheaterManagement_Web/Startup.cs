using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dapr.Client;
using Dii_TheaterManagement_Web.Clients;
using Dii_TheaterManagement_Web.Features.FakeUsers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Dii_TheaterManagement_Web
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

            services.AddDistributedMemoryCache();
            services.AddHealthChecks();
            services.AddSession(options =>
            {
                options.Cookie.Name = "TheaterWebSessionCookie";
                options.Cookie.HttpOnly = true; // true means disallowing client-side access
                options.IdleTimeout = TimeSpan.FromDays(2);
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

            services.AddScoped<FakeUserManager>();
            services.AddSingleton(provider => Configuration);
            services.AddApplicationServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
                    IApplicationBuilder app,
                    IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/hc");
            });
        }
    }
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            
            services.AddSingleton(typeof(TheaterManagementBffClient), serviceProvider => {
                
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
               

                var httpClient = DaprClient.CreateInvokeHttpClient("diitheatermanagementbff");
                return new TheaterManagementBffClient(httpClient, httpContextAccessor);
            });
            return services;
        }
    }
}
