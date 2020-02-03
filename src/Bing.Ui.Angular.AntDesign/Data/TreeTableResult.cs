﻿using System.Collections.Generic;
using System.Linq;
using Bing.Extensions;
using Bing.Mapping;
using Bing.Extensions;

namespace Bing.Ui.Data
{
    /// <summary>
    /// 树形表格结果
    /// </summary>
    /// <typeparam name="TNode">树节点类型</typeparam>
    public class TreeTableResult<TNode> where TNode : TreeDto
    {
        /// <summary>
        /// 树形参数列表
        /// </summary>
        private readonly IEnumerable<TNode> _data;

        /// <summary>
        /// 树形表格结果
        /// </summary>
        private readonly List<ZorroTreeTableResult<TNode>> _result;

        /// <summary>
        /// 是否异步加载
        /// </summary>
        private readonly bool _async;

        /// <summary>
        /// 初始化一个<see cref="TreeTableResult{TNode}"/>类型的实例
        /// </summary>
        /// <param name="data">树形参数列表</param>
        /// <param name="async">是否异步加载</param>
        public TreeTableResult(IEnumerable<TNode> data, bool async = false)
        {
            _data = data;
            _async = async;
            _result = new List<ZorroTreeTableResult<TNode>>();
        }

        /// <summary>
        /// 获取树形表格结果
        /// </summary>
        public List<ZorroTreeTableResult<TNode>> GetResult()
        {
            if (_data == null)
            {
                return _result;
            }

            foreach (var root in _data.Where(IsRoot).OrderBy(t=>t.SortId))
            {
                var rootNode = ToNode(root);
                AddNode(rootNode, root);
                _result.Add(rootNode);
            }

            return _result;
        }

        /// <summary>
        /// 是否根节点
        /// </summary>
        /// <param name="dto">树节点数据传输对象</param>
        protected virtual bool IsRoot(TNode dto)
        {
            if (_data.Any(t => t.ParentId.IsEmpty()))
            {
                return dto.ParentId.IsEmpty();
            }

            return dto.Level == _data.Min(t => t.Level);
        }

        /// <summary>
        /// 转换为结果节点
        /// </summary>
        /// <param name="dto">树节点数据传输对象</param>
        protected virtual ZorroTreeTableResult<TNode> ToNode(TNode dto)
        {
            return dto?.MapTo<ZorroTreeTableResult<TNode>>();
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="root">树形表格根节点</param>
        /// <param name="dto">树节点数据传输对象</param>
        private void AddNode(ZorroTreeTableResult<TNode> root, TNode dto)
        {
            if (root == null || dto == null)
            {
                return;
            }
            InitLeaf(dto);
            root.Children.Add(dto);
            var children = GetChildren(dto);
            foreach (var child in children)
            {
                AddNode(root, child);
            }
        }

        /// <summary>
        /// 初始化叶节点状态
        /// </summary>
        /// <param name="node">树节点数据传输对象</param>
        protected virtual void InitLeaf(TNode node)
        {
            node.Leaf = false;
            if (_async)
            {
                return;
            }

            if (IsLeaf(node))
            {
                node.Leaf = true;
            }
        }

        /// <summary>
        /// 是否叶节点
        /// </summary>
        /// <param name="node">树节点数据传输对象</param>
        protected virtual bool IsLeaf(TNode node)
        {
            if (node.Id.IsEmpty())
            {
                return true;
            }

            return _data.All(t => t.ParentId != node.Id);
        }

        /// <summary>
        /// 获取节点直接下级
        /// </summary>
        /// <param name="node">树节点数据传输对象</param>
        private List<TNode> GetChildren(TNode node)
        {
            return _data.Where(t => t.ParentId == node.Id).OrderBy(t => t.SortId).ToList();
        }
    }
}
