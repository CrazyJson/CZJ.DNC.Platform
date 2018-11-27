//******************************************************************************
//
// 文 件 名: NormalException.cs
//
// 描    述: 表示 Baiynui Library 中发生的异常
//
// 作    者: 陈飞
//
// 地    点: 曙光新村 30 号
//
// 创建时间: 2011-04-04
//
// 修改历史: 2011-04-04 陈飞创建
//******************************************************************************

using System;
using System.Runtime.Serialization;

namespace CZJ.DNC.License
{
	/// <summary>
	/// 表示 Baiynui Library 中发生的异常
	/// </summary>
	public class NormalException : Exception
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public NormalException()
		{

		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="message">错误消息</param>
		public NormalException(string message)
			: base(message)
		{

		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="message">错误消息</param>
		/// <param name="innerException">内部异常</param>
		public NormalException(string message, Exception innerException)
			: base(message, innerException)
		{

		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="info">序列化信息</param>
		/// <param name="context">序列化上下文环境</param>
		public NormalException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NormalException"/> class.
		/// </summary>
		/// <param name="innerException">The inner exception.</param>
		public NormalException(Exception innerException)
			: this(innerException.Message, innerException)
		{
		
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NormalException"/> class.
		/// </summary>
		/// <param name="messageFormat">The message format.</param>
		/// <param name="args">The args.</param>
		public NormalException(string messageFormat, params object [] args)
			: this(string.Format(messageFormat, args))
		{

		}
	}
}
