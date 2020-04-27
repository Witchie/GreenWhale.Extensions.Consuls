using GreenWhale.Extensions.Consuls;
using Microsoft.Extensions.DependencyInjection;
using System;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using GreenWhale.Extensions.Consuls.Properties;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Consul扩展
	/// </summary>
	public static class ConsulExtension
	{
		/// <summary>
		/// 添加Consul到服务,使用接口<see cref="IConsulDiscoveryService"/>查找服务,如需扩展功能请直接扩展<see cref="IConsulDiscoveryService"/>接口
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static OptionsBuilder<NodeInfo> AddConsul(this IServiceCollection services)
		{
			services.AddSingleton<ConsulClient>();
			services.AddTransient<IConsulDiscoveryService, ConsulDiscoveryService>();
			return services.AddOptions<NodeInfo>();
		}

		/// <summary>
		/// 使用Consul
		/// </summary>
		/// <param name="applicationBuilder"></param>
		/// <returns></returns>
		public static IConsulBuilder UseConsulBuilder(this IApplicationBuilder applicationBuilder)
		{
			var client = applicationBuilder.ApplicationServices.GetService<ConsulClient>();
			var options = applicationBuilder.ApplicationServices.GetService<IOptions<NodeInfo>>();
			if (options.Value != null)
			{
				var builder = new ConsulBuilder(client, options.Value,applicationBuilder);
				return builder;
			}
			else
			{
				throw new ArgumentNullException(Resource.argument_can_not_be_null_or_empty);
			}
		}
	}
}
