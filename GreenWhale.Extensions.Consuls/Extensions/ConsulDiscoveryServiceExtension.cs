using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Consul发现服务扩展
	/// </summary>
	public static class ConsulDiscoveryServiceExtension
	{
		/// <summary>
		/// 查找服务
		/// </summary>
		/// <param name="serviceName">服务名称</param>
		/// <returns></returns>
		public static async Task<IReadOnlyList<CatalogService>> Discovery(this IConsulDiscoveryService service, string serviceName)
		{
			var clients = await service.ConsulClient.Catalog.Service(serviceName);
			return clients.Response;
		}
	}
}
