using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Consul发现服务
	/// </summary>
	public interface IConsulDiscoveryService
	{
		/// <summary>
		/// Consul客户端
		/// </summary>
		ConsulClient ConsulClient { get; }
		/// <summary>
		/// 服务提供器
		/// </summary>
		IServiceProvider ServiceProvider { get; }
	}
}
