﻿namespace Bing.Domains.Entities
{
    /// <summary>
    /// 逻辑删除
    /// </summary>
    public interface IDelete
    {
        /// <summary>
        /// 是否已删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
