﻿using System;
using System.Collections.Generic;
using System.Text;
using Bing.Applications.Dtos;

namespace Bing.Applications.Commands
{
    /// <summary>
    /// 命令工厂
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// 创建命令
        /// </summary>
        /// <typeparam name="TRequest">请求类型</typeparam>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="name">命令名称</param>
        /// <returns></returns>
        ICommand<TRequest,TResponse> Create<TRequest,TResponse>(string name) where TRequest:IRequest where TResponse:IResponse;
    }
}
