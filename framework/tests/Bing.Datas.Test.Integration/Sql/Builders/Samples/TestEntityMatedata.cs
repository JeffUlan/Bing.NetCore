﻿using System;
using Bing.Data.Sql.Matedatas;

namespace Bing.Data.Test.Integration.Sql.Builders.Samples;

/// <summary>
/// 测试实体元数据
/// </summary>
public class TestEntityMatedata : IEntityMatedata
{
    /// <summary>
    /// 获取表名
    /// </summary>
    /// <param name="entity">实体类型</param>
    public string GetTable(Type entity) => $"t_{entity.Name}";

    /// <summary>
    /// 获取架构
    /// </summary>
    /// <param name="entity">实体类型</param>
    public string GetSchema(Type entity) => $"as_{entity.Name}";

    /// <summary>
    /// 获取列名
    /// </summary>
    /// <param name="entity">实体类型</param>
    /// <param name="property">属性名</param>
    public string GetColumn(Type entity, string property) => property == "DecimalValue" ? property : $"{entity.Name}_{property}";
}