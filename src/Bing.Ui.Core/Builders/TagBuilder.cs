﻿using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Bing.Ui.Builders
{
    /// <summary>
    /// 标签生成器，注意：已禁用Html编码
    /// </summary>
    public class TagBuilder:IHtmlContent
    {
        /// <summary>
        /// 标签生成器
        /// </summary>
        private readonly Microsoft.AspNetCore.Mvc.Rendering.TagBuilder _tagBuilder;

        /// <summary>
        /// 类
        /// </summary>
        private readonly List<string> _classList;

        /// <summary>
        /// Html内容
        /// </summary>
        public IHtmlContentBuilder InnerHtml => _tagBuilder.InnerHtml;

        /// <summary>
        /// 是否包含Html内容
        /// </summary>
        public bool HasInnerHtml => _tagBuilder.HasInnerHtml;

        /// <summary>
        /// 空标签生成器
        /// </summary>
        public static readonly TagBuilder Null = new EmptyTagBuilder();

        /// <summary>
        /// 初始化一个<see cref="TagBuilder"/>类型的实例
        /// </summary>
        /// <param name="tagName">标签名称，范例：div</param>
        /// <param name="renderMode">渲染模式</param>
        public TagBuilder(string tagName, TagRenderMode renderMode = TagRenderMode.Normal)
        {
            _tagBuilder = new Microsoft.AspNetCore.Mvc.Rendering.TagBuilder(tagName) {TagRenderMode = renderMode};
            _classList = new List<string>();
        }

        /// <summary>
        /// 获取标签生成器
        /// </summary>
        /// <returns></returns>
        public Microsoft.AspNetCore.Mvc.Rendering.TagBuilder GetTagBuilder()
        {
            return _tagBuilder;
        }

        /// <summary>
        /// 添加class属性
        /// </summary>
        /// <param name="class">class属性值</param>
        /// <returns></returns>
        public TagBuilder Class(string @class)
        {
            if (string.IsNullOrWhiteSpace(@class))
            {
                return this;
            }
            if (_classList.Contains(@class))
            {
                return this;
            }
            _classList.Add(@class);
            _tagBuilder.AddCssClass(@class);
            return this;
        }

        /// <summary>
        /// 添加属性，当属性名已存在则忽略，也可进行替换
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        /// <param name="replaceExisting">是否替换已存在的属性</param>
        /// <returns></returns>
        public TagBuilder Attribute(string name, string value, bool replaceExisting = false)
        {
            _tagBuilder.MergeAttribute(name, value, replaceExisting);
            return this;
        }

        /// <summary>
        /// 添加属性，当属性名已存在则忽略
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        /// <param name="ignoreIfValueIsEmpty">当值为空时忽略</param>
        /// <returns></returns>
        public TagBuilder AddAttribute(string name, string value, bool ignoreIfValueIsEmpty = true)
        {
            if (ignoreIfValueIsEmpty && string.IsNullOrWhiteSpace(value))
            {
                return this;
            }
            Attribute(name, value);
            return this;
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns></returns>
        public TagBuilder AddAttribute(string name)
        {
            return Attribute(name, string.Empty);
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public TagBuilder AppendContent(string content)
        {
            _tagBuilder.InnerHtml.AppendHtml(content);
            return this;
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public TagBuilder AppendContent(IHtmlContent content)
        {
            _tagBuilder.InnerHtml.AppendHtml(content);
            return this;
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="tagBuilder">标签生成器</param>
        /// <returns></returns>
        public TagBuilder AppendContent(TagBuilder tagBuilder)
        {
            AppendContent(tagBuilder.GetTagBuilder());
            return this;
        }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public TagBuilder SetContent(string content)
        {
            _tagBuilder.InnerHtml.SetHtmlContent(content);
            return this;
        }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public TagBuilder SetContent(IHtmlContent content)
        {
            _tagBuilder.InnerHtml.SetHtmlContent(content);
            return this;
        }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="tagBuilder">标签生成器</param>
        /// <returns></returns>
        public TagBuilder SetContent(TagBuilder tagBuilder)
        {
            _tagBuilder.InnerHtml.SetHtmlContent(tagBuilder.GetTagBuilder());
            return this;
        }

        /// <summary>
        /// 写入文本流
        /// </summary>
        /// <param name="writer">流写入器</param>
        /// <param name="encoder">编码</param>
        public virtual void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            _tagBuilder.WriteTo(writer, encoder);
        }

        /// <summary>
        /// 渲染起始标签
        /// </summary>
        /// <param name="writer">流写入器</param>
        public void RenderStartTag(TextWriter writer)
        {
            _tagBuilder.RenderStartTag().WriteTo(writer, NullHtmlEncoder.Default);
        }

        /// <summary>
        /// 渲染内容
        /// </summary>
        /// <param name="writer">流写入器</param>
        public void RenderBody(TextWriter writer)
        {
            _tagBuilder.RenderBody().WriteTo(writer, NullHtmlEncoder.Default);
        }

        /// <summary>
        /// 渲染结束标签
        /// </summary>
        /// <param name="writer">流写入器</param>
        public void RenderEndTag(TextWriter writer)
        {
            _tagBuilder.RenderEndTag().WriteTo(writer, NullHtmlEncoder.Default);
        }

        /// <summary>
        /// 获取Html结果
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            using (var writer=new StringWriter())
            {
                _tagBuilder.WriteTo(writer, NullHtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
