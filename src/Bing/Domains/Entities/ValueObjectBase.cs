﻿using System;
using System.Linq;
using System.Reflection;

namespace Bing.Domains.Entities
{
    /// <summary>
    /// 值对象
    /// </summary>
    /// <typeparam name="TValueObject">值对象类型</typeparam>
    public abstract class ValueObjectBase<TValueObject> : DomainBase<TValueObject>, IEquatable<TValueObject> where TValueObject : ValueObjectBase<TValueObject>
    {
        /// <summary>
        /// 相等性比较
        /// </summary>
        /// <param name="other">值对象</param>
        /// <returns></returns>
        public bool Equals(TValueObject other)
        {
            return this == other;
        }

        /// <summary>
        /// 相等性比较
        /// </summary>
        /// <param name="other">值对象</param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            return Equals(other as TValueObject);
        }

        /// <summary>
        /// 相等性比较
        /// </summary>
        /// <param name="left">值对象</param>
        /// <param name="right">值对象</param>
        /// <returns></returns>
        public static bool operator ==(ValueObjectBase<TValueObject> left, ValueObjectBase<TValueObject> right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            if (!(left is TValueObject) || !(right is TValueObject))
            {
                return false;
            }
            var properties = left.GetType().GetTypeInfo().GetProperties();
            return properties.All(property => property.GetValue(left) == property.GetValue(right));
        }

        /// <summary>
        /// 不相等比较
        /// </summary>
        /// <param name="left">值对象</param>
        /// <param name="right">值对象</param>
        /// <returns></returns>
        public static bool operator !=(ValueObjectBase<TValueObject> left, ValueObjectBase<TValueObject> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 获取哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var properties = GetType().GetTypeInfo().GetProperties();
            return properties.Select(property => property.GetValue(this))
                .Where(value => value != null)
                .Aggregate(0, (current, value) => current ^ value.GetHashCode());
        }

        /// <summary>
        /// 克隆副本
        /// </summary>
        /// <returns></returns>
        public virtual TValueObject Clone()
        {
            return Utils.Helpers.Conv.To<TValueObject>(MemberwiseClone());
        }
    }
}
