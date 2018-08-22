﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bing.Domains.Entities;

namespace Bing.Datas.Stores.Operations
{
    /// <summary>
    /// 查找单个实体
    /// </summary>
    /// <typeparam name="TEntity">对象类型</typeparam>
    /// <typeparam name="TKey">对象标识类型</typeparam>
    public interface ISingleAsync<TEntity, in TKey> where TEntity : class, IKey<TKey>
    {
        /// <summary>
        /// 查找单个实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
