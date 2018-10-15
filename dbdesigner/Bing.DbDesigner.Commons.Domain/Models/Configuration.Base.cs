﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bing;
using Bing.Utils;
using Bing.Domains;
using Bing.Domains.Entities;
using Bing.Domains.Entities.Auditing;
using Bing.Domains.Entities.Tenants;

namespace Bing.DbDesigner.Commons.Domain.Models {
    /// <summary>
    /// 系统配置
    /// </summary>
    [DisplayName( "系统配置" )]
    public partial class Configuration : AggregateRoot<Configuration>,ITenant,IDelete,IAudited {
        /// <summary>
        /// 初始化系统配置
        /// </summary>
        public Configuration() : this( Guid.Empty ) {
        }

        /// <summary>
        /// 初始化系统配置
        /// </summary>
        /// <param name="id">系统配置标识</param>
        public Configuration( Guid id ) : base( id ) {
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName( "名称" )]
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength( 250, ErrorMessage = "名称输入过长，不能超过250位" )]
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [DisplayName( "编码" )]
        [StringLength( 250, ErrorMessage = "编码输入过长，不能超过250位" )]
        public string Code { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [DisplayName( "值" )]
        public string Value { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [DisplayName( "是否删除" )]
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName( "创建时间" )]
        public DateTime? CreationTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [DisplayName( "创建人" )]
        public Guid? CreatorId { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DisplayName( "最后修改时间" )]
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        [DisplayName( "最后修改人" )]
        public Guid? LastModifierId { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        [DisplayName( "启用" )]
        public bool Enabled { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName( "备注" )]
        [StringLength( 500, ErrorMessage = "备注输入过长，不能超过500位" )]
        public string Note { get; set; }
        /// <summary>
        /// 租户标识
        /// </summary>
        [DisplayName( "租户标识" )]
        [StringLength( 50, ErrorMessage = "租户标识输入过长，不能超过50位" )]
        public string TenantId { get; set; }
        
        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            AddDescription( t => t.Id );
            AddDescription( t => t.Name );
            AddDescription( t => t.Code );
            AddDescription( t => t.Value );
            AddDescription( t => t.CreationTime );
            AddDescription( t => t.CreatorId );
            AddDescription( t => t.LastModificationTime );
            AddDescription( t => t.LastModifierId );
            AddDescription( t => t.Enabled );
            AddDescription( t => t.Note );
            AddDescription( t => t.TenantId );
        }
        
        /// <summary>
        /// 添加变更列表
        /// </summary>
        protected override void AddChanges( Configuration other ) {
            AddChange( t => t.Id, other.Id );
            AddChange( t => t.Name, other.Name );
            AddChange( t => t.Code, other.Code );
            AddChange( t => t.Value, other.Value );
            AddChange( t => t.CreationTime, other.CreationTime );
            AddChange( t => t.CreatorId, other.CreatorId );
            AddChange( t => t.LastModificationTime, other.LastModificationTime );
            AddChange( t => t.LastModifierId, other.LastModifierId );
            AddChange( t => t.Enabled, other.Enabled );
            AddChange( t => t.Note, other.Note );
            AddChange( t => t.TenantId, other.TenantId );
        }
    }
}