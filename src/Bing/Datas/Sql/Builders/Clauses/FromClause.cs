﻿using System;
using Bing.Datas.Sql.Builders.Core;
using Bing.Datas.Sql.Builders.Extensions;
using Bing.Properties;

namespace Bing.Datas.Sql.Builders.Clauses
{
    /// <summary>
    /// From子句
    /// </summary>
    public class FromClause:IFromClause
    {
        /// <summary>
        /// Sql项
        /// </summary>
        protected SqlItem Table;

        /// <summary>
        /// Sql方言
        /// </summary>
        protected readonly IDialect Dialect;

        /// <summary>
        /// 实体解析器
        /// </summary>
        protected readonly IEntityResolver Resolver;

        /// <summary>
        /// 实体别名注册器
        /// </summary>
        protected readonly IEntityAliasRegister Register;

        /// <summary>
        /// Sql生成器
        /// </summary>
        protected readonly ISqlBuilder Builder;

        /// <summary>
        /// 初始化一个<see cref="FromClause"/>类型的实例
        /// </summary>
        /// <param name="builder">Sql生成器</param>
        /// <param name="dialect">Sql方言</param>
        /// <param name="resolver">实体解析器</param>
        /// <param name="register">实体别名注册器</param>
        /// <param name="table">表</param>
        public FromClause(ISqlBuilder builder,IDialect dialect, IEntityResolver resolver, IEntityAliasRegister register, SqlItem table = null)
        {
            Builder = builder;
            Dialect = dialect;
            Resolver = resolver;
            Register = register;
            Table = table;
        }        

        /// <summary>
        /// 克隆
        /// </summary>
        /// <param name="builder">Sql生成器</param>
        /// <param name="register">实体别名注册器</param>
        /// <returns></returns>
        public virtual IFromClause Clone(ISqlBuilder builder, IEntityAliasRegister register)
        {
            return new FromClause(builder, Dialect, Resolver, register, Table);
        }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        public void From(string table, string alias = null)
        {
            Table = CreateSqlItem(table, null, alias);
        }

        /// <summary>
        /// 创建Sql项
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="schema">架构名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        protected virtual SqlItem CreateSqlItem(string table, string schema, string alias)
        {
            return new SqlItem(table, schema, alias);
        }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="alias">别名</param>
        /// <param name="schema">架构名</param>
        public void From<TEntity>(string alias = null, string schema = null) where TEntity : class
        {
            var entity = typeof(TEntity);
            var table = Resolver.GetTableAndSchema(entity);
            Table = CreateSqlItem(table, schema, alias);
            Register.Register(entity, Resolver.GetAlias(entity, alias));
        }

        /// <summary>
        /// 设置子查询表
        /// </summary>
        /// <param name="builder">Sql生成器</param>
        /// <param name="alias">表别名</param>
        public void From(ISqlBuilder builder, string alias)
        {
            if (builder == null)
            {
                return;
            }

            var result = builder.ToSql();
            if (string.IsNullOrWhiteSpace(alias) == false)
            {
                result = $"({result}) As {Dialect.SafeName(alias)}";
            }
            AppendSql(result);
        }

        /// <summary>
        /// 设置子查询表
        /// </summary>
        /// <param name="action">子查询操作</param>
        /// <param name="alias">表别名</param>
        public void From(Action<ISqlBuilder> action, string alias)
        {
            if (action == null)
            {
                return;
            }

            var builder = Builder.New();
            action(builder);
            From(builder, alias);
        }

        /// <summary>
        /// 添加到From子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        public void AppendSql(string sql)
        {
            if (Table != null && Table.Raw)
            {
                Table = new SqlItem($"{Table.Name}{sql}", raw: true);
                return;
            }
            Table = new SqlItem(sql, raw: true);
        }

        /// <summary>
        /// 验证
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Table?.Name))
            {
                throw new InvalidOperationException(LibraryResource.TableIsEmpty);
            }
        }

        /// <summary>
        /// 输出Sql
        /// </summary>
        /// <returns></returns>
        public string ToSql()
        {
            var table = Table?.ToSql(Dialect);
            if (string.IsNullOrWhiteSpace(table))
            {
                return null;
            }
            return $"From {table}";
        }
    }
}
