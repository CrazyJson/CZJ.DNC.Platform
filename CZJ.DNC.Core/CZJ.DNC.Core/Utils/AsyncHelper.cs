namespace System.Threading.Tasks
{
    /// <summary>
    /// 异步帮助类
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// 同步执行异步方法
        /// </summary>
        /// <typeparam name="TResult">泛型</typeparam>
        /// <param name="function">执行函数</param>
        /// <returns>执行结果</returns>
        /// <example>
        /// <code>
        /// private async Task XY()
        ///   {
        ///         CitmsHttpRequest request = new CitmsHttpRequest
        ///         {
        ///             AddressUrl = "/api/SysConfig/Department/FindAll",
        ///             Token = "F13CECD23F92526B41867EDF7631F5435800C2F33CAAEFFE1C5DFF441337B0B691C348B4C0998A7BCB988396836AF30A",
        ///         };
        ///     var c = await request.SendAsync<ApiResult<List<SysDepartment>>>();
        ///    }
        /// }
        ///    var fuc = new Func<Task>(XY);
        ///    fuc.SyncExcute();
        ///    AsyncHelper.SyncExcute(fuc);
        /// </code>
        /// </example>
        public static TResult SyncExcute<TResult>(this Func<Task<TResult>> function)
        {
            var task = Task.Factory.StartNew(() =>
            {
                return Task.Run(function).GetAwaiter().GetResult();
            });
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 同步执行异步方法
        /// </summary>
        /// <typeparam name="TResult">泛型</typeparam>
        /// <param name="function">执行函数</param>
        /// <returns>执行结果</returns>
        /// <example>
        /// <code>
        /// private async Task XY()
        ///   {
        ///         CitmsHttpRequest request = new CitmsHttpRequest
        ///         {
        ///             AddressUrl = "/api/SysConfig/Department/FindAll",
        ///             Token = "F13CECD23F92526B41867EDF7631F5435800C2F33CAAEFFE1C5DFF441337B0B691C348B4C0998A7BCB988396836AF30A",
        ///         };
        ///     var c = await request.SendAsync<ApiResult<List<SysDepartment>>>();
        ///    }
        /// }
        ///    var fuc = new Func<Task>(XY);
        ///    fuc.SyncExcute();
        ///    AsyncHelper.SyncExcute(fuc);
        /// </code>
        /// </example>
        public static void SyncExcute(this Func<Task> action)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Task.Run(action).GetAwaiter().GetResult();
            });
            task.Wait();
        }
    }
}
