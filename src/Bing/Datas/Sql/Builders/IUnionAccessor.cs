﻿using System.Collections.Generic;
using Bing.Datas.Sql.Builders.Core;

namespace Bing.Datas.Sql.Builders
{
    /// <summary>
    /// 联合操作访问器
    /// </summary>
    public interface IUnionAccessor
    {
        /// <summary>
        /// 是否包含联合操作
        /// </summary>
        bool IsUnion { get; }

        /// <summary>
        /// 联合操作项集合
        /// </summary>
        List<BuilderItem> UnionItems { get; }
    }
}
