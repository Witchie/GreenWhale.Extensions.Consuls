## Consul的注册与发现

**使用方式**

Nuget安装`GreenWhale.Extensions.Consuls` 依次添加Consul，并配置Consul。
在这里你需要指定Consul服务器的地址，本机的地址，ID，名称等。

然后`UseConsulBuilder().UseConsul()`;


``` cs
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
```
## 服务器的发现
在要发现的控制器中直接`IConsulDiscoveryService`注入。如果觉得默认方法不够用，扩展IConsulDiscoveryService 即可。
```cs

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConsulDiscoveryService consulDiscoveryService;
        public WeatherForecastController(IConsulDiscoveryService consulDiscoveryService)
        {
            this.consulDiscoveryService = consulDiscoveryService;
        }

        [HttpGet]
        public async Task<Uri> Get()
        {
            var services= await  consulDiscoveryService.Find("Startup");
            _logger.LogInformation("服务的URI已查找到");
            return services.BaseAddress;
        }
    }
```


## 特别注意

>> 如果你使用IIS或者IISExpress则你必须手动指定本机地址和端口，如果你使用非IIS方式，则直接指定UseEnviroment为True即可。

## 关于配置文件
如果要用配置文件，则在对应的配置文件中编写一样的名称的键值对就可以了。
``` cs
	/// <summary>
	/// Consul节点信息
	/// </summary>
	public class NodeInfo
	{
		/// <summary>
		/// 服务ID
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 服务名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 附加信息
		/// </summary>
		public string[] Tags { get; set; }
		/// <summary>
		/// 本机服务器
		/// </summary>
		public LocalServer LocalServer { get; set; }
		/// <summary>
		/// Consul服务器地址
		/// </summary>
		public string ServerAddress { get; set; }
		/// <summary>
		/// 健康检查地址,默认 "/health"
		/// </summary>
		public string HealthCheckPath { get; set; } = "/health";
	}
```
当`UseEnvironment==true` 时将自动用 aspnetcore监听的地址和端口，当为false时，你需要手动指定本机的地址和端口
```cs
	/// <summary>
	/// 本机服务器
	/// </summary>
	public class LocalServer
	{
		/// <summary>
		/// 是否自动生成
		/// </summary>
		public bool UseEnvironment { get; set; }
		/// <summary>
		/// 本机服务地址
		/// </summary>
		public string? LocalAddress { get; set; }
		/// <summary>
		/// 本机服务端口
		/// </summary>
		public int? LocalPort { get; set; }
	}
```
单元测试代码如下，由于git无法签入cosnul（太大）故而没有编写自动测试的测试用例
```cs
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
```