using GreenWhale.Extensions.Consuls;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 负载均衡选择器
	/// </summary>
	public class LoadBalanceSelector : ILoadBalanceSelector
	{
		/// <summary>
		/// 负载均衡选择器
		/// </summary>
		/// <param name="provider"></param>
		public LoadBalanceSelector(IServiceProvider provider)
		{
			serviceProvider = provider;
		}
		private readonly IServiceProvider serviceProvider;
		/// <summary>
		/// 选择负载均衡器
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public LoadBalanceAbstract? Select(LoadBalanceMethod method)
		{
            return method switch
            {
                LoadBalanceMethod.Random => serviceProvider.GetService<LoadBalanceRandom?>(),
                LoadBalanceMethod.None => serviceProvider.GetService<LoadBalanceNone?>(),
                LoadBalanceMethod.Increment => serviceProvider.GetService<LoadBalanceIncrement?>(),
                _ => throw new NotSupportedException(method.ToString()),
            };
        }
	}
}
