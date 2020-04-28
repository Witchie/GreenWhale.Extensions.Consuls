using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 发生短路时的配置消息
	/// </summary>
	public class CircuitBreakerRespondMessage : MessageBase
	{
		/// <summary>
		/// 从闭合到断开的重试次数
		/// </summary>
		public int ToCloseCount { get; set; }
		/// <summary>
		/// 从断开状态恢复到闭合状态的时间间隔
		/// </summary>
		public TimeSpan RecoveryTimeSpan { get; set; }
	}
}
