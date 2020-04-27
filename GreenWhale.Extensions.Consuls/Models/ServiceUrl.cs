using System;

namespace GreenWhale.Extensions.Consuls
{
	/// <summary>
	/// 服务地址
	/// </summary>
	public class ServiceUrl
	{
		/// <summary>
		/// 服务地址
		/// </summary>
		/// <param name="urlPath">URL</param>
		public ServiceUrl(string urlPath)
		{
			UrlPath = urlPath;
		}
		/// <summary>
		/// 客户端地址
		/// </summary>
		public string UrlPath { get; set; }
	}
}
