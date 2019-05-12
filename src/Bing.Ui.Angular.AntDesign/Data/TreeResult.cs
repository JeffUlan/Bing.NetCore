﻿using System.Collections.Generic;
using System.Linq;
using Bing.Utils.Extensions;

namespace Bing.Ui.Data
{
    /// <summary>
    /// 树形结果
    /// </summary>
    public class TreeResult
    {
        /// <summary>
        /// 树形参数列表
        /// </summary>
        private readonly IEnumerable<TreeDto> _data;

        /// <summary>
        /// 树形结果
        /// </summary>
        private readonly ZorroTreeResult _result;

        /// <summary>
        /// 是否异步加载
        /// </summary>
        private readonly bool _async;

        /// <summary>
        /// 初始化一个<see cref="TreeResult"/>类型的实例
        /// </summary>
        /// <param name="data">树形参数列表</param>
        /// <param name="async">是否异步加载</param>
        public TreeResult(IEnumerable<TreeDto> data, bool async = false)
        {
            _data = data;
            _async = async;
            _result = new ZorroTreeResult();
        }

        /// <summary>
        /// 获取树形结果
        /// </summary>
        /// <returns></returns>
        public ZorroTreeResult GetResult()
        {
            if (_data == null)
            {
                return _result;
            }

            foreach (var root in _data.Where(IsRoot))
            {
                AddNodes(root);
                InitKeys();
            }

            return _result;
        }

        /// <summary>
        /// 是否根节点
        /// </summary>
        /// <param name="dto">树形数据传输对象</param>
        protected virtual bool IsRoot(TreeDto dto)
        {
            if (_data.Any(t => t.ParentId.IsEmpty()))
            {
                return dto.ParentId.IsEmpty();
            }

            return dto.Level == _data.Min(t => t.Level);
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="root">树形数据传输对象</param>
        private void AddNodes(TreeDto root)
        {
            var rootNode = ToNode(root);
            _result.Nodes.Add(rootNode);
            AddChildren(rootNode);
        }

        /// <summary>
        /// 转换为树节点
        /// </summary>
        /// <param name="dto">树形数据传输对象</param>
        protected virtual ZorroTreeNode ToNode(TreeDto dto)
        {
            var result=new ZorroTreeNode()
            {
                Key = dto.Id,
                Title = dto.Text,
                Icon = dto.Icon,
                Disabled = !dto.Enabled.SafeValue(),
                Expanded = dto.Expanded.SafeValue(),
                Checked = dto.Checked.SafeValue(),
                DisableCheckbox = dto.DisableCheckbox.SafeValue(),
                Selectable = dto.Selectable.SafeValue(),
                Selected = dto.Selected.SafeValue()
            };
            InitIsLeaf(result);
            return result;
        }

        /// <summary>
        /// 初始化叶节点状态
        /// </summary>
        /// <param name="node">树节点</param>
        protected virtual void InitIsLeaf(ZorroTreeNode node)
        {
            node.IsLeaf = false;
            if (_async)
            {
                return;
            }

            if (IsLeaf(node))
            {
                node.IsLeaf = true;
            }
        }

        /// <summary>
        /// 是否叶节点
        /// </summary>
        /// <param name="node">树节点</param>
        protected virtual bool IsLeaf(ZorroTreeNode node)
        {
            if (node.Key.IsEmpty())
            {
                return true;
            }

            return _data.All(t => t.ParentId != node.Key);
        }

        /// <summary>
        /// 获取节点直接下级
        /// </summary>
        /// <param name="parentId">父标识</param>
        private List<TreeDto> GetChildren(string parentId)
        {
            return _data.Where(t => t.ParentId == parentId).ToList();
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="node">树节点</param>
        private void AddChildren(ZorroTreeNode node)
        {
            if (node == null)
            {
                return;
            }

            node.Children = GetChildren(node.Key).Select(ToNode).ToList();
            foreach (var child in node.Children)
            {
                AddChildren(child);
            }
        }

        /// <summary>
        /// 初始化展开、选中标识列表
        /// </summary>
        private void InitKeys()
        {
            _result.ExpandedKeys = _data.Where(t => t.Expanded == true).Select(t => t.Id).ToList();
            _result.CheckedKeys = _data.Where(t => t.Checked == true).Select(t => t.Id).ToList();
            _result.SelectedKeys = _data.Where(t => t.Selected == true).Select(t => t.Id).ToList();
        }
    }
}
