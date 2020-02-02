﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bing.Exceptions;
using Bing.Helpers;
using Bing.Logs.Contents;
using Bing.Logs.Properties;
using Bing.Utils.Extensions;
using Bing.Utils.Helpers;

namespace Bing.Logs.Extensions
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static partial class LogExtensions
    {
        /// <summary>
        /// 设置业务编号
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="bussinessId">业务编号</param>
        /// <returns></returns>
        public static ILog BussinessId(this ILog log, string bussinessId)
        {
            return log.Set<LogContent>(content =>
            {
                if (string.IsNullOrWhiteSpace(content.BussinessId) == false)
                {
                    content.BussinessId += ",";
                }

                content.BussinessId += bussinessId;
            });
        }

        /// <summary>
        /// 设置模块
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="module">业务编号</param>
        /// <returns></returns>
        public static ILog Module(this ILog log, string module)
        {
            return log.Set<LogContent>(content => content.Module = module);
        }

        /// <summary>
        /// 设置类名
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="class">类名</param>
        /// <returns></returns>
        public static ILog Class(this ILog log, string @class)
        {
            return log.Set<LogContent>(content => content.Class = @class);
        }

        /// <summary>
        /// 设置方法
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="method">方法</param>
        /// <returns></returns>
        public static ILog Method(this ILog log, string method)
        {
            return log.Set<LogContent>(content => content.Method = method);
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static ILog Params(this ILog log, string value)
        {
            return log.Set<LogContent>(content => content.AppendLine(content.Params, value));
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <returns></returns>
        public static ILog Params(this ILog log, string name, string value, string type = null)
        {
            return log.Set<LogContent>(content =>
            {
                if (string.IsNullOrWhiteSpace(type))
                {
                    content.AppendLine(content.Params,
                        $"{LogResource.ParameterName}: {name}, {LogResource.ParameterValue}: {value}");
                    return;
                }
                content.AppendLine(content.Params,
                    $"{LogResource.ParameterType}: {type}, {LogResource.ParameterName}: {name}, {LogResource.ParameterValue}: {value}");
            });
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="dictionary">字典</param>
        /// <returns></returns>
        public static ILog Params(this ILog log, IDictionary<string, object> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return log;
            }
            foreach (var item in dictionary)
            {
                Params(log, item.Key, item.Value.SafeString());
            }
            return log;
        }

        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="caption">标题</param>
        /// <returns></returns>
        public static ILog Caption(this ILog log, string caption)
        {
            return log.Set<LogContent>(content => content.Caption = caption);
        }

        /// <summary>
        /// 设置Sql语句
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static ILog Sql(this ILog log, string value)
        {
            return log.Set<LogContent>(content => content.Sql.AppendLine(value));
        }

        /// <summary>
        /// 设置Sql参数
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static ILog SqlParams(this ILog log, string value)
        {
            return log.Set<LogContent>(content => content.AppendLine(content.SqlParams, value));
        }

        /// <summary>
        /// 设置Sql参数
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="list">键值对列表</param>
        /// <returns></returns>
        public static ILog SqlParams(this ILog log, IEnumerable<KeyValuePair<string, object>> list)
        {
            if (list == null)
            {
                return log;
            }
            var dictionary = list.ToList();
            if (dictionary.Count == 0)
            {
                return log;
            }
            var result = new StringBuilder();
            foreach (var item in dictionary)
            {
                result.AppendLine($"    {item.Key} : {GetParamLiterals(item.Value)} : {item.Value?.GetType()},");
            }

            return SqlParams(log, result.ToString().RemoveEnd($",{Common.Line}"));
        }

        /// <summary>
        /// 获取参数字面值
        /// </summary>
        /// <param name="value">参数值</param>
        private static string GetParamLiterals(object value)
        {
            if (value == null)
                return "''";
            switch (value.GetType().Name.ToLower())
            {
                case "boolean":
                    return Conv.ToBool(value) ? "1" : "0";
                case "int16":
                case "int32":
                case "int64":
                case "single":
                case "double":
                case "decimal":
                    return value.SafeString();
                default:
                    return $"'{value}'";
            }
        }

        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="exception">异常</param>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static ILog Exception(this ILog log, Exception exception, string errorCode = "")
        {
            if (exception == null)
            {
                return log;
            }

            return log.Set<LogContent>(content =>
            {
                content.Exception = exception;
                content.ErrorCode = errorCode;
            });
        }

        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="exception">异常</param>
        /// <returns></returns>
        public static ILog Exception(this ILog log, Warning exception)
        {
            if (exception == null)
            {
                return log;
            }

            return Exception(log, exception, exception.Code);
        }
    }
}
