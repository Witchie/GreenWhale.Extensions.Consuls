using Consul;
using System;
using System.Runtime.CompilerServices;

namespace GreenWhale.Extensions.Consuls
{
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
}
