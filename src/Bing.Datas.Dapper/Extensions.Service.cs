﻿using System;
using Bing.Datas.Dapper.Handlers;
using Bing.Datas.Dapper.MySql;
using Bing.Datas.Dapper.PgSql;
using Bing.Datas.Dapper.SqlServer;
using Bing.Datas.Enums;
using Bing.Datas.Matedatas;
using Bing.Datas.Sql;
using Bing.Datas.Sql.Configs;
using Bing.Utils.Extensions;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bing.Datas.Dapper
{
    /// <summary>
    /// 服务扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 注册Sql查询服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="action">Sql查询配置</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlQuery(this IServiceCollection services,
            Action<SqlQueryOptions> action = null)
        {
            return AddSqlQuery(services, action, null, null);
        }

        /// <summary>
        /// 注册Sql查询服务
        /// </summary>
        /// <typeparam name="TDatabase">IDatabase实现类型，提供数据库连接</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="action">Sql查询配置</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlQuery<TDatabase>(this IServiceCollection services,
            Action<SqlQueryOptions> action = null) where TDatabase : class, IDatabase
        {
            return AddSqlQuery(services, action, typeof(TDatabase), null);
        }

        /// <summary>
        /// 注册Sql查询服务
        /// </summary>
        /// <typeparam name="TDatabase">IDatabase实现类型，提供数据库连接</typeparam>
        /// <typeparam name="TEntityMatedata">IEntityMatedata实现类型，提供实体元数据解析</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="configAction">Sql查询配置</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlQuery<TDatabase, TEntityMatedata>(this IServiceCollection services,
            Action<SqlQueryOptions> configAction = null) 
            where TDatabase : class, IDatabase
            where TEntityMatedata : class, IEntityMatedata
        {
            return AddSqlQuery(services, configAction, typeof(TDatabase), typeof(TEntityMatedata));
        }

        /// <summary>
        /// 注册Sql查询服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configAction">Sql查询配置</param>
        /// <param name="database">数据库类型</param>
        /// <param name="entityMatedata">实体元数据解析器类型</param>
        /// <returns></returns>
        private static IServiceCollection AddSqlQuery(IServiceCollection services, Action<SqlQueryOptions> configAction,
            Type database, Type entityMatedata)
        {
            if (database != null)
            {
                services.TryAddScoped(database);
                services.TryAddScoped(typeof(IDatabase), t => t.GetService(database));
            }

            services.TryAddScoped<ISqlQuery, SqlQuery>();
            if (entityMatedata != null)
            {
                services.TryAddScoped(typeof(IEntityMatedata), t => t.GetService(entityMatedata));
            }

            var config = new SqlQueryOptions();
            if (configAction != null)
            {
                configAction.Invoke(config);
                services.Configure(configAction);
            }

            AddSqlBuilder(services, config);
            RegisterTypeHandlers();
            return services;
        }

        /// <summary>
        /// 配置Sql生成器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="config">Sql查询配置</param>
        private static void AddSqlBuilder(IServiceCollection services, SqlQueryOptions config)
        {
            switch (config.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    services.TryAddScoped<ISqlBuilder, SqlServerBuilder>();
                    return;
                case DatabaseType.MySql:
                    services.TryAddScoped<ISqlBuilder, MySqlBuilder>();
                    return;
                case DatabaseType.PgSql:
                    services.TryAddScoped<ISqlBuilder, PgSqlBuilder>();
                    return;
                default:
                    throw new NotImplementedException($"Sql生成器未实现 {config.DatabaseType.Description()} 数据库");
            }
        }

        /// <summary>
        /// 注册类型处理器
        /// </summary>
        private static void RegisterTypeHandlers()
        {
            SqlMapper.AddTypeHandler(typeof(string), new StringTypeHandler());
        }
    }
}
