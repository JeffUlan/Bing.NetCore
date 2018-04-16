﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Bing.Applications.Dtos;
using Bing.Datas.Queries;
using Bing.Domains.Repositories;

namespace Bing.Applications.Operations
{
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    public interface IPagerQueryAsync<TDto, in TQueryParameter>
        where TDto : IDto, new()
        where TQueryParameter : IQueryParameter
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="parameter">查询参数</param>
        /// <returns></returns>
        Task<List<TDto>> QueryAsync(TQueryParameter parameter);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="parameter">查询参数</param>
        /// <returns></returns>
        Task<PagerList<TDto>> PagerQueryAsync(TQueryParameter parameter);
    }
}
