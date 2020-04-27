using Consul;
using GreenWhale.Extensions.Consuls;
using Microsoft.AspNetCore.Builder;
using System;
namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Consul构建器
	/// </summary>
	public class ConsulBuilder : IConsulBuilder
	{
		public ConsulBuilder(ConsulClient consulClient, NodeInfo consulNodeInfo, IApplicationBuilder builder)
		{
			Client = consulClient;
			NodeInfo = consulNodeInfo;
			Builder = builder;
		}
		/// <summary>
		/// 注册节点信息
		/// </summary>
		public NodeInfo NodeInfo { get; }
		/// <summary>
		/// Consul客户端
		/// </summary>
		public ConsulClient Client { get; }
		/// <summary>
		/// 应用构建器
		/// </summary>
		public IApplicationBuilder Builder { get; }
	}
}
