//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  License: GNU Lesser General Public License (LGPLv3)
//
//  Email: pavel_torgashov@ukr.net.
//
//  Copyright (C) Pavel Torgashov, 2014. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FastTreeNS
{
    [ToolboxItem(true)]
    [DefaultEvent("NodeChildrenNeeded")]
    public class FastTree : FastListBase
    {
        [DefaultValue(true)]
        public bool AutoCollapse { get; set; }

        [DefaultValue(false)]
        public bool ShowRootNode { get; set; }

        [Browsable(false)]
        public IEnumerable<object> ExpandedNodes { get { return expandedNodes; } }

        [Browsable(false)]
        public IEnumerable<object> SelectedNodes { get { return SelectedItemIndex.OrderBy(i => i).Select(i => nodes[i]); } }

        [Browsable(false)]
        public IEnumerable<object> CheckedNodes { get { return CheckedItemIndex.OrderBy(i => i).Select(i => nodes[i]); } }

        /// <summary>
        /// List of all visible nodes
        /// </summary>
        [Browsable(false)]
        public IEnumerable<object> Nodes { get { return nodes; } }

        protected List<object> nodes = new List<object>();
        protected List<int> levels = new List<int>();
        protected HashSet<object> expandedNodes = new HashSet<object>();
        protected BitArray hasChildren;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int ItemCount
        {
            get { return base.ItemCount; }
            set { base.ItemCount = value; }
        }

        [DefaultValue(16)]
        public override int ItemIndentDefault
        {
            get { return base.ItemIndentDefault; }
            set { base.ItemIndentDefault = value; }
        }

        public FastTree()
        {
            AutoCollapse = true;
            ShowRootNode = false;
            ShowExpandBoxes = true;
            ItemIndentDefault = 16;

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                Build(new object[] { "Node 1", "Node 2", "Node 3" });
                SelectedItemIndex.Add(0);
            }
        }

        #region Events

        public event EventHandler<IntNodeEventArgs> NodeHeightNeeded;
        public event EventHandler<StringNodeEventArgs> NodeTextNeeded;
        public event EventHandler<BoolNodeEventArgs> NodeCheckStateNeeded;
        public event EventHandler<ImageNodeEventArgs> NodeIconNeeded;
        public event EventHandler<BoolNodeEventArgs> NodeCheckBoxVisibleNeeded;
        public event EventHandler<ColorNodeEventArgs> NodeBackColorNeeded;
        public event EventHandler<IntNodeEventArgs> NodeIndentNeeded;
        public event EventHandler<ColorNodeEventArgs> NodeForeColorNeeded;
        public event EventHandler<BoolNodeEventArgs> NodeVisibilityNeeded;
        public event EventHandler<BoolNodeEventArgs> CanUnselectNodeNeeded;
        public event EventHandler<BoolNodeEventArgs> CanSelectNodeNeeded;
        public event EventHandler<BoolNodeEventArgs> CanUncheckNodeNeeded;
        public event EventHandler<BoolNodeEventArgs> CanCheckNodeNeeded;
        public event EventHandler<BoolNodeEventArgs> CanExpandNodeNeeded;
        public event EventHandler<BoolNodeEventArgs> CanCollapseNodeNeeded;
        
        public event EventHandler<NodeCheckedStateChangedEventArgs> NodeCheckedStateChanged;
        public event EventHandler<NodeExpandedStateChangedEventArgs> NodeExpandedStateChanged;
        public event EventHandler<NodeSelectedStateChangedEventArgs> NodeSelectedStateChanged;
        
        public event EventHandler<PaintNodeContentEventArgs> PaintNode;

        public event EventHandler<NodeChildrenNeededEventArgs> NodeChildrenNeeded;

        /// <summary>
        /// Occurs when user start to drag node
        /// </summary>
        public event EventHandler<NodeDragEventArgs> NodeDrag;

        /// <summary>
        /// Occurs when user drag object over node
        /// </summary>
        public event EventHandler<DragOverItemEventArgs> DragOverNode;

        /// <summary>
        /// Occurs when user drop object on given node
        /// </summary>
        public event EventHandler<DragOverItemEventArgs> DropOverNode;

        protected virtual bool GetNodeVisibility(object node)
        {
            if (NodeVisibilityNeeded != null)
            {
                boolArg.Node = node;
                boolArg.Result = true;
                NodeVisibilityNeeded(this, boolArg);
                return boolArg.Result;
            }
            return true;
        }

        protected override string GetItemText(int itemIndex)
        {
            return GetStringNodeProperty(itemIndex, NodeTextNeeded, nodes[itemIndex].ToString());
        }

        protected override bool GetItemChecked(int itemIndex)
        {
            return GetBoolNodeProperty(itemIndex, NodeCheckStateNeeded, CheckedItemIndex.Contains(itemIndex));
        }

        protected override Image GetItemIcon(int itemIndex)
        {
            return GetImageNodeProperty(itemIndex, NodeIconNeeded, ImageDefaultIcon);
        }

        protected override int GetItemHeight(int itemIndex)
        {
            return GetIntNodeProperty(itemIndex, NodeHeightNeeded, ItemHeightDefault);
        }

        protected override bool GetItemCheckBoxVisible(int itemIndex)
        {
            return GetBoolNodeProperty(itemIndex, NodeCheckBoxVisibleNeeded, ShowCheckBoxes);
        }

        protected override Color GetItemBackColor(int itemIndex)
        {
            return GetColorNodeProperty(itemIndex, NodeBackColorNeeded, Color.Empty);
        }

        protected override Color GetItemForeColor(int itemIndex)
        {
            return GetColorNodeProperty(itemIndex, NodeForeColorNeeded, ForeColor);
        }

        protected override bool CanUnselectItem(int itemIndex)
        {
            return GetBoolNodeProperty(itemIndex, CanUnselectNodeNeeded, true);
        }

        protected override bool CanSelectItem(int itemIndex)
        {
            return GetBoolNodeProperty(itemIndex, CanSelectNodeNeeded, true);
        }

        protected override bool CanUncheckItem(int itemIndex)
        {
            return GetBoolNodeProperty(itemIndex, CanUncheckNodeNeeded, true);
        }

        protected override bool CanCheckItem(int itemIndex)
        {
            return GetBoolNodeProperty(itemIndex, CanCheckNodeNeeded, true);
        }

        protected override bool CanCollapseItem(int itemIndex)
        {
            return GetBoolNodeProperty(itemIndex, CanCollapseNodeNeeded, true);
        }

        protected override void OnItemChecked(int itemIndex)
        {
            if (NodeCheckedStateChanged != null)
                NodeCheckedStateChanged(this, new NodeCheckedStateChangedEventArgs { Node = nodes[itemIndex], Checked = true });

            base.OnItemChecked(itemIndex);
        }

        protected override void OnItemUnchecked(int itemIndex)
        {
            if (NodeCheckedStateChanged != null)
                NodeCheckedStateChanged(this, new NodeCheckedStateChangedEventArgs { Node = nodes[itemIndex], Checked = false });

            base.OnItemUnchecked(itemIndex);
        }

        protected override void OnItemExpanded(int itemIndex)
        {
            OnNodeExpanded(nodes[itemIndex]);
            base.OnItemExpanded(itemIndex);
        }

        protected virtual void OnNodeExpanded(object node)
        {
            if (NodeExpandedStateChanged != null)
                NodeExpandedStateChanged(this, new NodeExpandedStateChangedEventArgs { Node = node, Expanded = true });
        }

        protected override void OnItemCollapsed(int itemIndex)
        {
            if (NodeExpandedStateChanged != null)
                NodeExpandedStateChanged(this, new NodeExpandedStateChangedEventArgs { Node = nodes[itemIndex], Expanded = false });

            base.OnItemCollapsed(itemIndex);
        }

        protected override void OnItemSelected(int itemIndex)
        {
            if (NodeSelectedStateChanged != null)
                NodeSelectedStateChanged(this, new NodeSelectedStateChangedEventArgs { Node = nodes[itemIndex], Selected = true });

            base.OnItemSelected(itemIndex);
        }

        protected override void OnItemUnselected(int itemIndex)
        {
            if (NodeSelectedStateChanged != null)
                NodeSelectedStateChanged(this, new NodeSelectedStateChangedEventArgs { Node = nodes[itemIndex], Selected = false });

            base.OnItemUnselected(itemIndex);
        }

        protected override void OnItemDrag(HashSet<int> itemIndex)
        {
            var nodes = new HashSet<object>(itemIndex.Select(i => this.nodes[i]));

            if (NodeDrag != null)
                NodeDrag(this, new NodeDragEventArgs {Nodes = nodes});
            else
                DoDragDrop(nodes, DragDropEffects.Copy);

            base.OnItemDrag(itemIndex);
        }

        protected override void DrawItem(Graphics gr, VisibleItemInfo info)
        {
            if (PaintNode != null)
                PaintNode(this, new PaintNodeContentEventArgs { Graphics = gr, Info = info, Node = nodes[info.ItemIndex]});
            else
                base.DrawItem(gr, info);
        }

        protected override void OnDragOverItem(DragOverItemEventArgs e)
        {
            base.OnDragOverItem(e);

            e.Tag = nodes[e.ItemIndex];

            if (DragOverNode != null)
                DragOverNode(this, e);
        }

        protected override void OnDropOverItem(DragOverItemEventArgs e)
        {
            e.Tag = nodes[e.ItemIndex];

            if (DropOverNode != null)
                DropOverNode(this, e);

            base.OnDropOverItem(e);
        }

        #endregion Events

        #region Build

        protected object root;

        public void Build(object root)
        {
            this.root = root;
            //create set of selected and checked nodes
            var selected = new HashSet<object>();
            var check = new HashSet<object>();

            foreach (var i in SelectedItemIndex)
                selected.Add(nodes[i]);

            foreach (var i in CheckedItemIndex)
                check.Add(nodes[i]);

            //
            nodes.Clear();
            levels.Clear();
            SelectedItemIndex.Clear();
            CheckedItemIndex.Clear();

            //build list of expanded nodes
            if (ShowRootNode)
                AddNode(root, 0);
            else
                AddNodeChildren(root, 0);
            //restore indexes of selected and checked nodes
            var newExpanded = new HashSet<object>();
            hasChildren = new BitArray(nodes.Count);

            for (int i = 0; i < nodes.Count;i++)
            {
                var node = nodes[i];
                if (selected.Contains(node)) SelectedItemIndex.Add(i);
                if (check.Contains(node)) CheckedItemIndex.Add(i);
                if (expandedNodes.Contains(node)) newExpanded.Add(node);
                hasChildren[i] = GetNodeChildren(nodes[i]).Cast<object>().Any();
            }

            expandedNodes = newExpanded;
            ItemCount = nodes.Count;
            base.Build();
        }

        private void AddNode(object node, int level)
        {
            if (node == null || !GetNodeVisibility(node))
                return;
            //
            nodes.Add(node);
            levels.Add(level);
            //
            if (expandedNodes.Contains(node))
                AddNodeChildren(node, level + 1);
        }

        private void AddNodeChildren(object node, int level)
        {
            foreach (var child in GetNodeChildren(node))
                AddNode(child, level);
        }

        protected virtual IEnumerable GetNodeChildren(object node)
        {
            if(NodeChildrenNeeded != null)
            {
                var arg = new NodeChildrenNeededEventArgs() {Node = node};
                NodeChildrenNeeded(this, arg);
                if (arg.Children != null)
                    foreach (var child in arg.Children)
                        yield return child;
            }else
            if(node is IEnumerable)
            {
                foreach (var child in node as IEnumerable)
                    yield return child;
            }
        }

        #endregion Build

        #region Additional methods

        public virtual object GetNodeByIndex(int index)
        {
            if (index < 0 || index >= nodes.Count) return null;
            return nodes[index];
        }

        public override bool ExpandItem(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= nodes.Count)
                return false;

            var list = GetNodeChildren(nodes[itemIndex]).Cast<object>().ToList();
            if(list.Count > 0)
            if(CanExpandItem(itemIndex))
            {
                expandedNodes.Add(nodes[itemIndex]);
                Build(root);
                if (itemIndex < nodes.Count)
                    OnItemExpanded(itemIndex);
                return true;
            }

            return false;
        }

        public override bool CollapseItem(int itemIndex)
        {
            return CollapseItem(itemIndex, true);
        }

        protected virtual bool CollapseItem(int itemIndex, bool build)
        {
            if (itemIndex < 0 || itemIndex >= nodes.Count)
                return false;

            if (CanCollapseItem(itemIndex))
            {
                //range of collapsing nodes
                var i = itemIndex + 1;
                var level = levels[itemIndex];
                while (i < nodes.Count && levels[i] > level)
                    i++;

                var from = itemIndex + 1;
                var to = i - 1;
                if (to < from)
                    return true;

                //check selection, checked
                foreach (var j in SelectedItemIndex)
                    if (j >= from && j <= to)
                        if (!CanUnselectItem(j))
                            return false;

                foreach (var j in CheckedItemIndex)
                    if (j >= from && j <= to)
                        if (!CanUncheckItem(j))
                            return false;

                for (var j = from; j <= to; j++)
                    if (expandedNodes.Contains(nodes[j]))
                        if (!CanCollapseItem(j))
                            return false;

                //unselect, uncheck
                for (int j = from; j <= to; j++)
                {
                    UnselectItem(j);
                    UncheckItem(j);
                    if (expandedNodes.Contains(nodes[j]))
                    {
                        expandedNodes.Remove(nodes[j]);
                        OnItemCollapsed(j);
                    }
                }
                //remove
                expandedNodes.Remove(nodes[itemIndex]);
                //
                OnItemCollapsed(itemIndex);
                if (build)
                    Build(root);
                return true;
            }

            return false;
        }

        public virtual bool UnselectNode(object node)
        {
            return base.UnselectItem(nodes.IndexOf(node));
        }

        public virtual bool SelectNode(object node, bool unselectOtherItems = true)
        {
            return base.SelectItem(nodes.IndexOf(node), unselectOtherItems);
        }

        public virtual bool UncheckNode(object node)
        {
            return base.UncheckItem(nodes.IndexOf(node));
        }

        public virtual bool CheckNode(object node)
        {
            return base.CheckItem(nodes.IndexOf(node));
        }

        public virtual bool IsNodeSelected(object node)
        {
            return SelectedItemIndex.Contains(nodes.IndexOf(node));
        }

        public virtual bool IsNodeChecked(object node)
        {
            return GetItemChecked(nodes.IndexOf(node));
        }

        public virtual bool IsNodeExpanded(object node)
        {
            return expandedNodes.Contains(node);
        }

        private bool ExpandNodeAndChildren(object node, int maxExpandLevelCount)
        {
            var list = GetNodeChildren(node).Cast<object>().ToList();
            if (list.Count > 0)
                if (CanExpandNode(node))
                {
                    expandedNodes.Add(node);
                    OnNodeExpanded(node);

                    if (maxExpandLevelCount > 1)
                    foreach (var child in list)
                        ExpandNodeAndChildren(child, maxExpandLevelCount - 1);

                    return true;
                }

            return false;
        }

        public bool ExpandNode(object node, bool expandChildren = false)
        {
            if (expandChildren)
            {
                if(ExpandNodeAndChildren(node, int.MaxValue))
                {
                    Build(root);
                    return true;
                }
                return false;
            }
            else
                return ExpandItem(nodes.IndexOf(node));
        }

        public bool ExpandNode(object node, int maxExpandLevelCount)
        {
            if (maxExpandLevelCount > 1)
            {
                if (ExpandNodeAndChildren(node, maxExpandLevelCount))
                {
                    Build(root);
                    return true;
                }
                return false;
            }
            else
                return ExpandItem(nodes.IndexOf(node));
        }

        public bool CollapseNode(object node)
        {
            return CollapseItem(nodes.IndexOf(node));
        }

        public void ExpandAll()
        {
            if (ShowRootNode)
                ExpandNodeAndChildren(root, int.MaxValue);
            else
                foreach (var child in GetNodeChildren(root))
                    ExpandNodeAndChildren(child, int.MaxValue);

            Build(root);
        }

        public bool CollapseAll()
        {
            var res = true;

            for (int i = nodes.Count - 1; i >= 0; i--)
            if (expandedNodes.Contains(nodes[i]))
                res &= CollapseItem(i, false);

            Build(root);

            return res;
        }

        /// <summary>
        /// Returns all expanded children of the node
        /// </summary>
        public virtual IEnumerable GetNodeExpandedChildren(object node, bool onlyFirstLevel = false)
        {
            var itemIndex = nodes.IndexOf(node);
            if (itemIndex < 0) 
                yield break;

            foreach (var i in GetItemExpandedChildren(itemIndex, onlyFirstLevel))
                yield return nodes[i];
        }

        /// <summary>
        /// Returns all expanded children of the item
        /// </summary>
        public virtual IEnumerable<int> GetItemExpandedChildren(int itemIndex, bool onlyFirstLevel = false)
        {
            var i = itemIndex + 1;
            var level = levels[itemIndex];
            while (i < nodes.Count && levels[i] > level)
            {
                if ((!onlyFirstLevel) || (levels[i] == level + 1))
                    yield return i;
                i++;
            }
        }

        public int GetItemIndexOfNode(object node)
        {
            return nodes.IndexOf(node);
        }

        #endregion

        #region Overrided methods

        protected override int GetItemIndent(int itemIndex)
        {
            return GetIntNodeProperty(itemIndex, NodeIndentNeeded, levels[itemIndex] * ItemIndentDefault);
        }

        protected override bool GetItemExpanded(int itemIndex)
        {
            return expandedNodes.Contains(nodes[itemIndex]);
        }

        protected override bool CanExpandItem(int itemIndex)
        {
            return GetBoolNodeProperty(itemIndex, CanExpandNodeNeeded, hasChildren[itemIndex]);
        }

        protected virtual bool CanExpandNode(object node)
        {
            if (CanExpandNodeNeeded != null)
            {
                boolArg.Node = node;
                boolArg.Result = true;
                CanExpandNodeNeeded(this, boolArg);
                return boolArg.Result;
            }

            return true;
        }

        public override bool CheckItem(int itemIndex)
        {
            if (GetItemChecked(itemIndex))
                return true;

            Invalidate();

            if (CanCheckItem(itemIndex))
            {
                if (NodeCheckStateNeeded == null)//add to CheckedItemIndex only if handler of NodeCheckStateNeeded is not assigned
                    CheckedItemIndex.Add(itemIndex);
                OnItemChecked(itemIndex);
                return true;
            }

            return false;
        }

        protected override bool IsItemHeightFixed
        {
            get
            {
                return NodeHeightNeeded == null;
            }
        }

        #endregion

        #region Event Helpers

        private IntNodeEventArgs intArg = new IntNodeEventArgs();
        private BoolNodeEventArgs boolArg = new BoolNodeEventArgs();
        private StringNodeEventArgs stringArg = new StringNodeEventArgs();
        private ImageNodeEventArgs imageArg = new ImageNodeEventArgs();
        private ColorNodeEventArgs colorArg = new ColorNodeEventArgs();

        int GetIntNodeProperty(int itemIndex, EventHandler<IntNodeEventArgs> handler, int defaultValue)
        {
            if (handler != null)
            {
                intArg.Node = nodes[itemIndex];
                intArg.Result = defaultValue;
                handler(this, intArg);
                return intArg.Result;
            }

            return defaultValue;
        }

        string GetStringNodeProperty(int itemIndex, EventHandler<StringNodeEventArgs> handler, string defaultValue)
        {
            if (handler != null)
            {
                stringArg.Node = nodes[itemIndex];
                stringArg.Result = defaultValue;
                handler(this, stringArg);
                return stringArg.Result;
            }

            return defaultValue;
        }

        bool GetBoolNodeProperty(int itemIndex, EventHandler<BoolNodeEventArgs> handler, bool defaultValue)
        {
            if (handler != null)
            {
                boolArg.Node = nodes[itemIndex];
                boolArg.Result = defaultValue;
                handler(this, boolArg);
                return boolArg.Result;
            }

            return defaultValue;
        }

        Image GetImageNodeProperty(int itemIndex, EventHandler<ImageNodeEventArgs> handler, Image defaultValue)
        {
            if (handler != null)
            {
                imageArg.Node = nodes[itemIndex];
                imageArg.Result = defaultValue;
                handler(this, imageArg);
                return imageArg.Result;
            }

            return defaultValue;
        }

        Color GetColorNodeProperty(int itemIndex, EventHandler<ColorNodeEventArgs> handler, Color defaultValue)
        {
            if (handler != null)
            {
                colorArg.Node = nodes[itemIndex];
                colorArg.Result = defaultValue;
                handler(this, colorArg);
                return colorArg.Result;
            }

            return defaultValue;
        }

        #endregion Helpers
    }

    public class GenericNodeResultEventArgs<T> : EventArgs
    {
        public object Node { get; internal set; }
        public T Result;
    }

    public class IntNodeEventArgs : GenericNodeResultEventArgs<int>
    {
    }

    public class StringNodeEventArgs : GenericNodeResultEventArgs<string>
    {
    }

    public class ImageNodeEventArgs : GenericNodeResultEventArgs<Image>
    {
    }

    public class ColorNodeEventArgs : GenericNodeResultEventArgs<Color>
    {
    }

    public class BoolNodeEventArgs : GenericNodeResultEventArgs<bool>
    {
    }

    public class NodeCheckedStateChangedEventArgs : EventArgs
    {
        public object Node;
        public bool Checked;
    }

    public class NodeExpandedStateChangedEventArgs : EventArgs
    {
        public object Node;
        public bool Expanded;
    }

    public class NodeSelectedStateChangedEventArgs : EventArgs
    {
        public object Node;
        public bool Selected;
    }

    public class PaintNodeContentEventArgs : EventArgs
    {
        public Graphics Graphics;
        public FastListBase.VisibleItemInfo Info;
        public object Node;
    }

    public class NodeDragEventArgs : EventArgs
    {
        public HashSet<object> Nodes;
    }

    public class NodeChildrenNeededEventArgs : EventArgs
    {
        public object Node { get; internal set; }
        public IEnumerable Children { get; set; }
    }
}
