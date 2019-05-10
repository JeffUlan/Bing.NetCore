﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using Bing.Ui.Angular.Internal;
using Bing.Utils.Helpers;

namespace Bing.Ui.CkEditor.Models
{
    /// <summary>
    /// 模型富文本框编辑器
    /// </summary>
    /// <typeparam name="TModel">模型类型</typeparam>
    /// <typeparam name="TProperty">属性类型</typeparam>
    public class ModelEditor<TModel, TProperty> : Editor
    {
        /// <summary>
        /// 属性表达式
        /// </summary>
        private readonly Expression<Func<TModel, TProperty>> _expression;

        /// <summary>
        /// 成员
        /// </summary>
        private readonly MemberInfo _memberInfo;

        /// <summary>
        /// 初始化一个<see cref="ModelEditor{TModel,TProperty}"/>类型的实例
        /// </summary>
        /// <param name="expression">属性表达式</param>
        public ModelEditor(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
            {
                return;
            }

            _expression = expression;
            _memberInfo = Lambda.GetMember(_expression);
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            Helper.Init(OptionConfig, _expression, _memberInfo);
        }
    }
}
