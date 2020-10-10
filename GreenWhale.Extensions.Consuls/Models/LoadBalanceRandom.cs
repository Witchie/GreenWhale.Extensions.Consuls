using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GreenWhale.Extensions.Consuls
{
	/// <summary>
	/// 随机负载均衡器
	/// </summary>
	public class LoadBalanceRandom : LoadBalanceAbstract
	{
		Random random = new Random();
		/// <summary>
		/// 负载均衡随机选择器
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public override T? Select<T>(IEnumerable<T> list) where T : class
		{
			if (list is null)
			{
				throw new ArgumentNullException(nameof(list));
			}
			if (list.Count() == 1) return list.FirstOrDefault();
			var number = random.Next(list.Count());
			return list.ToArray()[number];
		}
	}
}
