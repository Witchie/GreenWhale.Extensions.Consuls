using System;
using System.Linq;
using System.Net;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 发生降级时应答内容
	/// </summary>
	public class FallBackRespondMessage : MessageBase
	{
		/// <summary>
		/// HTTP状态码
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }

	}
}
