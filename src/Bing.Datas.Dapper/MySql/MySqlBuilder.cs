﻿using Bing.Datas.Matedatas;
using Bing.Datas.Sql;
using Bing.Datas.Sql.Builders;
using Bing.Datas.Sql.Builders.Core;

namespace Bing.Datas.Dapper.MySql
{
    /// <summary>
    /// MySql Sql生成器
    /// </summary>
    public class MySqlBuilder:SqlBuilderBase
    {
        /// <summary>
        /// 初始化一个<see cref="MySqlBuilder"/>类型的实例
        /// </summary>
        /// <param name="matedata">实体元数据解析器</param>
        /// <param name="parameterManager">参数管理器</param>
        public MySqlBuilder(IEntityMatedata matedata=null,IParameterManager parameterManager = null) : base(matedata, parameterManager) { }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public override ISqlBuilder Clone()
        {
            var sqlBuilder = new MySqlBuilder();
            sqlBuilder.Clone(this);
            return sqlBuilder;
        }

        /// <summary>
        /// 创建Sql生成器
        /// </summary>
        /// <returns></returns>
        public override ISqlBuilder New()
        {
            return new MySqlBuilder(EntityMatedata, ParameterManager);
        }        

        /// <summary>
        /// 创建分页Sql
        /// </summary>
        protected override string CreateLimitSql()
        {
            return $"Limit {GetOffsetParam()}, {GetLimitParam()}";
        }

        /// <summary>
        /// 获取Sql方言
        /// </summary>
        /// <returns></returns>
        protected override IDialect GetDialect()
        {
            return new MySqlDialect();
        }

        /// <summary>
        /// 创建From子句
        /// </summary>
        /// <returns></returns>
        protected override IFromClause CreateFromClause()
        {
            return new MySqlFromClause(this, GetDialect(), EntityResolver, AliasRegister);
        }

        /// <summary>
        /// 创建Join子句
        /// </summary>
        /// <returns></returns>
        protected override IJoinClause CreateJoinClause()
        {
            return new MySqlJoinClause(this, GetDialect(), EntityResolver, AliasRegister);
        }
    }
}
