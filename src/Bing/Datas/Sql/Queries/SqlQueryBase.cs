﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bing.Datas.Queries;
using Bing.Datas.Sql.Queries.Builders.Abstractions;
using Bing.Datas.Sql.Queries.Builders.Conditions;
using Bing.Domains.Repositories;
using Bing.Utils;
using Bing.Utils.Extensions;
using Bing.Utils.Helpers;

namespace Bing.Datas.Sql.Queries
{
    /// <summary>
    /// Sql查询对象基类
    /// </summary>
    public abstract class SqlQueryBase:ISqlQuery
    {
        /// <summary>
        /// 数据库
        /// </summary>
        private readonly IDatabase _database;

        /// <summary>
        /// Sql生成器
        /// </summary>
        protected ISqlBuilder Builder { get; }

        /// <summary>
        /// 参数列表
        /// </summary>
        protected IDictionary<string, object> Params => Builder.GetParams();

        /// <summary>
        /// 初始化一个<see cref="SqlQueryBase"/>类型的实例
        /// </summary>
        /// <param name="sqlBuilder">Sql生成器</param>
        /// <param name="database">数据库</param>
        protected SqlQueryBase(ISqlBuilder sqlBuilder, IDatabase database = null)
        {
            Builder = sqlBuilder ?? throw new ArgumentNullException(nameof(sqlBuilder));
            _database = database;
        }

        /// <summary>
        /// 获取调试Sql语句
        /// </summary>
        /// <returns></returns>
        public string GetDebugSql()
        {
            return Builder.ToDebugSql();
        }

        /// <summary>
        /// 获取Sql语句
        /// </summary>
        /// <returns></returns>
        protected string GetSql()
        {
            return Builder.ToSql();
        }

        /// <summary>
        /// 创建Sql生成器
        /// </summary>
        /// <returns></returns>
        public virtual ISqlBuilder NewBuilder()
        {
            return Builder.New();
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public string ToString(IDbConnection connection = null)
        {
            return ToScalar(connection, GetSql(), Params).SafeString();
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public async Task<string> ToStringAsync(IDbConnection connection = null)
        {
            var result = await ToScalarAsync(connection, GetSql(), Params);
            return result.SafeString();
        }

        /// <summary>
        /// 获取整型
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public int ToInt(IDbConnection connection = null)
        {
            return Conv.ToInt(ToScalar(connection, GetSql(), Params));
        }

        /// <summary>
        /// 获取整型
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public async Task<int> ToIntAsync(IDbConnection connection = null)
        {
            return Conv.ToInt(await ToScalarAsync(connection, GetSql(), Params));
        }

        /// <summary>
        /// 获取可空整型
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public int? ToIntOrNull(IDbConnection connection = null)
        {
            return Conv.ToIntOrNull(ToScalar(connection, GetSql(), Params));
        }

        /// <summary>
        /// 获取可空整型
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public async Task<int?> ToIntOrNullAsync(IDbConnection connection = null)
        {
            return Conv.ToIntOrNull(await ToScalarAsync(connection, GetSql(), Params));
        }

        /// <summary>
        /// 获取单值
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        protected abstract object ToScalar(IDbConnection connection, string sql,
            IDictionary<string, object> parameters);

        /// <summary>
        /// 获取单值
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        protected abstract Task<object> ToScalarAsync(IDbConnection connection, string sql,
            IDictionary<string, object> parameters);

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public abstract TResult To<TResult>(IDbConnection connection = null);

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public abstract Task<TResult> ToAsync<TResult>(IDbConnection connection = null);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public abstract List<TResult> ToList<TResult>(IDbConnection connection = null);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public abstract Task<List<TResult>> ToListAsync<TResult>(IDbConnection connection = null);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="parameter">分页参数</param>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public abstract PagerList<TResult> ToPagerList<TResult>(IPager parameter, IDbConnection connection = null);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="parameter">分页参数</param>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public abstract Task<PagerList<TResult>> ToPagerListAsync<TResult>(IPager parameter, IDbConnection connection = null);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页显示行数</param>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public abstract PagerList<TResult> ToPagerList<TResult>(int page, int pageSize, IDbConnection connection = null);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <param name="page">页数</param>
        /// <param name="pageSize">每页显示行数</param>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public abstract Task<PagerList<TResult>> ToPagerListAsync<TResult>(int page, int pageSize, IDbConnection connection = null);

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        protected IDbConnection GetConnection(IDbConnection connection)
        {
            if (connection != null)
            {
                return connection;
            }
            connection = _database?.GetConnection();
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            return connection;
        }

        /// <summary>
        /// 设置列名
        /// </summary>
        /// <param name="columns">列名，范例：a,b.c as d</param>
        /// <param name="tableAlias">表别名</param>
        /// <returns></returns>
        public ISqlQuery Select(string columns, string tableAlias = null)
        {
            Builder.Select(columns, tableAlias);
            return this;
        }

        /// <summary>
        /// 设置列名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="columns">列名，范例：t => new object[] { t.A, t.B }</param>
        /// <returns></returns>
        public ISqlQuery Select<TEntity>(Expression<Func<TEntity, object[]>> columns) where TEntity : class
        {
            Builder.Select(columns);
            return this;
        }

        /// <summary>
        /// 设置列名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="column">列名，范例：t => t.A</param>
        /// <param name="columnAlias">列别名</param>
        /// <returns></returns>
        public ISqlQuery Select<TEntity>(Expression<Func<TEntity, object>> column, string columnAlias = null) where TEntity : class
        {
            Builder.Select(column, columnAlias);
            return this;
        }

        /// <summary>
        /// 添加到Select子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public ISqlQuery AppendSelect(string sql)
        {
            Builder.AppendSelect(sql);
            return this;
        }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public ISqlQuery From(string table, string alias = null)
        {
            Builder.From(table, alias);
            return this;
        }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="alias">别名</param>
        /// <param name="schema">架构名</param>
        /// <returns></returns>
        public ISqlQuery From<TEntity>(string alias = null, string schema = null) where TEntity : class
        {
            Builder.From<TEntity>(alias, schema);
            return this;
        }

        /// <summary>
        /// 添加到From子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public ISqlQuery AppendFrom(string sql)
        {
            Builder.AppendFrom(sql);
            return this;
        }

        /// <summary>
        /// 内连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public ISqlQuery Join(string table, string alias = null)
        {
            Builder.Join(table, alias);
            return this;
        }

        /// <summary>
        /// 内连接
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="alias">别名</param>
        /// <param name="schema">架构名</param>
        /// <returns></returns>
        public ISqlQuery Join<TEntity>(string alias = null, string schema = null) where TEntity : class
        {
            Builder.Join<TEntity>(alias, schema);
            return this;
        }

        /// <summary>
        /// 添加到内连接子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public ISqlQuery AppendJoin(string sql)
        {
            Builder.AppendJoin(sql);
            return this;
        }

        /// <summary>
        /// 左外连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public ISqlQuery LeftJoin(string table, string alias = null)
        {
            Builder.LeftJoin(table, alias);
            return this;
        }

        /// <summary>
        /// 左外连接
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="alias">别名</param>
        /// <param name="schema">架构名</param>
        /// <returns></returns>
        public ISqlQuery LeftJoin<TEntity>(string alias = null, string schema = null) where TEntity : class
        {
            Builder.LeftJoin<TEntity>(alias, schema);
            return this;
        }

        /// <summary>
        /// 添加到左外连接子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public ISqlQuery AppendLeftJoin(string sql)
        {
            Builder.AppendLeftJoin(sql);
            return this;
        }

        /// <summary>
        /// 右外连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public ISqlQuery RightJoin(string table, string alias = null)
        {
            Builder.RightJoin(table, alias);
            return this;
        }

        /// <summary>
        /// 右外连接
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="alias">别名</param>
        /// <param name="schema">架构名</param>
        /// <returns></returns>
        public ISqlQuery RightJoin<TEntity>(string alias = null, string schema = null) where TEntity : class
        {
            Builder.RightJoin<TEntity>(alias, schema);
            return this;
        }

        /// <summary>
        /// 添加到右外连接子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public ISqlQuery AppendRightJoin(string sql)
        {
            Builder.AppendRightJoin(sql);
            return this;
        }

        /// <summary>
        /// 设置连接条件
        /// </summary>
        /// <param name="left">左表列名</param>
        /// <param name="right">右表列名</param>
        /// <param name="operator">条件运算符</param>
        /// <returns></returns>
        public ISqlQuery On(string left, string right, Operator @operator = Operator.Equal)
        {
            Builder.On(left, right, @operator);
            return this;
        }

        /// <summary>
        /// 设置连接条件
        /// </summary>
        /// <typeparam name="TLeft">左表实体类型</typeparam>
        /// <typeparam name="TRight">右表实体类型</typeparam>
        /// <param name="left">左表列名</param>
        /// <param name="right">右表列名</param>
        /// <param name="operator">条件运算符</param>
        /// <returns></returns>
        public ISqlQuery On<TLeft, TRight>(Expression<Func<TLeft, object>> left, Expression<Func<TRight, object>> right, Operator @operator = Operator.Equal) where TLeft : class where TRight : class
        {
            Builder.On(left, right, @operator);
            return this;
        }

        /// <summary>
        /// 设置连接条件
        /// </summary>
        /// <typeparam name="TLeft">左表实体类型</typeparam>
        /// <typeparam name="TRight">右表实体类型</typeparam>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public ISqlQuery On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) where TLeft : class where TRight : class
        {
            Builder.On(expression);
            return this;
        }

        /// <summary>
        /// And连接条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ISqlQuery And(ICondition condition)
        {
            Builder.And(condition);
            return this;
        }

        /// <summary>
        /// Or连接条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ISqlQuery Or(ICondition condition)
        {
            Builder.Or(condition);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ISqlQuery Where(ICondition condition)
        {
            Builder.Where(condition);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        /// <returns></returns>
        public ISqlQuery Where(string column, object value, Operator @operator = Operator.Equal)
        {
            Builder.Where(column, value, @operator);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        /// <returns></returns>
        public ISqlQuery Where<TEntity>(Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal) where TEntity : class
        {
            Builder.Where(expression, value, @operator);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">查询条件表达式</param>
        /// <returns></returns>
        public ISqlQuery Where<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            Builder.Where(expression);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <param name="condition">拼接条件，该值为true时添加查询条件，否则忽略</param>
        /// <param name="operator">运算符</param>
        /// <returns></returns>
        public ISqlQuery WhereIf(string column, object value, bool condition, Operator @operator = Operator.Equal)
        {
            Builder.WhereIf(column, value, condition, @operator);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <param name="condition">拼接条件，该值为true时添加查询条件，否则忽略</param>
        /// <param name="operator">运算符</param>
        /// <returns></returns>
        public ISqlQuery WhereIf<TEntity>(Expression<Func<TEntity, object>> expression, object value, bool condition, Operator @operator = Operator.Equal) where TEntity : class
        {
            Builder.WhereIf(expression, value, condition, @operator);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">查询条件表达式</param>
        /// <param name="condition">拼接条件，该值为true时添加查询条件，否则忽略</param>
        /// <returns></returns>
        public ISqlQuery WhereIf<TEntity>(Expression<Func<TEntity, bool>> expression, bool condition) where TEntity : class
        {
            Builder.WhereIf(expression, condition);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值，如果该值为空，则忽略该查询条件</param>
        /// <param name="operator">运算符</param>
        /// <returns></returns>
        public ISqlQuery WhereIfNotEmpty(string column, object value, Operator @operator = Operator.Equal)
        {
            Builder.WhereIfNotEmpty(column, value, @operator);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值，如果该值为空，则忽略该查询条件</param>
        /// <param name="operator">运算符</param>
        /// <returns></returns>
        public ISqlQuery WhereIfNotEmpty<TEntity>(Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal) where TEntity : class
        {
            Builder.WhereIfNotEmpty(expression, value, @operator);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">查询条件表达式，如果参数值为空，则忽略该查询条件</param>
        /// <returns></returns>
        public ISqlQuery WhereIfNotEmpty<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            Builder.WhereIfNotEmpty(expression);
            return this;
        }

        /// <summary>
        /// 添加到Where子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public ISqlQuery AppendWhere(string sql)
        {
            Builder.AppendWhere(sql);
            return this;
        }

        /// <summary>
        /// 设置相等查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Equal(string column, object value)
        {
            Builder.Equal(column, value);
            return this;
        }

        /// <summary>
        /// 设置相等查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Equal<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            Builder.Equal(expression, value);
            return this;
        }

        /// <summary>
        /// 设置不相等查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery NotEqual(string column, object value)
        {
            Builder.NotEqual(column, value);
            return this;
        }

        /// <summary>
        /// 设置不相等查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery NotEqual<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            Builder.NotEqual(expression, value);
            return this;
        }

        /// <summary>
        /// 设置大于查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Greater(string column, object value)
        {
            Builder.Greater(column, value);
            return this;
        }

        /// <summary>
        /// 设置大于查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Greater<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            Builder.Greater(expression, value);
            return this;
        }

        /// <summary>
        /// 设置大于等于查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery GreaterEqual(string column, object value)
        {
            Builder.GreaterEqual(column, value);
            return this;
        }

        /// <summary>
        /// 设置大于等于查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery GreaterEqual<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            Builder.GreaterEqual(expression, value);
            return this;
        }

        /// <summary>
        /// 设置小于查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Less(string column, object value)
        {
            Builder.Less(column, value);
            return this;
        }

        /// <summary>
        /// 设置小于查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Less<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            Builder.Less(expression, value);
            return this;
        }

        /// <summary>
        /// 设置小于等于查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery LessEqual(string column, object value)
        {
            Builder.LessEqual(column, value);
            return this;
        }

        /// <summary>
        /// 设置小于等于查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery LessEqual<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            Builder.LessEqual(expression, value);
            return this;
        }

        /// <summary>
        /// 设置模糊匹配查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Contains(string column, object value)
        {
            Builder.Contains(column, value);
            return this;
        }

        /// <summary>
        /// 设置模糊匹配查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Contains<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            Builder.Contains(expression, value);
            return this;
        }

        /// <summary>
        /// 设置头匹配查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Starts(string column, object value)
        {
            Builder.Starts(column, value);
            return this;
        }

        /// <summary>
        /// 设置头匹配查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Starts<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            Builder.Starts(expression, value);
            return this;
        }

        /// <summary>
        /// 设置尾匹配查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Ends(string column, object value)
        {
            Builder.Ends(column, value);
            return this;
        }

        /// <summary>
        /// 设置尾匹配查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlQuery Ends<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            Builder.Ends(expression, value);
            return this;
        }

        /// <summary>
        /// 设置Is Null查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public ISqlQuery IsNull(string column)
        {
            Builder.IsNull(column);
            return this;
        }

        /// <summary>
        /// 设置Is Null查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <returns></returns>
        public ISqlQuery IsNull<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            Builder.IsNull(expression);
            return this;
        }

        /// <summary>
        /// 设置Is Not Null查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public ISqlQuery IsNotNull(string column)
        {
            Builder.IsNotNull(column);
            return this;
        }

        /// <summary>
        /// 设置Is Not Null查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <returns></returns>
        public ISqlQuery IsNotNull<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            Builder.IsNotNull(expression);
            return this;
        }

        /// <summary>
        /// 设置空条件，范例：[Name] Is Null Or [Name]=''
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public ISqlQuery IsEmpty(string column)
        {
            Builder.IsEmpty(column);
            return this;
        }

        /// <summary>
        /// 设置空条件，范例：[Name] Is Null Or [Name]=''
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <returns></returns>
        public ISqlQuery IsEmpty<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            Builder.IsEmpty(expression);
            return this;
        }

        /// <summary>
        /// 设置非空条件，范例：[Name] Is Null Or [Name]&lt;&gt;''
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public ISqlQuery IsNotEmpty(string column)
        {
            Builder.IsNotEmpty(column);
            return this;
        }

        /// <summary>
        /// 设置非空条件，范例：[Name] Is Null Or [Name]&lt;&gt;''
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <returns></returns>
        public ISqlQuery IsNotEmpty<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            Builder.IsNotEmpty(expression);
            return this;
        }

        /// <summary>
        /// 设置In条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        public ISqlQuery In(string column, IEnumerable<object> values)
        {
            Builder.In(column,values);
            return this;
        }

        /// <summary>
        /// 设置In条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        public ISqlQuery In<TEntity>(Expression<Func<TEntity, object>> expression, IEnumerable<object> values) where TEntity : class
        {
            Builder.In(expression, values);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between<TEntity>(Expression<Func<TEntity, object>> expression, int? min, int? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            Builder.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between<TEntity>(Expression<Func<TEntity, object>> expression, long? min, long? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            Builder.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between<TEntity>(Expression<Func<TEntity, object>> expression, float? min, float? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            Builder.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between<TEntity>(Expression<Func<TEntity, object>> expression, double? min, double? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            Builder.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between<TEntity>(Expression<Func<TEntity, object>> expression, decimal? min, decimal? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            Builder.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式，范例：t => t.Name</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="includeTime">是否包含时间</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between<TEntity>(Expression<Func<TEntity, object>> expression, DateTime? min, DateTime? max, bool includeTime = true,
            Boundary? boundary = null) where TEntity : class
        {
            Builder.Between(expression, min, max, includeTime, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between(string column, int? min, int? max, Boundary boundary = Boundary.Both)
        {
            Builder.Between(column, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between(string column, long? min, long? max, Boundary boundary = Boundary.Both)
        {
            Builder.Between(column, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between(string column, float? min, float? max, Boundary boundary = Boundary.Both)
        {
            Builder.Between(column, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between(string column, double? min, double? max, Boundary boundary = Boundary.Both)
        {
            Builder.Between(column, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between(string column, decimal? min, decimal? max, Boundary boundary = Boundary.Both)
        {
            Builder.Between(column, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="includeTime">是否包含时间</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlQuery Between(string column, DateTime? min, DateTime? max, bool includeTime = true, Boundary? boundary = null)
        {
            Builder.Between(column, min, max, includeTime, boundary);
            return this;
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="group">分组字段，范例：a.Id,b.Name</param>
        /// <param name="having">分组条件，范例：Count(*) > 1</param>
        /// <returns></returns>
        public ISqlQuery GroupBy(string @group, string having = null)
        {
            Builder.GroupBy(group, having);
            return this;
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="column">分组字段，范例：t => t.Name</param>
        /// <param name="having">分组条件，范例：Count(*) > 1</param>
        /// <returns></returns>
        public ISqlQuery GroupBy<TEntity>(Expression<Func<TEntity, object>> column, string having = null) where TEntity : class
        {
            Builder.GroupBy(column, having);
            return this;
        }

        /// <summary>
        /// 添加到GroupBy子句
        /// </summary>
        /// <param name="sql">Sql语句，说明：将会原样添加到Sql中，不会进行任何处理</param>
        /// <returns></returns>
        public ISqlQuery AppendGroupBy(string sql)
        {
            Builder.AppendGroupBy(sql);
            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="order">排序列表</param>
        /// <returns></returns>
        public ISqlQuery OrderBy(string order)
        {
            Builder.OrderBy(order);
            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="column">排序列</param>
        /// <param name="desc">是否倒序</param>
        /// <returns></returns>
        public ISqlQuery OrderBy<TEntity>(Expression<Func<TEntity, object>> column, bool desc = false)
        {
            Builder.OrderBy(column, desc);
            return this;
        }

        /// <summary>
        /// 添加到OrderBy子句
        /// </summary>
        /// <param name="sql">排序列表，说明：将会原样添加到Sql中，不会进行任何处理</param>
        /// <returns></returns>
        public ISqlQuery AppendOrderBy(string sql)
        {
            Builder.AppendOrderBy(sql);
            return this;
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        protected virtual int GetCount(IDbConnection connection)
        {
            return Conv.ToInt(ToScalar(connection, GetCountSql(), Params));
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        protected virtual async Task<int> GetCountAsync(IDbConnection connection)
        {
            return Conv.ToInt(await ToScalarAsync(connection, GetCountSql(), Params));
        }

        /// <summary>
        /// 获取行数Sql
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCountSql()
        {
            var result = new StringBuilder();
            result.AppendLine("Select Count(*) ");
            AppendSql(result,Builder.GetFrom());
            AppendSql(result, Builder.GetJoin());
            AppendSql(result,Builder.GetWhere());
            return result.ToString().Trim();
        }

        /// <summary>
        /// 添加Sql
        /// </summary>
        /// <param name="result">Sql拼接</param>
        /// <param name="sql">Sql语句</param>
        protected void AppendSql(StringBuilder result, string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return;
            }
            result.AppendLine($"{sql} ");
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="parameters">参数</param>
        protected abstract void WriteTraceLog(string sql, IDictionary<string, object> parameters);
    }
}
