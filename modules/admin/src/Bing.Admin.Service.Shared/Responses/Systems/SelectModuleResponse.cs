﻿using System;
using System.Collections.Generic;
using Bing.Application.Dtos;

namespace Bing.Admin.Service.Shared.Responses.Systems
{
    /// <summary>
    /// 选中模块响应
    /// </summary>
    public class SelectModuleResponse : IResponse
    {
        /// <summary>
        /// 初始化一个<see cref="ModuleResponse"/>类型的实例
        /// </summary>
        public SelectModuleResponse()
        {
            Children = new List<SelectModuleResponse>();
            Buttons = new List<SelectOperationResponse>();
        }

        /// <summary>
        /// 模块标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 应用程序标识
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// 父标识
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? SortId { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// 子模块
        /// </summary>
        public List<SelectModuleResponse> Children { get; set; }

        /// <summary>
        /// 按钮列表
        /// </summary>
        public List<SelectOperationResponse> Buttons { get; set; }
    }
}
