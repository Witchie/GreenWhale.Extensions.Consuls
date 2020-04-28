using System;
using System.Collections.Generic;

namespace GreenWhale.Extensions.Consuls
{
	/// <summary>
	/// 负载均衡选择器
	/// </summary>
	public interface ILoadBalance
	{
		/// <summary>
		/// 服务均衡选择器
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		T Select<T>(IEnumerable<T> list);
	}
}
