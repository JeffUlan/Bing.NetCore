﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Bing.Datas.Matedatas;
using Bing.Datas.Queries;
using Bing.Datas.Sql.Queries.Builders.Abstractions;
using Bing.Datas.Sql.Queries.Builders.Clauses;
using Bing.Datas.Sql.Queries.Builders.Conditions;
using Bing.Domains.Repositories;
using Bing.Utils;

namespace Bing.Datas.Sql.Queries.Builders.Core
{
    /// <summary>
    /// Sql生成器基类
    /// </summary>
    public abstract class SqlBuilderBase:ISqlBuilder
    {
        #region 属性

        /// <summary>
        /// 实体元数据解析器
        /// </summary>
        protected IEntityMatedata EntityMatedata { get; }

        /// <summary>
        /// 实体解析器
        /// </summary>
        protected IEntityResolver EntityResolver { get; }

        /// <summary>
        /// 实体别名注册器
        /// </summary>
        protected IEntityAliasRegister AliasRegister { get; }

        /// <summary>
        /// 参数管理器
        /// </summary>
        private IParameterManager _parameterManager;

        /// <summary>
        /// 参数管理器
        /// </summary>
        protected IParameterManager ParameterManager =>
            _parameterManager ?? (_parameterManager = CreateParameterManager());

        /// <summary>
        /// 创建参数管理器
        /// </summary>
        /// <returns></returns>
        protected virtual IParameterManager CreateParameterManager()
        {
            return new ParameterManager(GetDialect());
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化一个<see cref="SqlBuilderBase"/>类型的实例
        /// </summary>
        /// <param name="matedata">实体元数据解析器</param>
        /// <param name="parameterManager">参数管理器</param>
        protected SqlBuilderBase(IEntityMatedata matedata = null, IParameterManager parameterManager = null)
        {
            EntityMatedata = matedata;
            EntityResolver = new EntityResolver(matedata);
            AliasRegister = new EntityAliasRegister();
            _parameterManager = parameterManager;
        }

        #endregion

        #region New(创建Sql生成器)

        /// <summary>
        /// 创建Sql生成器
        /// </summary>
        /// <returns></returns>
        public abstract ISqlBuilder New();

        #endregion

        #region ToDebugSql(生成调试Sql语句)

        /// <summary>
        /// 生成调试Sql语句，Sql语句中的参数被替换为参数值
        /// </summary>
        /// <returns></returns>
        public string ToDebugSql()
        {
            var result = ToSql();
            var parameters = GetParams();
            foreach (var parameter in parameters)
            {
                result = result.Replace(parameter.Key, SqlHelper.GetParamLiterals(parameter.Value));
            }
            return result;
        }

        #endregion

        #region ToSql(生成Sql)

        /// <summary>
        /// 生成Sql语句
        /// </summary>
        /// <returns></returns>
        public string ToSql()
        {
            Validate();
            var result=new StringBuilder();
            CreateSql(result);
            return result.ToString().Trim();
        }

        /// <summary>
        /// 验证
        /// </summary>
        public void Validate()
        {
            FromClause.Validate();
        }

        /// <summary>
        /// 创建Sql语句
        /// </summary>
        /// <param name="result"></param>
        protected virtual void CreateSql(StringBuilder result)
        {
            if (_pager == null)
            {
                CreateNoPagerSql(result);
                return;
            }
            CreatePagerSql(result);
        }

        /// <summary>
        /// 创建不分页Sql
        /// </summary>
        /// <param name="result">Sql拼接器</param>
        protected virtual void CreateNoPagerSql(StringBuilder result)
        {
            AppendSql(result, GetSelect());
            AppendSql(result, GetFrom());
            AppendSql(result, GetJoin());
            AppendSql(result, GetWhere());
            AppendSql(result, GetGroupBy());
            AppendSql(result, GetOrderBy());
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
            result.AppendLine($"{sql}");
        }

        /// <summary>
        /// 创建分页Sql
        /// </summary>
        /// <param name="result">Sql拼接</param>
        protected abstract void CreatePagerSql(StringBuilder result);

        #endregion

        #region Select(设置列名)

        /// <summary>
        /// Select子句
        /// </summary>
        private ISelectClause _selectClause;

        /// <summary>
        /// Select子句
        /// </summary>
        protected ISelectClause SelectClause => _selectClause ?? (_selectClause = CreateSelectClause());

        /// <summary>
        /// 创建Select子句
        /// </summary>
        /// <returns></returns>
        protected virtual ISelectClause CreateSelectClause()
        {
            return new SelectClause(GetDialect(), EntityResolver, AliasRegister);
        }

        /// <summary>
        /// 获取Sql方言
        /// </summary>
        /// <returns></returns>
        protected abstract IDialect GetDialect();

        /// <summary>
        /// 获取Select语句
        /// </summary>
        /// <returns></returns>
        public virtual string GetSelect()
        {
            return SelectClause.ToSql();
        }

        /// <summary>
        /// 设置列名
        /// </summary>
        /// <param name="columns">列名</param>
        /// <param name="tableAlias">表别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder Select(string columns, string tableAlias = null)
        {
            SelectClause.Select(columns, tableAlias);
            return this;
        }

        /// <summary>
        /// 设置列名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="columns">列名</param>
        /// <returns></returns>
        public virtual ISqlBuilder Select<TEntity>(Expression<Func<TEntity, object[]>> columns) where TEntity : class
        {
            SelectClause.Select(columns);
            return this;
        }

        /// <summary>
        /// 设置列名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="column">列名</param>
        /// <param name="columnAlias">列别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder Select<TEntity>(Expression<Func<TEntity, object>> column, string columnAlias = null) where TEntity : class
        {
            SelectClause.Select(column,columnAlias);
            return this;
        }

        /// <summary>
        /// 添加到Select子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendSelect(string sql)
        {
            SelectClause.AppendSql(sql);
            return this;
        }

        #endregion

        #region From(设置表名)

        /// <summary>
        /// From子句
        /// </summary>
        private IFromClause _fromClause;

        /// <summary>
        /// From子句
        /// </summary>
        protected IFromClause FromClause => _fromClause ?? (_fromClause = CreateFromClause());

        /// <summary>
        /// 创建From子句
        /// </summary>
        /// <returns></returns>
        protected virtual IFromClause CreateFromClause()
        {
            return new FromClause(GetDialect(), EntityResolver, AliasRegister);
        }

        /// <summary>
        /// 获取From语句
        /// </summary>
        /// <returns></returns>
        public virtual string GetFrom()
        {
            return FromClause.ToSql();
        }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder From(string table, string alias = null)
        {
            FromClause.From(table,alias);
            return this;
        }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="alias">别名</param>
        /// <param name="schema">架构名</param>
        /// <returns></returns>
        public virtual ISqlBuilder From<TEntity>(string alias = null, string schema = null) where TEntity : class
        {
            FromClause.From<TEntity>(alias,schema);
            return this;
        }

        /// <summary>
        /// 添加到From子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendFrom(string sql)
        {
            FromClause.AppendSql(sql);
            return this;
        }

        #endregion

        #region Join(设置连接)

        /// <summary>
        /// Join子句
        /// </summary>
        private IJoinClause _joinClause;

        /// <summary>
        /// Join子句
        /// </summary>
        protected IJoinClause JoinClause => _joinClause ?? (_joinClause = CreateJoinClause());

        /// <summary>
        /// 创建Join子句
        /// </summary>
        /// <returns></returns>
        protected virtual IJoinClause CreateJoinClause()
        {
            return new JoinClause(GetDialect(),EntityResolver,AliasRegister);
        }

        /// <summary>
        /// 获取Join语句
        /// </summary>
        /// <returns></returns>
        public virtual string GetJoin()
        {
            return JoinClause.ToSql();
        }

        /// <summary>
        /// 内连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder Join(string table, string alias = null)
        {
            JoinClause.Join(table,alias);
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
            JoinClause.Join<TEntity>(alias,schema);
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
        /// 左外连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder LeftJoin(string table, string alias = null)
        {
            JoinClause.LeftJoin(table,alias);
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
            JoinClause.LeftJoin<TEntity>(alias,schema);
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
        /// 右外连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public virtual ISqlBuilder RightJoin(string table, string alias = null)
        {
            JoinClause.RightJoin(table,alias);
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
            JoinClause.RightJoin<TEntity>(alias,schema);
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
        /// 设置连接条件
        /// </summary>
        /// <param name="left">左表列名</param>
        /// <param name="right">右表列名</param>
        /// <param name="operator">条件运算符</param>
        /// <returns></returns>
        public virtual ISqlBuilder On(string left, string right, Operator @operator = Operator.Equal)
        {
            JoinClause.On(left,right,@operator);
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
        public virtual ISqlBuilder On<TLeft, TRight>(Expression<Func<TLeft, object>> left, Expression<Func<TRight, object>> right, Operator @operator = Operator.Equal) where TLeft : class where TRight : class
        {
            JoinClause.On(left,right,@operator);
            return this;
        }

        /// <summary>
        /// 设置连接条件
        /// </summary>
        /// <typeparam name="TLeft">左表实体类型</typeparam>
        /// <typeparam name="TRight">右表实体类型</typeparam>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public virtual ISqlBuilder On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) where TLeft : class where TRight : class
        {
            JoinClause.On(expression);
            return this;
        }

        #endregion

        #region Where(设置查询条件)

        /// <summary>
        /// Where子句
        /// </summary>
        private IWhereClause _whereClause;

        /// <summary>
        /// Where子句
        /// </summary>
        protected IWhereClause WhereClause => _whereClause ?? (_whereClause = CreateWhereClause());

        /// <summary>
        /// 创建Where子句
        /// </summary>
        /// <returns></returns>
        protected virtual IWhereClause CreateWhereClause()
        {
            return new WhereClause(GetDialect(), EntityResolver, AliasRegister, ParameterManager);
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        public string GetCondition()
        {
            return WhereClause.GetCondition();
        }

        /// <summary>
        /// 获取Where语句
        /// </summary>
        /// <returns></returns>
        public virtual string GetWhere()
        {
            return WhereClause.ToSql();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetParams()
        {
            return ParameterManager.GetParams();
        }

        /// <summary>
        /// And连接条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ISqlBuilder And(ICondition condition)
        {
            WhereClause.And(condition);
            return this;
        }

        /// <summary>
        /// Or连接条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ISqlBuilder Or(ICondition condition)
        {
            WhereClause.Or(condition);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ISqlBuilder Where(ICondition condition)
        {
            WhereClause.Where(condition);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        /// <returns></returns>
        public ISqlBuilder Where(string column, object value, Operator @operator = Operator.Equal)
        {
            WhereClause.Where(column,value,@operator);
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
        public ISqlBuilder Where<TEntity>(Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal) where TEntity : class
        {
            WhereClause.Where(expression,value,@operator);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">查询条件表达式</param>
        /// <returns></returns>
        public ISqlBuilder Where<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            WhereClause.Where(expression);
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
        public ISqlBuilder WhereIf(string column, object value, bool condition, Operator @operator = Operator.Equal)
        {
            WhereClause.WhereIf(column,value,condition,@operator);
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
        public ISqlBuilder WhereIf<TEntity>(Expression<Func<TEntity, object>> expression, object value, bool condition, Operator @operator = Operator.Equal) where TEntity : class
        {
            WhereClause.WhereIf(expression, value, condition, @operator);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">查询条件表达式</param>
        /// <param name="condition">拼接条件，该值为true时添加查询条件，否则忽略</param>
        /// <returns></returns>
        public ISqlBuilder WhereIf<TEntity>(Expression<Func<TEntity, bool>> expression, bool condition) where TEntity : class
        {
            WhereClause.WhereIf(expression,condition);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值，如果该值为空，则忽略该查询条件</param>
        /// <param name="operator">运算符</param>
        /// <returns></returns>
        public ISqlBuilder WhereIfNotEmpty(string column, object value, Operator @operator = Operator.Equal)
        {
            WhereClause.WhereIfNotEmpty(column,value,@operator);
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
        public ISqlBuilder WhereIfNotEmpty<TEntity>(Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal) where TEntity : class
        {
            WhereClause.WhereIfNotEmpty(expression,value,@operator);
            return this;
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">查询条件表达式，如果参数值为空，则忽略该查询条件</param>
        /// <returns></returns>
        public ISqlBuilder WhereIfNotEmpty<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            WhereClause.WhereIfNotEmpty(expression);
            return this;
        }

        /// <summary>
        /// 添加到Where子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public ISqlBuilder AppendWhere(string sql)
        {
            WhereClause.AppendSql(sql);
            return this;
        }

        /// <summary>
        /// 设置相等查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Equal(string column, object value)
        {
            return Where(column, value);
        }

        /// <summary>
        /// 设置相等查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Equal<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            return Where(expression, value);
        }

        /// <summary>
        /// 设置不相等查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder NotEqual(string column, object value)
        {
            return Where(column, value, Operator.NotEqual);
        }

        /// <summary>
        /// 设置不相等查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder NotEqual<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            return Where(expression, value, Operator.NotEqual);
        }

        /// <summary>
        /// 设置大于查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Greater(string column, object value)
        {
            return Where(column, value, Operator.Greater);
        }

        /// <summary>
        /// 设置大于查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Greater<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            return Where(expression, value, Operator.Greater);
        }

        /// <summary>
        /// 设置大于等于查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder GreaterEqual(string column, object value)
        {
            return Where(column, value, Operator.GreaterEqual);
        }

        /// <summary>
        /// 设置大于等于查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder GreaterEqual<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            return Where(expression, value, Operator.GreaterEqual);
        }

        /// <summary>
        /// 设置小于查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Less(string column, object value)
        {
            return Where(column, value, Operator.Less);
        }

        /// <summary>
        /// 设置小于查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Less<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            return Where(expression, value, Operator.Less);
        }

        /// <summary>
        /// 设置小于等于查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder LessEqual(string column, object value)
        {
            return Where(column, value, Operator.LessEqual); 
        }

        /// <summary>
        /// 设置小于等于查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder LessEqual<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            return Where(expression, value, Operator.LessEqual);
        }

        /// <summary>
        /// 设置模糊匹配查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Contains(string column, object value)
        {
            return Where(column, value, Operator.Contains);
        }

        /// <summary>
        /// 设置模糊匹配查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Contains<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            return Where(expression, value, Operator.Contains);
        }

        /// <summary>
        /// 设置头匹配查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Starts(string column, object value)
        {
            return Where(column, value, Operator.Starts);
        }

        /// <summary>
        /// 设置头匹配查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Starts<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            return Where(expression, value, Operator.Starts);
        }

        /// <summary>
        /// 设置尾匹配查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Ends(string column, object value)
        {
            return Where(column, value, Operator.Ends);
        }

        /// <summary>
        /// 设置尾匹配查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public ISqlBuilder Ends<TEntity>(Expression<Func<TEntity, object>> expression, object value) where TEntity : class
        {
            return Where(expression, value, Operator.Ends);
        }

        /// <summary>
        /// 设置Is Null查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public ISqlBuilder IsNull(string column)
        {
            WhereClause.IsNull(column);
            return this;
        }

        /// <summary>
        /// 设置Is Null查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <returns></returns>
        public ISqlBuilder IsNull<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            WhereClause.IsNull(expression);
            return this; 
        }

        /// <summary>
        /// 设置Is Not Null查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public ISqlBuilder IsNotNull(string column)
        {
            WhereClause.IsNotNull(column);
            return this;
        }

        /// <summary>
        /// 设置Is Not Null查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <returns></returns>
        public ISqlBuilder IsNotNull<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            WhereClause.IsNotNull(expression);
            return this;
        }

        /// <summary>
        /// 设置空条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public ISqlBuilder IsEmpty(string column)
        {
            WhereClause.IsEmpty(column);
            return this;
        }

        /// <summary>
        /// 设置空条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <returns></returns>
        public ISqlBuilder IsEmpty<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            WhereClause.IsEmpty(expression);
            return this;
        }

        /// <summary>
        /// 设置非空条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public ISqlBuilder IsNotEmpty(string column)
        {
            WhereClause.IsNotEmpty(column);
            return this;
        }

        /// <summary>
        /// 设置非空条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <returns></returns>
        public ISqlBuilder IsNotEmpty<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            WhereClause.IsNotEmpty(expression);
            return this;
        }

        /// <summary>
        /// 设置In条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        public ISqlBuilder In(string column, IEnumerable<object> values)
        {
            WhereClause.In(column,values);
            return this;
        }

        /// <summary>
        /// 设置In条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        public ISqlBuilder In<TEntity>(Expression<Func<TEntity, object>> expression, IEnumerable<object> values) where TEntity : class
        {
            WhereClause.In(expression,values);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlBuilder Between<TEntity>(Expression<Func<TEntity, object>> expression, int? min, int? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            WhereClause.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlBuilder Between<TEntity>(Expression<Func<TEntity, object>> expression, long? min, long? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            WhereClause.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlBuilder Between<TEntity>(Expression<Func<TEntity, object>> expression, float? min, float? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            WhereClause.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlBuilder Between<TEntity>(Expression<Func<TEntity, object>> expression, double? min, double? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            WhereClause.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlBuilder Between<TEntity>(Expression<Func<TEntity, object>> expression, decimal? min, decimal? max, Boundary boundary = Boundary.Both) where TEntity : class
        {
            WhereClause.Between(expression, min, max, boundary);
            return this;
        }

        /// <summary>
        /// 设置范围查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="includeTime">是否包含时间</param>
        /// <param name="boundary">包含边界</param>
        /// <returns></returns>
        public ISqlBuilder Between<TEntity>(Expression<Func<TEntity, object>> expression, DateTime? min, DateTime? max, bool includeTime = true,
            Boundary? boundary = null) where TEntity : class
        {
            WhereClause.Between(expression, min, max,includeTime, boundary);
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
        public ISqlBuilder Between(string column, int? min, int? max, Boundary boundary = Boundary.Both)
        {
            WhereClause.Between(column, min, max, boundary);
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
        public ISqlBuilder Between(string column, long? min, long? max, Boundary boundary = Boundary.Both)
        {
            WhereClause.Between(column, min, max, boundary);
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
        public ISqlBuilder Between(string column, float? min, float? max, Boundary boundary = Boundary.Both)
        {
            WhereClause.Between(column, min, max, boundary);
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
        public ISqlBuilder Between(string column, double? min, double? max, Boundary boundary = Boundary.Both)
        {
            WhereClause.Between(column, min, max, boundary);
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
        public ISqlBuilder Between(string column, decimal? min, decimal? max, Boundary boundary = Boundary.Both)
        {
            WhereClause.Between(column, min, max, boundary);
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
        public ISqlBuilder Between(string column, DateTime? min, DateTime? max, bool includeTime = true,
            Boundary? boundary = null)
        {
            WhereClause.Between(column, min, max, includeTime, boundary);
            return this;
        }

        #endregion

        #region GroupBy(分组)

        /// <summary>
        /// 分组字句
        /// </summary>
        private IGroupByClause _groupByClause;

        protected IGroupByClause GroupByClause => _groupByClause ?? (_groupByClause = CreateGroupByClause());

        /// <summary>
        /// 创建分组子句
        /// </summary>
        /// <returns></returns>
        protected virtual IGroupByClause CreateGroupByClause()
        {
            return new GroupByClause(GetDialect(), EntityResolver, AliasRegister);
        }

        /// <summary>
        /// 获取分组语句
        /// </summary>
        /// <returns></returns>
        public virtual string GetGroupBy()
        {
            return GroupByClause.ToSql();
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="group">分组字段</param>
        /// <param name="having">分组条件</param>
        /// <returns></returns>
        public ISqlBuilder GroupBy(string @group, string having = null)
        {
            GroupByClause.GroupBy(group,having);
            return this;
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="column">分组字段</param>
        /// <param name="having">分组条件</param>
        /// <returns></returns>
        public ISqlBuilder GroupBy<TEntity>(Expression<Func<TEntity, object>> column, string having = null) where TEntity : class
        {
            GroupByClause.GroupBy(column,having);
            return this;
        }

        /// <summary>
        /// 添加到GroupBy子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public ISqlBuilder AppendGroupBy(string sql)
        {
            GroupByClause.AppendSql(sql);
            return this;
        }

        #endregion

        #region OrderBy(设置排序)

        /// <summary>
        /// 排序子句
        /// </summary>
        private IOrderByClause _orderByClause;

        /// <summary>
        /// 排序子句
        /// </summary>
        protected IOrderByClause OrderByClause => _orderByClause ?? (_orderByClause = CreateOrderByClause());

        /// <summary>
        /// 创建排序子句
        /// </summary>
        /// <returns></returns>
        protected virtual IOrderByClause CreateOrderByClause()
        {
            return new OrderByClause(GetDialect(),EntityResolver,AliasRegister);
        }

        /// <summary>
        /// 获取排序语句
        /// </summary>
        /// <returns></returns>
        public virtual string GetOrderBy()
        {
            return OrderByClause.ToSql();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="order">排序列表</param>
        /// <returns></returns>
        public virtual ISqlBuilder OrderBy(string order)
        {
            OrderByClause.OrderBy(order);
            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="column">排序列</param>
        /// <param name="desc">是否降序</param>
        /// <returns></returns>
        public virtual ISqlBuilder OrderBy<TEntity>(Expression<Func<TEntity, object>> column, bool desc = false)
        {
            OrderByClause.OrderBy(column, desc);
            return this;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="order">排序列表</param>
        /// <returns></returns>
        public virtual ISqlBuilder AppendOrderBy(string order)
        {
            OrderByClause.AppendSql(order);
            return this;
        }

        #endregion

        #region Pager(设置分页)

        /// <summary>
        /// 分页
        /// </summary>
        private IPager _pager;

        /// <summary>
        /// 获取分页参数
        /// </summary>
        /// <returns></returns>
        protected IPager GetPager()
        {
            return _pager;
        }

        /// <summary>
        /// 设置分页
        /// </summary>
        /// <param name="pager">分页参数</param>
        /// <returns></returns>
        public virtual ISqlBuilder Pager(IPager pager)
        {
            _pager = pager;
            return this;
        }

        #endregion
    }
}
