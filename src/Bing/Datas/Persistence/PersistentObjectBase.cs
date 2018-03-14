﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bing.Datas.Persistence
{
    /// <summary>
    /// 持久化对象
    /// </summary>
    /// <typeparam name="TKey">标识类型</typeparam>
    public abstract class PersistentObjectBase<TKey> : IPersistentObject<TKey>
    {
        /// <summary>
        /// 标识
        /// </summary>
        [Key]
        public TKey Id { get; set; }

        /// <summary>
        /// 版本号（乐观锁）
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        /// 相等运算
        /// </summary>
        /// <param name="other">比较对象</param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            return this == (PersistentObjectBase<TKey>)other;
        }

        /// <summary>
        /// 获取哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ReferenceEquals(Id, null) ? 0 : Id.GetHashCode();
        }

        /// <summary>
        /// 相等比较
        /// </summary>
        /// <param name="left">左比较对象</param>
        /// <param name="right">右比较对象</param>
        /// <returns></returns>
        public static bool operator ==(PersistentObjectBase<TKey> left, PersistentObjectBase<TKey> right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            if (left.GetType() != right.GetType())
            {
                return false;
            }
            if (Equals(left.Id, null))
            {
                return false;
            }
            if (left.Id.Equals(default(TKey)))
            {
                return false;
            }
            return left.Id.Equals(right.Id);
        }

        /// <summary>
        /// 不相等比较
        /// </summary>
        /// <param name="left">左比较对象</param>
        /// <param name="right">右比较对象</param>
        /// <returns></returns>
        public static bool operator !=(PersistentObjectBase<TKey> left, PersistentObjectBase<TKey> right)
        {
            return !(left == right);
        }
    }
}
