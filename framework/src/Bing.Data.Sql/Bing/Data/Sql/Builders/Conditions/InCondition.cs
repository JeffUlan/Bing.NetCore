﻿using System.Text;
using Bing.Extensions;

namespace Bing.Data.Sql.Builders.Conditions;

/// <summary>
/// In查询条件
/// </summary>
public class InCondition : ICondition
{
    /// <summary>
    /// 列名
    /// </summary>
    private readonly string _name;

    /// <summary>
    /// 值集合
    /// </summary>
    private readonly IList<string> _values;

    /// <summary>
    /// 初始化一个<see cref="InCondition"/>类型的实例
    /// </summary>
    /// <param name="name">列名</param>
    /// <param name="values">值集合</param>
    public InCondition(string name, IList<string> values)
    {
        _name = name;
        _values = values;
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    public string GetCondition()
    {
        if (string.IsNullOrWhiteSpace(_name) || _values == null || _values.Count == 0)
            return null;
        var result = new StringBuilder();
        result.Append($"{_name} In (");
        result.Append(_values.Join());
        result.Append(")");
        return result.ToString();
    }
}