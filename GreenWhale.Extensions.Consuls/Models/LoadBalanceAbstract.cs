using System;
using System.Collections.Generic;

namespace GreenWhale.Extensions.Consuls
{
	/// <summary>
	/// 负载均衡选择器
	/// </summary>
	public abstract class LoadBalanceAbstract : ILoadBalance
	{
		/// <summary>
		/// 服务均衡选择器
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public abstract T Select<T>(IEnumerable<T> list);
	}
}
