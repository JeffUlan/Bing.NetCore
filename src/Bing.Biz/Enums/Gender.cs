﻿using System.ComponentModel;
using Bing.Utils.Extensions;

namespace Bing.Biz.Enums
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 女
        /// </summary>
        [Description("女士")]
        Female = 1,
        /// <summary>
        /// 男
        /// </summary>
        [Description("先生")]
        Male = 2
    }

    /// <summary>
    /// 性别枚举扩展
    /// </summary>
    public static class GenderExtensions
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="gender">性别</param>
        /// <returns></returns>
        public static string Description(this Gender? gender)
        {
            return gender == null ? string.Empty : gender.Value.Description();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="gender">性别</param>
        /// <returns></returns>
        public static int? Value(this Gender? gender)
        {
            return gender?.Value();
        }
    }
}
