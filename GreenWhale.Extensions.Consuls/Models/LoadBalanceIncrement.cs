using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenWhale.Extensions.Consuls
{
	/// <summary>
	/// 自增负载均衡器
	/// </summary>
	public class LoadBalanceIncrement : LoadBalanceAbstract
	{
		int count = 0;
		int current = 0;
		private readonly static object _locker = new object();
		/// <summary>
		/// 自增负载均衡器
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public override T? Select<T>(IEnumerable<T> list) where T:class
		{
			var array = list.ToArray();
			count = array.Count();
			lock (_locker)
			{
				current++;
				var t = array[current];
				if (current >= count)
				{
					current = 0;
				}
				return t;
			}

		}
	}
}
