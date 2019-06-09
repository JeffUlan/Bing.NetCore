﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Bing.Utils.Extensions
{
    /// <summary>
    /// 系统扩展 - 验证
    /// </summary>
    public static partial class Extensions
    {
        #region CheckNull(检查对象是否为null)

        /// <summary>
        /// 检查对象是否为null，为null则抛出<see cref="ArgumentNullException"/>异常
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="parameterName">参数名</param>
        public static void CheckNull(this object obj, string parameterName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        #endregion

        #region IsEmpty(是否为空)

        /// <summary>
        /// 判断 字符串 是否为空、null或空白字符串
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 判断 Guid 是否为空、null或Guid.Empty
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        public static bool IsEmpty(this Guid value)
        {
            return value == Guid.Empty;
        }

        /// <summary>
        /// 判断 Guid 是否为空、null或Guid.Empty
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        public static bool IsEmpty(this Guid? value)
        {
            return value == null || IsEmpty(value.Value);
        }

        /// <summary>
        /// 判断 可变字符串 是否为空
        /// </summary>
        /// <param name="sb">数据</param>
        /// <returns></returns>
        public static bool IsEmpty(this StringBuilder sb)
        {
            return sb == null || sb.Length == 0 || sb.ToString().IsEmpty();
        }        

        /// <summary>
        /// 判断 迭代集合 是否为空
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="list">数据</param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return null == list || !list.Any();
        }

        /// <summary>
        /// 判断 字典 是否为空
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">数据</param>
        /// <returns></returns>
        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return null == dictionary || dictionary.Count == 0;
        }

        /// <summary>
        /// 判断 字典 是否为空
        /// </summary>
        /// <param name="dictionary">数据</param>
        /// <returns></returns>
        public static bool IsEmpty(this IDictionary dictionary)
        {
            return null == dictionary || dictionary.Count == 0;
        }

        #endregion

        #region IsNull(是否为空)

        /// <summary>
        /// 判断目标对象是否为空
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public static bool IsNull(this object target)
        {
            return target.IsNull<object>();
        }

        /// <summary>
        /// 判断目标对象是否为空
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public static bool IsNull<T>(this T target)
        {
            var result = ReferenceEquals(target, null);
            return result;
        }

        #endregion

        #region NotEmpty(是否非空)

        #endregion
    }
}
