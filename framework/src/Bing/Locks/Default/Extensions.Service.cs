﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bing.Locks.Default
{
    /// <summary>
    /// 业务锁扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 注册业务锁
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddLock(this IServiceCollection services) => services.TryAddScoped<ILock, DefaultLock>();

        /// <summary>
        /// 注册业务锁
        /// </summary>
        /// <typeparam name="TLock">业务锁类型</typeparam>
        /// <param name="services">服务集合</param>
        public static void AddLock<TLock>(this IServiceCollection services) where TLock : class, ILock =>
            services.TryAddScoped<ILock, TLock>();
    }
}
