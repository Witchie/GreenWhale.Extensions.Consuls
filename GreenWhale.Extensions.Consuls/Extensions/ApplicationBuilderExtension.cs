using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 应用构建器扩展
	/// </summary>
	public static class ApplicationBuilderExtension
	{
		/// <summary>
		/// 当前应用监听的URI地址
		/// </summary>
		/// <param name="applicationBuilder"></param>
		/// <returns></returns>
		public static IReadOnlyList<Uri> HostUrls(this IApplicationBuilder applicationBuilder)
		{
			var address = applicationBuilder.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
			return address.Select(p => new Uri(p)).ToList();
		}
	}
}
