using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 继电器信息
	/// </summary>
	public class PollyInfo
	{
		/// <summary>
		/// 发生降级时应答内容
		/// </summary>
		[NotNull]
		public FallBackRespondMessage FallBackRespond { get; set; } = new FallBackRespondMessage();
		/// <summary>
		/// 断路器策略
		/// </summary>
		public CircuitBreakerRespondMessage CircuitBreaker { get; set; } = new CircuitBreakerRespondMessage();
		/// <summary>
		/// 重试次数
		/// </summary>
		public int RetryCount { get; set; }
		/// <summary>
		/// 超时时间（服务不可访问）
		/// </summary>
		public TimeSpan TimeOutTimeSpan { get; set; }
	}
}
