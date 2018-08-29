﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bing.Datas.Sql.Queries.Builders.Abstractions;
using Bing.Datas.Sql.Queries.Builders.Conditions;
using Bing.Datas.Sql.Queries.Builders.Core;
using Bing.Datas.Sql.Queries.Builders.Internal;
using Bing.Properties;
using Bing.Utils;
using Bing.Utils.Extensions;
using Bing.Utils.Helpers;

namespace Bing.Datas.Sql.Queries.Builders.Clauses
{
    /// <summary>
    /// Where子句
    /// </summary>
    public class WhereClause:IWhereClause
    {        
        /// <summary>
        /// 实体解析器
        /// </summary>
        private readonly IEntityResolver _resolver;

        /// <summary>
        /// 辅助操作
        /// </summary>
        private readonly Helper _helper;

        /// <summary>
        /// 谓词表达式解析器
        /// </summary>
        private readonly PredicateExpressionResolver _expressionResolver;

        /// <summary>
        /// 查询条件
        /// </summary>
        private ICondition _condition;

        /// <summary>
        /// 初始化一个<see cref="WhereClause"/>类型的实例
        /// </summary>
        /// <param name="dialect">Sql方言</param>
        /// <param name="resolver">实体解析器</param>
        /// <param name="register">实体别名注册器</param>
        /// <param name="parameterManager">参数管理器</param>
        /// <param name="tag">参数标识</param>
        public WhereClause(IDialect dialect, IEntityResolver resolver, IEntityAliasRegister register,
            IParameterManager parameterManager)
        {
            _resolver = resolver;
            _helper = new Helper(dialect, resolver, register, parameterManager);
            _expressionResolver = new PredicateExpressionResolver(dialect, resolver, register, parameterManager);
        }

        /// <summary>
        /// And连接条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        public void And(ICondition condition)
        {
            _condition = new AndCondition(_condition, condition);
        }

        /// <summary>
        /// Or连接条件
        /// </summary>
        /// <param name="condition">查询条件</param>
        public void Or(ICondition condition)
        {
            _condition = new OrCondition(_condition, condition);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        public void Where(string column, object value, Operator @operator = Operator.Equal)
        {
            And(_helper.CreateCondition(column, value, @operator));
        }        

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        public void Where<TEntity>(Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal) where TEntity : class
        {
            Where(_helper.GetColumn(expression), value, @operator);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">查询条件表达式</param>
        public void Where<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var condition = _expressionResolver.Resolve(expression);
            And(condition);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值</param>
        /// <param name="condition">拼接条件，该值为true时添加查询条件，否则忽略</param>
        /// <param name="operator">运算符</param>
        public void WhereIf(string column, object value, bool condition, Operator @operator = Operator.Equal)
        {
            if (condition)
            {
                Where(column,value,@operator);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值</param>
        /// <param name="condition">拼接条件，该值为true时添加查询条件，否则忽略</param>
        /// <param name="operator">运算符</param>
        public void WhereIf<TEntity>(Expression<Func<TEntity, object>> expression, object value, bool condition, Operator @operator = Operator.Equal) where TEntity : class
        {
            if (condition)
            {
                Where(expression,value,@operator);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">查询条件表达式</param>
        /// <param name="condition">拼接条件，该值为true时添加查询条件，否则忽略</param>
        public void WhereIf<TEntity>(Expression<Func<TEntity, bool>> expression, bool condition) where TEntity : class
        {
            if (condition)
            {
                Where(expression);
            }
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="value">值，如果该值为空，则忽略该查询条件</param>
        /// <param name="operator">运算符</param>
        public void WhereIfNotEmpty(string column, object value, Operator @operator = Operator.Equal)
        {
            if (string.IsNullOrWhiteSpace(value.SafeString()))
            {
                return;
            }
            Where(column,value,@operator);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="value">值，如果该值为空，则忽略该查询条件</param>
        /// <param name="operator">运算符</param>
        public void WhereIfNotEmpty<TEntity>(Expression<Func<TEntity, object>> expression, object value, Operator @operator = Operator.Equal) where TEntity : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (string.IsNullOrWhiteSpace(value.SafeString()))
            {
                return;
            }
            Where(expression, value, @operator);
        }

        /// <summary>
        /// 设置查询条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">查询条件表达式，如果参数值为空，则忽略该查询条件</param>
        public void WhereIfNotEmpty<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (Lambda.GetConditionCount(expression) > 1)
            {
                throw new InvalidOperationException(string.Format(LibraryResource.OnlyOnePredicate,expression));
            }
            if (string.IsNullOrWhiteSpace(Lambda.GetValue(expression).SafeString()))
            {
                return;
            }
            Where(expression);
        }

        /// <summary>
        /// 添加到Where子句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        public void AppendSql(string sql)
        {
            And(new SqlCondition(sql));
        }

        /// <summary>
        /// 设置Is Null条件
        /// </summary>
        /// <param name="column">列名</param>
        public void IsNull(string column)
        {
            Where(column, null);
        }

        /// <summary>
        /// 设置Is Null条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        public void IsNull<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            Where(expression, null);
        }

        /// <summary>
        /// 设置Is Not Null条件
        /// </summary>
        /// <param name="column">列名</param>
        public void IsNotNull(string column)
        {
            column = _helper.GetColumn(column);
            And(new IsNotNullCondition(column));
        }

        /// <summary>
        /// 设置Is Not Null条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        public void IsNotNull<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            var column = _helper.GetColumn(_resolver.GetColumn(expression), typeof(TEntity));
            IsNotNull(column);
        }

        /// <summary>
        /// 设置空条件
        /// </summary>
        /// <param name="column">列名</param>
        public void IsEmpty(string column)
        {
            column = _helper.GetColumn(column);
            And(new OrCondition(new IsNullCondition(column), new EqualCondition(column, "''")));
        }

        /// <summary>
        /// 设置空条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        public void IsEmpty<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            var column = _helper.GetColumn(_resolver.GetColumn(expression), typeof(TEntity));
            IsEmpty(column);
        }

        /// <summary>
        /// 设置非空条件
        /// </summary>
        /// <param name="column">列名</param>
        public void IsNotEmpty(string column)
        {
            column = _helper.GetColumn(column);
            And(new OrCondition(new IsNotNullCondition(column), new NotEqualCondition(column, "''")));
        }

        /// <summary>
        /// 设置非空条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        public void IsNotEmpty<TEntity>(Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            var column = _helper.GetColumn(_resolver.GetColumn(expression), typeof(TEntity));
            IsNotEmpty(column);
        }

        /// <summary>
        /// 设置In条件
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="values">值集合</param>
        public void In(string column, IEnumerable<object> values)
        {
            Where(column,values,Operator.Contains);
        }

        /// <summary>
        /// 设置In条件
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="expression">列名表达式</param>
        /// <param name="values">值集合</param>
        public void In<TEntity>(Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
            where TEntity : class
        {
            Where(expression,values,Operator.Contains);
        }

        /// <summary>
        /// 输出Sql
        /// </summary>
        /// <returns></returns>
        public string ToSql()
        {
            var condition = GetCondition();
            if (string.IsNullOrWhiteSpace(condition))
            {
                return null;
            }
            return $"Where {condition}";
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        public string GetCondition()
        {
            return _condition?.GetCondition();
        }
    }
}
