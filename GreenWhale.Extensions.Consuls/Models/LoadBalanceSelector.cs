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
		private IServiceProvider serviceProvider;
		/// <summary>
		/// 选择负载均衡器
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public LoadBalanceAbstract Select(LoadBalanceMethod method)
		{
			switch (method)
			{
				case LoadBalanceMethod.Random:
					return serviceProvider.GetService<LoadBalanceRandom>();
				case LoadBalanceMethod.None:
					return serviceProvider.GetService<LoadBalanceNone>();
				case LoadBalanceMethod.Increment:
					return serviceProvider.GetService<LoadBalanceIncrement>();
				default:
					throw new NotSupportedException(method.ToString());
			}
		}
	}
}
