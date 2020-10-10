using System.Threading.Tasks;

namespace GreenWhale.Extensions.Consuls
{
    /// <summary>
    /// 键值存储服务
    /// </summary>
    public interface IKeyValueService
	{
        /// <summary>
        /// 读取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<byte[]?> Get(string key);
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string?> GetString(string key);
        /// <summary>
        /// 将字符串存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task Put(string key, string value = "");
	}
}
