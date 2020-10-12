using GreenWhale.Extensions.Consuls;
using GreenWhale.Extensions.Consuls.Properties;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// 熔断器扩展
	/// </summary>
	public static class PollyExtension
	{
		/// <summary>
		/// 添加Polly
		/// </summary>
		/// <param name="service"></param>
		/// <param name="clientName"></param>
		/// <returns></returns>
		public static OptionsBuilder<PollyInfo> AddPolly(this IServiceCollection service,string clientName= ConstHelper.DefaultPollyName)
		{
			var client=service.AddHttpClient(clientName);
			service.AddSingleton<IPollyBuilder>(new PollyBuilder(client));

			return service.AddOptions<PollyInfo>();
		}
		/// <summary>
		/// 熔断降级构建器
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IPollyBuilder UsePollyBuilder(this IApplicationBuilder services)
		{
			var builder=services.ApplicationServices.GetService<IPollyBuilder>();
			builder.SetProvider(services.ApplicationServices);
			return builder;
		}
		/// <summary>
		/// 使用Polly默认策略
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IPollyBuilder UsePollyDefault(this IPollyBuilder services)
		{
			var provider = services.ServiceProvider.GetService<IOptions<PollyInfo>>();
			var logger = services.ServiceProvider.GetService<ILogger<PollyInfo>>();
			if (provider == null)
			{
				throw new ArgumentNullException("PollyInfo",Resource.you_must_add_polly_first);
			}
			var model =provider.Value;
			services.AddFallBack(model.FallBackRespond.Convert(),async (s)=>
			{
				logger.LogWarning(s.Exception,model.FallBackRespond.Content);
				await Task.CompletedTask;
			});
			services.AddCircuitBreakerAsync(model.CircuitBreaker.ToCloseCount, model.CircuitBreaker.RecoveryTimeSpan,(exc,span)=>
			{
				logger.LogWarning(exc.Exception,exc.Exception.Message);
				logger.LogWarning(string.Format(Resource.opening_time,span));
				logger.LogWarning(model.CircuitBreaker.Content);
			},()=> 
			{
				logger.LogWarning(Resource.service_has_borken_down);
			},()=>{
				logger.LogWarning(Resource.service_is_half_open);
			});
			services.AddRetryAsync(model.RetryCount).AddTimeOutAsync(model.TimeOutTimeSpan);
			return services;
		}
		/// <summary>
		/// 添加降级功能
		/// </summary>
		/// <param name="polly"></param>
		/// <param name="onFallBackMessage">发生降级时调用的应答内容</param>
		/// <param name="onFallbackAsync">发生降级时触发</param>
		/// <returns></returns>
		public static IPollyBuilder AddFallBack(this IPollyBuilder polly, HttpResponseMessage onFallBackMessage, Func<DelegateResult<HttpResponseMessage>, Task> onFallbackAsync)
		{
			var content = Policy<HttpResponseMessage>.HandleInner<Exception>().FallbackAsync(onFallBackMessage, onFallbackAsync);
			polly.HttpClientBuilder.AddPolicyHandler(content);
			return polly;
		}
		/// <summary>
		/// 添加断路器策略
		/// </summary>
		/// <param name="polly"></param>
		/// <param name="toCloseCount">开到关的错误次数</param>
		/// <param name="recoveryTimeSpan">从关到开的时间间隔</param>
		/// <param name="onBreak">当断路了发生</param>
		/// <param name="onReset">当完全恢复时发生</param>
		/// <param name="onHalfOpen">半开闭时发生</param>
		/// <returns></returns>
		public static IPollyBuilder AddCircuitBreakerAsync(this IPollyBuilder polly, int toCloseCount, TimeSpan recoveryTimeSpan, Action<DelegateResult<HttpResponseMessage>, TimeSpan> onBreak, Action onReset, Action onHalfOpen)
		{
			polly.HttpClientBuilder.AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().CircuitBreakerAsync(toCloseCount, recoveryTimeSpan, onBreak, onReset, onHalfOpen));
			return polly;
		}
		/// <summary>
		/// 添加重试
		/// </summary>
		/// <param name="polly"></param>
		/// <param name="retryCount">重试次数</param>
		/// <returns></returns>
		public static IPollyBuilder AddRetryAsync(this IPollyBuilder polly, int retryCount)
		{
			polly.HttpClientBuilder.AddPolicyHandler(Policy<HttpResponseMessage>
			  .Handle<Exception>()
			  .RetryAsync(retryCount));
			return polly;
		}
		/// <summary>
		/// 添加超时策略
		/// </summary>
		/// <param name="polly"></param>
		/// <param name="timeout">超时时间</param>
		/// <returns></returns>
		public static IPollyBuilder AddTimeOutAsync(this IPollyBuilder polly, TimeSpan timeout)
		{
			polly.HttpClientBuilder.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(timeout));
			return polly;
		}
	}
}
