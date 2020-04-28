using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UnitTest
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
            services.AddConsul().Configure((s)=> 
            {
                s.Id = Guid.NewGuid().ToString();
                s.Name = this.GetType().Name;
                s.ServerAddress = "http://localhost:8500";
                s.HealthCheckPath = "/health";
                s.LocalServer = new GreenWhale.Extensions.Consuls.LocalServer//要想使用环境注入的URLS，则不可使用iisexpress；
                {
                    UseEnvironment = true,
                };
            });
            services.AddPolly().Configure(s=>
            {
                s.TimeOutTimeSpan = TimeSpan.FromSeconds(5);
                s.RetryCount = 2;
                s.FallBackRespond = new FallBackRespondMessage
                {
                    Content = "服务已经降级",
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
                s.CircuitBreaker = new CircuitBreakerRespondMessage
                {
                    Content = "服务发生熔断",
                    RecoveryTimeSpan = TimeSpan.FromSeconds(20),
                    ToCloseCount = 2
                };
            });
            services.AddControllers();
            services.AddHealthChecks();
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

            app.UseAuthorization();
            app.UseConsulBuilder().UseConsul();
            app.UsePollyBuilder().UsePollyDefault();
            app.UseHealthChecks(new Microsoft.AspNetCore.Http.PathString("/health"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
