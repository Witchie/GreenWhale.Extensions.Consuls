using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 构建器
	/// </summary>
	public class PollyBuilder : IPollyBuilder
	{
		/// <summary>
		/// 构建器
		/// </summary>
		/// <param name="httpclientBuilder">HTTP构建器</param>
		public PollyBuilder(IHttpClientBuilder httpclientBuilder)
		{
			this.HttpClientBuilder = httpclientBuilder;
		}
		/// <summary>
		/// Polly客户端构建器
		/// </summary>
		public IHttpClientBuilder HttpClientBuilder { get; }
		/// <summary>
		/// 服务总线
		/// </summary>
		public IServiceProvider ServiceProvider { get; set; }
	}
}
