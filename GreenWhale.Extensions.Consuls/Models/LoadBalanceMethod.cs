using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 负载均衡算法
	/// </summary>
	public enum LoadBalanceMethod
	{
		/// <summary>
		/// 随机
		/// </summary>
		Random,
		/// <summary>
		/// 直接取默认
		/// </summary>
		None,
		/// <summary>
		/// 自增均衡器
		/// </summary>
		Increment,
	}
}
