//******************************************************************************
//
// 文 件 名: IReadable.cs
//
// 描    述: 可以读取的对象
//
// 作    者: 陈飞
//
// 地    点: 中科通达
//
// 创建时间: 2011-10-14
//
// 修改历史: 2011-10-14 陈飞创建
//******************************************************************************

namespace CZJ.DNC.License
{
	/// <summary>
	/// 可以读取的对象
	/// </summary>
	public interface IReadable
	{
		/// <summary>
		/// 读取数据
		/// </summary>
		/// <param name="buffer">将输入写入到其中的字节数组</param>
		/// <param name="offset">缓冲区数组中开始写入的偏移量</param>
		/// <param name="count">要读取的字节数</param>
		/// <returns>读取的字节数</returns>
		int Read(byte[] buffer, int offset, int count);
	}
}
