using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Consul;

namespace UnitTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConsulDiscoveryService consulDiscoveryService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConsulDiscoveryService consulDiscoveryService)
        {
            _logger = logger;
            this.consulDiscoveryService = consulDiscoveryService;
        }

        [HttpGet]
        public async Task<IReadOnlyList<CatalogService>> Get()
        {
            var services= await  consulDiscoveryService.Discovery(nameof(Startup));
            _logger.LogInformation("查找服务");
            return services;
        }
    }
}
