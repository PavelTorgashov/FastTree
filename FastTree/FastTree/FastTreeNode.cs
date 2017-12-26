using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastTreeNS
{
    /// <summary>
    /// General tree data model
    /// </summary>
    public class FastTreeNode
    {
        protected readonly List<FastTreeNode> childs = new List<FastTreeNode>();

        public object Tag { get; set; }

        private FastTreeNode parent;
        public FastTreeNode Parent
        {
            get { return parent; }
            set
            {
                if (parent == value)
                    return;

                SetParent(value);

                if(parent != null)
                    parent.childs.Add(this);
            }
        }

        protected virtual void SetParent(FastTreeNode value)
        {
            if (parent != null && parent != value)
                parent.childs.Remove(this);

            parent = value;
        }

        public virtual void RemoveNode(FastTreeNode node)
        {
            childs.Remove(node);
            SetParent(null);
        }

        public virtual void AddNode(FastTreeNode node)
        {
            if(node.Parent != this)
                childs.Add(node);
            SetParent(this);
        }

        public virtual void InsertNode(int index, FastTreeNode node)
        {
            childs.Insert(index, node);
            SetParent(this);
        }

        public virtual void InsertNodeBefore(FastTreeNode existsNode, FastTreeNode node)
        {
            var i = childs.IndexOf(existsNode);
            if (i < 0) i = 0;

            InsertNode(i, node);
        }

        public virtual void InsertNodeAfter(FastTreeNode existsNode, FastTreeNode node)
        {
            var i = childs.IndexOf(existsNode) + 1;
            InsertNode(i, node);
        }

        public virtual void RemoveNode(IEnumerable<FastTreeNode> nodes)
        {
            var hash = new HashSet<FastTreeNode>(nodes);
            var j = 0;
            for (int i = 0; i < childs.Count; i++)
            {
                if (hash.Contains(childs[i]))
                    j++;
                else
                    childs[i].SetParent(null);
                childs[i] = childs[i + j];
            }

            if(j > 0)
                childs.RemoveRange(childs.Count - j, j);
        }

        public virtual void AddNode(IEnumerable<FastTreeNode> nodes)
        {
            childs.AddRange(nodes);
            foreach(var node in nodes)
                node.SetParent(this);
        }

        public virtual void InsertNode(int index, IEnumerable<FastTreeNode> nodes)
        {
            childs.InsertRange(index, nodes);
            foreach (var node in nodes)
                node.SetParent(this);
        }

        public virtual void InsertNodeBefore(FastTreeNode existsNode,  IEnumerable<FastTreeNode> nodes)
        {
            var i = childs.IndexOf(existsNode);
            if (i < 0)
                i = 0;

            InsertNode(i, nodes);
        }

        public virtual void InsertNodeAfter(FastTreeNode existsNode, IEnumerable<FastTreeNode> nodes)
        {
            var i = childs.IndexOf(existsNode) + 1;
            InsertNode(i, nodes);
        }

        public int IndexOf(FastTreeNode node)
        {
            return childs.IndexOf(node);
        }

        public IEnumerable<FastTreeNode> Childs
        {
            get 
            {
                return childs;
            }
        }

        public IEnumerable<FastTreeNode> GetChilds<TagType>()
        {
            return GetChilds(t=>t is TagType);
        }

        public IEnumerable<FastTreeNode> GetChilds(Predicate<object> tagCondition)
        {
            return childs.Where(c=>tagCondition(c.Tag));
        }

        public IEnumerable<FastTreeNode> AllChilds
        {
            get
            {
                yield return this;

                foreach(var c in childs)
                    foreach (var cc in c.AllChilds)
                        yield return cc;
            }
        }

        public IEnumerable<FastTreeNode> GetAllChilds<TagType>()
        {
            return GetAllChilds(t => t is TagType);
        }

        public IEnumerable<FastTreeNode> GetAllChilds(Predicate<object> tagCondition)
        {
            return AllChilds.Where(c => tagCondition(c.Tag));
        }

        public FastTreeNode GetParent<TagType>()
        {
            return GetParent(t => t is TagType);
        }

        public FastTreeNode GetParent(Predicate<object> tagCondition)
        {
            var parent = Parent;
            while (parent != null && !tagCondition(parent))
                parent = parent.parent;
            return parent;
        }
    }
}
