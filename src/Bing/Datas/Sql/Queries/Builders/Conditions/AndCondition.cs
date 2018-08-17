﻿namespace Bing.Datas.Sql.Queries.Builders.Conditions
{
    /// <summary>
    /// And连接条件
    /// </summary>
    public class AndCondition:ICondition
    {
        /// <summary>
        /// 左操作数
        /// </summary>
        private readonly string _left;

        /// <summary>
        /// 右操作数
        /// </summary>
        private readonly string _right;

        /// <summary>
        /// 初始化一个<see cref="AndCondition"/>类型的实例
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public AndCondition(string left, string right)
        {
            _left = left;
            _right = right;
        }

        /// <summary>
        /// 初始化一个<see cref="AndCondition"/>类型的实例
        /// </summary>
        /// <param name="left">左操作数</param>
        /// <param name="right">右操作数</param>
        public AndCondition(ICondition left, ICondition right)
        {
            _left = left?.Getondition();
            _right = right?.Getondition();
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        public string Getondition()
        {
            if (string.IsNullOrWhiteSpace(_left))
            {
                return _right;
            }

            if (string.IsNullOrWhiteSpace(_right))
            {
                return _left;
            }

            return $"{_left} And {_right}";
        }
    }
}
