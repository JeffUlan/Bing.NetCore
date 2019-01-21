﻿using System;

namespace Bing.Datas.Sql.Queries.Builders.Core
{
    // Join(设置连接)
    public partial class SqlBuilderBase
    {
        /// <summary>
        /// 内连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder Join(string table, string alias = null)
        {
            JoinClause.Join(table, alias);
            return this;
        }

        /// <summary>
        /// 内连接
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="alias">别名</param>
        /// <param name="schema">架构名</param>
        /// <returns></returns>
        public virtual ISqlBuilder Join<TEntity>(string alias = null, string schema = null) where TEntity : class
        {
            JoinClause.Join<TEntity>(alias, schema);
            return this;
        }

        /// <summary>
        /// 添加到内连接子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendJoin(string sql)
        {
            JoinClause.AppendJoin(sql);
            return this;
        }

        /// <summary>
        /// 添加到内连接子句
        /// </summary>
        /// <param name="builder">Sql生成器</param>
        /// <param name="alias">表别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendJoin(ISqlBuilder builder, string alias)
        {
            JoinClause.AppendJoin(builder, alias);
            return this;
        }

        /// <summary>
        /// 添加到内连接子句
        /// </summary>
        /// <param name="action">子查询操作</param>
        /// <param name="alias">表别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendJoin(Action<ISqlBuilder> action, string alias)
        {
            JoinClause.AppendJoin(action, alias);
            return this;
        }

        /// <summary>
        /// 左外连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder LeftJoin(string table, string alias = null)
        {
            JoinClause.LeftJoin(table, alias);
            return this;
        }

        /// <summary>
        /// 左外连接
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="alias">别名</param>
        /// <param name="schema">架构名</param>
        /// <returns></returns>
        public virtual ISqlBuilder LeftJoin<TEntity>(string alias = null, string schema = null) where TEntity : class
        {
            JoinClause.LeftJoin<TEntity>(alias, schema);
            return this;
        }

        /// <summary>
        /// 添加到左外连接子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendLeftJoin(string sql)
        {
            JoinClause.AppendLeftJoin(sql);
            return this;
        }

        /// <summary>
        /// 添加到左外连接子句
        /// </summary>
        /// <param name="builder">Sql生成器</param>
        /// <param name="alias">表别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendLeftJoin(ISqlBuilder builder, string alias)
        {
            JoinClause.AppendLeftJoin(builder, alias);
            return this;
        }

        /// <summary>
        /// 添加到左外连接子句
        /// </summary>
        /// <param name="action">子查询操作</param>
        /// <param name="alias">表别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendLeftJoin(Action<ISqlBuilder> action, string alias)
        {
            JoinClause.AppendLeftJoin(action, alias);
            return this;
        }

        /// <summary>
        /// 右外连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder RightJoin(string table, string alias = null)
        {
            JoinClause.RightJoin(table, alias);
            return this;
        }

        /// <summary>
        /// 右外连接
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="alias">别名</param>
        /// <param name="schema">架构名</param>
        /// <returns></returns>
        public virtual ISqlBuilder RightJoin<TEntity>(string alias = null, string schema = null) where TEntity : class
        {
            JoinClause.RightJoin<TEntity>(alias, schema);
            return this;
        }

        /// <summary>
        /// 添加到右外连接子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendRightJoin(string sql)
        {
            JoinClause.AppendRightJoin(sql);
            return this;
        }

        /// <summary>
        /// 添加到右外连接子句
        /// </summary>
        /// <param name="builder">Sql生成器</param>
        /// <param name="alias">表别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendRightJoin(ISqlBuilder builder, string alias)
        {
            JoinClause.AppendRightJoin(builder, alias);
            return this;
        }

        /// <summary>
        /// 添加到右外连接子句
        /// </summary>
        /// <param name="action">子查询操作</param>
        /// <param name="alias">表别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendRightJoin(Action<ISqlBuilder> action, string alias)
        {
            JoinClause.AppendRightJoin(action, alias);
            return this;
        }
    }
}
