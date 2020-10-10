using Consul;

using System.Text;
using System.Threading.Tasks;

namespace GreenWhale.Extensions.Consuls
{
    /// <summary>
    /// 键值存储服务
    /// </summary>
    public class KeyValueService : IKeyValueService
    {
        private readonly ConsulClient client;
        /// <summary>
        /// 键值存储服务
        /// </summary>
        /// <param name="client"></param>
        public KeyValueService(ConsulClient client)
        {
            this.client = client;
        }
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string?> GetString(string key)
        {
            var temp = await Get(key);
            if (temp != null)
            {
                return Encoding.Default.GetString(temp);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 将字符串存储
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task Put(string key, string value = "")
        {
            await client.KV.Put(new KVPair(key) { Value = Encoding.Default.GetBytes(value) });
        }
        /// <summary>
        /// 读取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<byte[]?> Get(string key)
        {
            var result = await client.KV.Get(key);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.Response.Value;
            }
            else
            {
                return null;
            }
        }
    }
}
