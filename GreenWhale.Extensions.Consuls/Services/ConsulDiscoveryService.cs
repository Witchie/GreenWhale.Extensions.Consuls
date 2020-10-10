using Consul;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace GreenWhale.Extensions.Consuls
{
	/// <summary>
	/// Consul客户端发现服务
	/// </summary>
	public class ConsulDiscoveryService : IConsulDiscoveryService
	{
		/// <summary>
		/// Consul客户端
		/// </summary>
		public ConsulClient ConsulClient { get; }
		/// <summary>
		/// 服务提供器
		/// </summary>
		public IServiceProvider ServiceProvider { get; }
        /// <summary>
        /// Consul客户端发现服务
        /// </summary>
        /// <param name="consulClient"></param>
        /// <param name="serviceProvider"></param>
        public ConsulDiscoveryService(ConsulClient consulClient, IServiceProvider serviceProvider)
		{
			this.ConsulClient = consulClient;
			ServiceProvider = serviceProvider;
		}

	}
}
