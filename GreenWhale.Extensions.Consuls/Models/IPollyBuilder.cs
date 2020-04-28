using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 构建器接口
	/// </summary>
	public interface IPollyBuilder
	{
		/// <summary>
		/// 客户端构建器
		/// </summary>
		IHttpClientBuilder HttpClientBuilder { get; }
		/// <summary>
		/// 服务总线
		/// </summary>
		IServiceProvider ServiceProvider { get;  set; }
		/// <summary>
		/// 设置构建器
		/// </summary>
		/// <param name="provider"></param>
		void SetProvider(IServiceProvider provider) => ServiceProvider = provider;
	}
}
