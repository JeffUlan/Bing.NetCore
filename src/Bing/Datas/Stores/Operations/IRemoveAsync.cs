﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bing.Domains.Entities;

namespace Bing.Datas.Stores.Operations
{
    /// <summary>
    /// 移除实体
    /// </summary>
    /// <typeparam name="TEntity">对象类型</typeparam>
    /// <typeparam name="TKey">对象标识类型</typeparam>
    public interface IRemoveAsync<in TEntity, in TKey> where TEntity : class, IKey<TKey>, IVersion
    {
        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        Task RemoveAsync(object id);

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// 移除实体集合
        /// </summary>
        /// <param name="ids">标识集合</param>
        /// <returns></returns>
        Task RemoveAsync(IEnumerable<TKey> ids);

        /// <summary>
        /// 移除实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        Task RemoveAsync(IEnumerable<TEntity> entities);
    }
}
