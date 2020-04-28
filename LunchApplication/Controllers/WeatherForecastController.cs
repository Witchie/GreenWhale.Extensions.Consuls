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
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConsulDiscoveryService consulDiscoveryService)
        {
            _logger = logger;
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
}
