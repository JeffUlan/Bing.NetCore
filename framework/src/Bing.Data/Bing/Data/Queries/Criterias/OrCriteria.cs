﻿using System;
using System.Linq.Expressions;
using Bing.Expressions;

namespace Bing.Data.Queries.Criterias
{
    /// <summary>
    /// 或查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class OrCriteria<TEntity> : ICriteria<TEntity> where TEntity : class
    {
        /// <summary>
        /// 初始化一个<see cref="OrCriteria{TEntity}"/>类型的实例
        /// </summary>
        /// <param name="left">查询条件1</param>
        /// <param name="right">查询条件2</param>
        public OrCriteria(Expression<Func<TEntity, bool>> left, Expression<Func<TEntity, bool>> right) => Predicate = left.Or(right);

        /// <summary>
        /// 查询条件
        /// </summary>
        protected Expression<Func<TEntity, bool>> Predicate { get; set; }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        public virtual Expression<Func<TEntity, bool>> GetPredicate() => Predicate;
    }
}
