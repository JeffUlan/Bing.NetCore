﻿using System;
using System.Collections.Generic;
using System.Text;
using Bing.Applications.Dtos;
using Bing.Applications.Operations;
using Bing.Datas.Queries;

namespace Bing.Applications
{
    /// <summary>
    /// 删除服务
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    public interface IDeleteService<TDto, in TQueryParameter> : IQueryService<TDto, TQueryParameter>, IDelete,IDeleteAsync 
        where TDto : IResponse, new()
        where TQueryParameter : IQueryParameter
    {
    }
}
