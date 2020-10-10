using GreenWhale.Extensions.Consuls;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 键值扩展服务
    /// </summary>
    public static class KeyValueExtension
	{
		/// <summary>
		/// 添加<see cref="IKeyValueService"/>服务
		/// </summary>
		/// <param name="serviceDescriptors"></param>
		/// <returns></returns>
		public static IServiceCollection AddConsulKeyValue(this IServiceCollection serviceDescriptors)
		{
			serviceDescriptors.AddScoped<IKeyValueService, KeyValueService>();
			return serviceDescriptors;
		}
	}
}
