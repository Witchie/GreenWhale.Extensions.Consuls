using Consul;
using GreenWhale.Extensions.Consuls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
		/// <param name="service"></param>
		/// <param name="serviceName">服务名称</param>
		/// <returns></returns>
		public static async Task<IReadOnlyList<CatalogService>> FindAll(this IConsulDiscoveryService service, string serviceName)
		{
			if (service is null)
			{
				throw new ArgumentNullException(nameof(service));
			}

			if (string.IsNullOrEmpty(serviceName))
			{
				throw new ArgumentException("message", nameof(serviceName));
			}

			var clients = await service.ConsulClient.Catalog.Service(serviceName);
			return clients.Response;
		}
		/// <summary>
		/// 查找并使用负载均衡器过滤
		/// </summary>
		/// <param name="service"></param>
		/// <param name="method"></param>
		/// <param name="serviceName"></param>
		/// <returns></returns>
		public static async Task<CatalogService> FindBalance(this IConsulDiscoveryService service, LoadBalanceMethod method, string serviceName)
		{
			if (string.IsNullOrEmpty(serviceName))
			{
				throw new ArgumentException("message", nameof(serviceName));
			}

			var clients=await service.FindAll(serviceName);
			var selector=service.ServiceProvider.GetService<ILoadBalanceSelector>();
			var catalog = selector.Select(method).Select(clients); 
			return catalog;
		}
		/// <summary>
		/// 依据服务和负载均衡策略查找可调用的服务
		/// </summary>
		/// <param name="service"></param>
		/// <param name="serviceName">要查找的服务名称</param>
		/// <param name="balanceMethd">所用的负载均衡算法</param>
		/// <param name="httpclientName">polly客户端名称，除非存在多个，否则用more你的就好</param>
		/// <returns></returns>
		public static async Task<HttpClient> Find(this IConsulDiscoveryService service,string serviceName, LoadBalanceMethod balanceMethd = LoadBalanceMethod.Random,string httpclientName="micro")
		{
			if (string.IsNullOrEmpty(serviceName))
			{
				throw new ArgumentException("message", nameof(serviceName));
			}

			var factory=service.ServiceProvider.GetService<IHttpClientFactory>();
			var cataglog=await	service.FindBalance(balanceMethd,serviceName);
			var client=factory.CreateClient(httpclientName ?? "micro");
			client.BaseAddress =new Uri(cataglog.ServiceAddress+":"+cataglog.ServicePort);
			return client;
		}
	}
}
