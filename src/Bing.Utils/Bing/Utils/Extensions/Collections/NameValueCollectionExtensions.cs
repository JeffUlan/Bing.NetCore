﻿using System.Collections.Specialized;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Bing.Utils.Extensions
{
    /// <summary>
    /// 键值对集合(<see cref="NameValueCollection"/>) 扩展
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        #region ToQueryString(将键值对集合转换成查询字符串)

        /// <summary>
        /// 将键值对集合转换成查询字符串
        /// </summary>
        /// <param name="collection">键值对集合</param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection collection)
        {
            if (collection == null || !collection.HasKeys())
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (string key in collection.Keys)
            {
                sb.Append($"{key}={collection[key]}&");
            }

            sb.TrimEnd("&");
            return sb.ToString();
        }

        #endregion
    }
}
