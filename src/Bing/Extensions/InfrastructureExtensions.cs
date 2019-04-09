﻿using System;
using System.Text;
using AspectCore.Configuration;
using Bing.Configurations;
using Bing.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Bing
{
    /// <summary>
    /// 基础设施扩展
    /// </summary>
    public static partial class InfrastructureExtensions
    {
        /// <summary>
        /// 注册Bing基础设施服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configs">依赖配置</param>
        /// <returns></returns>
        public static IServiceProvider AddBing(this IServiceCollection services, params IConfig[] configs)
        {
            return AddBing(services, null, configs);
        }

        /// <summary>
        /// 注册Bing基础设施服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="aopConfigAction">Aop配置操作</param>
        /// <param name="configs">依赖配置</param>
        /// <returns></returns>
        public static IServiceProvider AddBing(this IServiceCollection services,
            Action<IAspectConfiguration> aopConfigAction, params IConfig[] configs)
        {
            services.AddHttpContextAccessor();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.TryAddSingleton<IConfigurationAccessor>(DefaultConfigurationAccessor.Empty);
            return Bootstrapper.Run(services, configs, aopConfigAction);
        }
    }
}
