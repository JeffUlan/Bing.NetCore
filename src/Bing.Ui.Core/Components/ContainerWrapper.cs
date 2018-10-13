﻿using System;
using Bing.Ui.Components.Internal;

namespace Bing.Ui.Components
{
    /// <summary>
    /// 容器包装器
    /// </summary>
    public class ContainerWrapper:IDisposable
    {
        /// <summary>
        /// 容器
        /// </summary>
        private readonly IContainer _container;

        /// <summary>
        /// 初始化一个<see cref="ContainerWrapper"/>类型的实例
        /// </summary>
        /// <param name="container">容器</param>
        public ContainerWrapper(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// 渲染结束
        /// </summary>
        public void Dispose()
        {
            if (_container is IRenderEnd container)
            {
                container.End();
            }
        }
    }
}
