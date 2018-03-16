﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Bing.Webs.Commons
{
    /// <summary>
    /// 状态码
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Ok=1,
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail=2
    }
}
