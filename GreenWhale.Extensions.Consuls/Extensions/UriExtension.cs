using System;

namespace Microsoft.Extensions.DependencyInjection
{
	internal static class UriExtension
	{
		/// <summary>
		/// 转换成URL
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public static Uri ToUri(this string content)
		{
			return new Uri(content);
		}
	}
}
