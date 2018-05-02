﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Bing.Domains.Entities;

namespace Bing.Datas.Stores.Operations
{
    /// <summary>
    /// 查找单个实体
    /// </summary>
    /// <typeparam name="TEntity">对象类型</typeparam>
    /// <typeparam name="TKey">对象标识类型</typeparam>
    public interface ISingle<TEntity,in TKey> where TEntity:class,IKey<TKey>
    {
        /// <summary>
        /// 查找单个实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);
    }
}
