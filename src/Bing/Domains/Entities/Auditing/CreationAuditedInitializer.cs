﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bing.Configuration;
using Bing.Contexts;
using Bing.Domains.Sessions;
using Bing.Utils.Extensions;

namespace Bing.Domains.Entities.Auditing
{
    /// <summary>
    /// 创建操作审计初始化器
    /// </summary>
    public class CreationAuditedInitializer
    {
        /// <summary>
        /// 实体
        /// </summary>
        private readonly object _entity;

        /// <summary>
        /// 用户会话
        /// </summary>
        private readonly ISession _session;

        /// <summary>
        /// 初始化一个<see cref="CreationAuditedInitializer"/>类型的实例
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="session">用户会话</param>
        private CreationAuditedInitializer(object entity, ISession session)
        {
            _entity = entity;
            _session = session;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="session">用户会话</param>
        public static void Init(object entity, ISession session)
        {
            new CreationAuditedInitializer(entity, session).Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (_entity is ICreationAudited<Guid>)
            {
                InitGuid();
                return;
            }
            if (_entity is ICreationAudited<Guid?>)
            {
                InitNullableGuid();
                return;
            }
            if (_entity is ICreationAudited<int>)
            {
                InitInt();
                return;
            }
            if (_entity is ICreationAudited<int?>)
            {
                InitNullableInt();
                return;
            }
            if (_entity is ICreationAudited<string>)
            {
                InitString();
                return;
            }
            if (_entity is ICreationAudited<long>)
            {
                InitLong();
                return;
            }
            if (_entity is ICreationAudited<long?>)
            {
                InitNullableLong();
                return;
            }
        }

        /// <summary>
        /// 初始化Guid
        /// </summary>
        private void InitGuid()
        {
            var result = (ICreationAudited<Guid>)_entity;
            result.CreationTime = DateTime.Now;
            result.CreatorId = _session.UserId.ToGuid();
        }

        /// <summary>
        /// 初始化可空Guid
        /// </summary>
        private void InitNullableGuid()
        {
            var result = (ICreationAudited<Guid?>)_entity;
            result.CreationTime = DateTime.Now;
            result.CreatorId = _session.UserId.ToGuidOrNull();
        }

        /// <summary>
        /// 初始化int
        /// </summary>
        private void InitInt()
        {
            var result = (ICreationAudited<int>)_entity;
            result.CreationTime = DateTime.Now;
            result.CreatorId = _session.UserId.ToInt();
        }

        /// <summary>
        /// 初始化可空int
        /// </summary>
        private void InitNullableInt()
        {
            var result = (ICreationAudited<int?>)_entity;
            result.CreationTime = DateTime.Now;
            result.CreatorId = _session.UserId.ToIntOrNull();
        }

        /// <summary>
        /// 初始化Long
        /// </summary>
        private void InitLong()
        {
            var result = (ICreationAudited<long>)_entity;
            result.CreationTime = DateTime.Now;
            result.CreatorId = _session.UserId.ToLong();
        }

        /// <summary>
        /// 初始化可空Long
        /// </summary>
        private void InitNullableLong()
        {
            var result = (ICreationAudited<long?>)_entity;
            result.CreationTime = DateTime.Now;
            result.CreatorId = _session.UserId.ToLongOrNull();
        }

        /// <summary>
        /// 初始化字符串
        /// </summary>
        private void InitString()
        {
            var result = (ICreationAudited<string>)_entity;
            result.CreationTime = DateTime.Now;
            if (result.CreatorId.IsEmpty())
            {
                result.CreatorId = BingConfig.Current.EnabledUserName
                    ? _session.UserName.SafeString()
                    : _session.UserId.SafeString();
            }           
        }
    }
}
