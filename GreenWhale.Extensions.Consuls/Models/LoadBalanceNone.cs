using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenWhale.Extensions.Consuls
{
	/// <summary>
	/// 不做随机选择直接返回第一个
	/// </summary>
	public class LoadBalanceNone : LoadBalanceAbstract
	{
		/// <summary>
		/// 负载均衡选择
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public override T? Select<T>(IEnumerable<T> list) where T : class
		{
			return list.FirstOrDefault();
		}
	}
}
