﻿using Bing.Exceptions;
using Bing.Utils.Helpers;
using Bing.Webs.Commons;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bing.Webs.Filters
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class ExceptionHandlerAttribute:ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context">异常上下文</param>
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            if (context.Exception is Warning warning && !string.IsNullOrWhiteSpace(warning.Code))
            {
                context.Result = new Result(Conv.ToInt(warning.Code), context.Exception.Message);
            }
            context.Result = new Result(StateCode.Fail, context.Exception.Message);
        }
    }
}
