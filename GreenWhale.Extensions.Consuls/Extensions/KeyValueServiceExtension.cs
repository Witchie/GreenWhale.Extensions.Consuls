
using GreenWhale.Extensions.Consuls;

using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 键值存储服务扩展
    /// </summary>
    public static class KeyValueServiceExtension
	{
		/// <summary>
		/// 读取指定对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="keyValue"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static async Task<T?> Get<T>(this IKeyValueService keyValue, string key) where T:class
		{
			var temp = await keyValue.GetString(key);
			if (string.IsNullOrEmpty(temp))
			{
				return JsonConvert.DeserializeObject<T>(temp);
			}
			else
			{
				return default;
			}
		}
		/// <summary>
		/// 存储指定对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="keyValue"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static async Task Put<T>(this IKeyValueService keyValue, string key, T value)
		{
			await keyValue.Put(key, JsonConvert.SerializeObject(value));
		}
	}
}
