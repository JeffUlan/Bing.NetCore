﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bing.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bing.Webs.Commons
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result:JsonResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { get; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; }

        /// <summary>
        /// 初始化一个<see cref="Result"/>类型的实例
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public Result(int code, string message, dynamic data = null):base(null)
        {
            Code = code;
            Message = message;
            Data = data;
            OperationTime = DateTime.Now;
        }

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public Result(StateCode code, string message, dynamic data = null) : base(null)
        {
            Code = code.Value();
            Message = message;
            Data = data;
            OperationTime = DateTime.Now;
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (Data != null)
            {
                this.Value = new
                {
                    Code = Code,
                    Message = Message,
                    OperationTime = OperationTime,
                    Data = Data
                };
            }
            else
            {
                this.Value = new
                {
                    Code = Code,
                    Message = Message,
                    OperationTime = OperationTime,
                };
            }            
            return base.ExecuteResultAsync(context);
        }
    }
}
