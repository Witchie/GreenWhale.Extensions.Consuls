using Consul;
using GreenWhale.Extensions.Consuls;
using Microsoft.AspNetCore.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Consul构建器接口
	/// </summary>
	public interface IConsulBuilder
	{
		IApplicationBuilder Builder { get; }
		/// <summary>
		/// 服务总线
		/// </summary>
		IServiceProvider Services => Builder.ApplicationServices;
		/// <summary>
		/// 节点信息
		/// </summary>
		NodeInfo NodeInfo { get;}
		/// <summary>
		/// Consul客户端
		/// </summary>
		 ConsulClient Client { get; }
	}
}
