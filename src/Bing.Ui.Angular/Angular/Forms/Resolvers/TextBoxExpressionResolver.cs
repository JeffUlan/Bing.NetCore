﻿using System.Reflection;
using Bing.Ui.Angular.Forms.Configs;
using Bing.Ui.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Bing.Ui.Angular.Forms.Resolvers
{
    /// <summary>
    /// 文本框表达式解析器
    /// </summary>
    public class TextBoxExpressionResolver
    {
        /// <summary>
        /// 属性表达式
        /// </summary>
        private readonly ModelExpression _expression;

        /// <summary>
        /// 配置
        /// </summary>
        private readonly TextBoxConfig _config;

        /// <summary>
        /// 成员
        /// </summary>
        private readonly MemberInfo _memberInfo;

        /// <summary>
        /// 初始化一个<see cref="TextBoxExpressionResolver"/>类型的实例
        /// </summary>
        /// <param name="expression">属性表达式</param>
        /// <param name="config">配置</param>
        private TextBoxExpressionResolver(ModelExpression expression, TextBoxConfig config)
        {
            if (expression == null || config == null)
            {
                return;
            }
            _expression = expression;
            _config = config;
            _memberInfo = expression.GetMemberInfo();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="expression">属性表达式</param>
        /// <param name="config">配置</param>
        public static void Init(ModelExpression expression, TextBoxConfig config)
        {
            new TextBoxExpressionResolver(expression, config).Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            Internal.Helper.Init(_config, _expression, _memberInfo);
            Internal.Helper.InitDataType(_config, _memberInfo);
            Internal.Helper.InitValidation(_config, _memberInfo);
        }
    }
}
