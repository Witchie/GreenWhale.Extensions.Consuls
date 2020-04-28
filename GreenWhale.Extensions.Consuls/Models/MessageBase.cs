using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 消息内容
	/// </summary>
	public class MessageBase
	{
		/// <summary>
		/// 消息内容
		/// </summary>
		[NotNull]
		public string Content { get; set; }
	}
}
