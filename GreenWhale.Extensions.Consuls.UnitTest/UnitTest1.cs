using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using NUnit.Framework;
using System;
using System.Linq;

namespace GreenWhale.Extensions.Consuls.UnitTest
{
    public class Tests
    {
        ApplicationBuilder application;
        TestApplicationLifetime lifeTime = new TestApplicationLifetime();

        /// <summary>
        /// 测试前请先打开Consul服务端
        /// </summary>
        [SetUp]
        public void Setup()
        {
            ServiceCollection services = new ServiceCollection();
            var feature = new ServerAddressesFeature();
            feature.Addresses.Add("http://127.0.0.1:1003");
            services.AddConsul().Configure(s=> 
            {
                var url = application.HostUrls().FirstOrDefault();
                s.Id = Guid.NewGuid().ToString();
                s.ServerAddress = "http://127.0.0.1:8500";
                s.LocalServer = new LocalServer
                {
                    LocalAddress =$"{url.Scheme}://{url.Host}",
                    LocalPort = url.Port
                };
                s.Name = "Test";
            });
            services.AddSingleton<IHostApplicationLifetime, TestApplicationLifetime>();
            application = new ApplicationBuilder(services.BuildServiceProvider());
            IFeatureCollection featureCollection = new FeatureCollection();
            featureCollection.Set<IServerAddressesFeature>(feature);
            application.Properties.Add("server.Features", featureCollection);
           var fea=  application.ServerFeatures.Get<IServerAddressesFeature>();
            lifeTime.Start();
        }

        [Test]
        public void Test1()
        {
            application.UseConsulBuilder().UseConsul();
            lifeTime.StopApplication();
        }
    }
}