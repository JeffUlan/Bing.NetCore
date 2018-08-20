﻿namespace Bing.Datas.Sql.Queries.Builders.Conditions
{
    /// <summary>
    /// Sql查询条件
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        string GetCondition();
    }
}
