using GreenWhale.Extensions.Consuls;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 负载均衡器选择接口
	/// </summary>
	public interface ILoadBalanceSelector
	{
		/// <summary>
		/// 选择负载均衡器
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		LoadBalanceAbstract Select(LoadBalanceMethod method);
	}
}
