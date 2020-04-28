using System;
using System.Linq;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	///消息应答扩展
	/// </summary>
	public static class RespondMessageExtension
	{
		/// <summary>
		/// 转换
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static HttpResponseMessage Convert(this FallBackRespondMessage message)
		{
			var respond = new HttpResponseMessage()
			{
				Content = new StringContent(message.Content),
				StatusCode = message.StatusCode
			};
			return respond;
		}
	}
}
