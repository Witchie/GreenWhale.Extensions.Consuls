using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenWhale.Extensions.Consuls
{
	/// <summary>
	/// 本机服务器
	/// </summary>
	public class LocalServer
	{
		/// <summary>
		/// 是否自动生成
		/// </summary>
		public bool UseEnvironment { get; set; }
		/// <summary>
		/// 本机服务地址
		/// </summary>
		public string? LocalAddress { get; set; }
		/// <summary>
		/// 本机服务端口
		/// </summary>
		public int? LocalPort { get; set; }
	}
}
