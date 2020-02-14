﻿using System;
using Microsoft.EntityFrameworkCore;

namespace Bing.Samples.Data.UnitOfWorks.SqlServer {
    /// <summary>
    /// 工作单元
    /// </summary>
    public class SampleUnitOfWork : Bing.Datas.EntityFramework.SqlServer.UnitOfWork, ISampleUnitOfWork {
        /// <summary>
        /// 初始化工作单元
        /// </summary>
        /// <param name="options">配置项</param>
        public SampleUnitOfWork( DbContextOptions<SampleUnitOfWork> options) 
            : base( options ) {
        }
    }
}
