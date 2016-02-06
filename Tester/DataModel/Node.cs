using System.Collections.Generic;

namespace Tester.DataModel
{
    /// <summary>
    /// Warning!!!
    /// This class is needed only for demonstration of features of FastTree.
    /// In real life you need to use own class of your data model.
    /// </summary>
    public class Node : IEnumerable<Node>
    {
        public string Title { get; set; }
        public List<Node> Childs { get; private set; }
        public Node Parent { get; set; }

        public Node(string title = null)
        {
            Title = title;
            Childs = new List<Node>();
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return Childs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Childs.GetEnumerator();
        }

        public override string ToString()
        {
            return Title;
        }

        public bool IsChildOf(Node parent)
        {
            return parent == Parent || (Parent != null && Parent.IsChildOf(parent));
        }

        public void AddChild(Node child)
        {
            Childs.Add(child);
            child.Parent = this;
        }

        public void InsertChild(Node child, int index)
        {
            Childs.Insert(index, child);
            child.Parent = this;
        }

        public void RemoveChild(Node child)
        {
            Childs.Remove(child);
            child.Parent = null;
        }
    }
}
