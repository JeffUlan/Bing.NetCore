﻿using Bing.Admin.Systems.Domain.Models;
using FreeSql.Extensions.EfCoreFluentApi;

namespace Bing.Admin.Data.Mappings.Systems.MySql
{
    /// <summary>
    /// 用户角色 映射配置
    /// </summary>
    public class UserRoleMap : Bing.FreeSQL.MySql.EntityMap<UserRole>
    {
        /// <summary>
        /// 映射表
        /// </summary>
        protected override void MapTable( EfCoreTableFluent<UserRole> builder ) 
        {
            builder.ToTable( "`Systems.UserRole`" );
        }
                
        /// <summary>
        /// 映射属性
        /// </summary>
        protected override void MapProperties( EfCoreTableFluent<UserRole> builder ) 
        {
            // 复合标识
            builder.HasKey(t => new { t.UserId, t.RoleId });
        }
    }
}
