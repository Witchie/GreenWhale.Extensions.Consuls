using Consul;

using GreenWhale.Extensions.Consuls;
using GreenWhale.Extensions.Consuls.Properties;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Consul构建器扩展
    /// </summary>
    public static class ConsulRegisterBuilderExtension
	{
		/// <summary>
		/// 转换为 注册服务模型
		/// </summary>
		/// <param name="node">节点信息</param>
		/// <param name="builder">应用构建器</param>
		/// <returns></returns>
		public static AgentServiceRegistration Convert(this NodeInfo node, IApplicationBuilder builder)
		{
			var regist = new AgentServiceRegistration()
			{
				ID = node.Id,
				Name = node.Name,
				Tags = node.Tags,
				Check = new AgentServiceCheck
				{
					Timeout = TimeSpan.FromSeconds(10),
					DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
					Interval = TimeSpan.FromSeconds(10),
				}
			};
			if (node.LocalServer == null)
			{
				throw new ArgumentNullException("LocalServer", Resource.argument_can_not_be_null_or_empty);
			}
			if (node.LocalServer.UseEnvironment)
			{
				var uri = builder.HostUrls().FirstOrDefault();
				if (uri == null)
				{
					//使用IISExpress可能出现该异常
					throw new ArgumentNullException("uri", Resource.maybe_you_use_iisexpress);
				}
				regist.Address = $"{uri.Scheme}://{uri.Host}";
				regist.Port = uri.Port;
			}
			else
			{

				if (node.LocalServer.LocalPort == null)
				{
					throw new ArgumentNullException("LocalPort", Resource.argument_can_not_be_null_or_empty);
				}
				if (node.LocalServer.LocalAddress == null)
				{
					throw new ArgumentNullException("LocalAddress", Resource.argument_can_not_be_null_or_empty);
				}
				regist.Address = node.LocalServer?.LocalAddress;
				regist.Port = node.LocalServer?.LocalPort.Value??0;
			}
			if (string.IsNullOrEmpty(node.HealthCheckPath))
			{
				throw new ArgumentNullException("HealthCheckPath", Resource.argument_can_not_be_null_or_empty);
			}
			else
			{
				regist.Check.HTTP = $"{regist.Address}:{regist.Port}{node.HealthCheckPath}";
			}
			return regist;
		}
		/// <summary>
		/// 注册到服务中心并注册注销事件,默认会调用健康检查，你需要指定对应的请求url
		/// </summary>
		/// <param name="builder">Consul构建器</param>
		/// <returns></returns>
		public static IConsulBuilder UseConsul(this IConsulBuilder builder)
		{
			return builder.Register().OnApplicationStopDeregister();
		}
		/// <summary>
		/// 注册服务到Consul服务中心
		/// </summary>
		public static  IConsulBuilder Register(this IConsulBuilder builder)
		{
			try
			{
				builder.Client.Config.Address = new Uri(builder.NodeInfo.ServerAddress);
				var info = builder.NodeInfo.Convert(builder.Builder);
				builder.Client.Agent.ServiceRegister(info).Wait();
				return builder;
			}
			catch (Exception exe)
			{
				throw new Exception("无法连接到Consul服务器",exe);
			}

		}
		/// <summary>
		/// 从服务中心注销当前服务
		/// </summary>
		/// <param name="builder">Consul构建器</param>
		/// <returns>Consul构建器</returns>
		public static  IConsulBuilder Deregister(this IConsulBuilder builder)
		{
			builder.Client.Config.Address = new Uri(builder.NodeInfo.ServerAddress);
			builder.Client.Agent.ServiceDeregister(builder.NodeInfo.Id).Wait();
			return builder;
		}
		/// <summary>
		/// 应用程序退出，注销当前服务
		/// </summary>
		/// <param name="builder">Consul构建器</param>
		/// <returns>Consul构建器</returns>
		public static IConsulBuilder OnApplicationStopDeregister(this IConsulBuilder builder)
		{
			var lifttime=	builder.Services.GetService<IHostApplicationLifetime>();
			lifttime.ApplicationStopped.Register(() =>
			{
				builder.Deregister();
			});
			return builder;
		}
	}
}
