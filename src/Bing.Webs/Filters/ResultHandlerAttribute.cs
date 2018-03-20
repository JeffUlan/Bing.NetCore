﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Bing.Webs.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bing.Webs.Filters
{
    /// <summary>
    /// 响应结果处理过滤器
    /// </summary>
    public class ResultHandlerAttribute: ResultFilterAttribute
    {
        /// <summary>
        /// 结果处理
        /// </summary>
        /// <param name="context">结果执行上下文</param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // 控制器过滤
            if (context.Controller.GetType().GetCustomAttributes<IgnoreResultHandlerAttribute>().Any())
            {
                return;
            }
            // Action过滤
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var ignore = controllerActionDescriptor.MethodInfo
                    .GetCustomAttributes<IgnoreResultHandlerAttribute>().Any();
                if (ignore)
                {
                    return;
                }
            }

            if (context.Result is ObjectResult objectResult)
            {
                context.Result = new Result(StateCode.Ok, string.Empty, objectResult.Value);
            }
            else if (context.Result is EmptyResult emptyResult)
            {
                context.Result = new Result(StateCode.Ok, string.Empty);
            }
            else if (context.Result is JsonResult jsonResult)
            {
                context.Result = new Result(StateCode.Ok, string.Empty, jsonResult.Value);
            }
        }
    }
}
