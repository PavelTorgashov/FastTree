using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tester.DataModel;

namespace Tester
{
    public partial class FastTreeDragAndDropSample : Form
    {
        private Node root;

        public FastTreeDragAndDropSample()
        {
            InitializeComponent();

            CreateTree();
            ft.Build(root);
        }

        private void CreateTree()
        {
            root = new Node("Root");
            for (int i = 0; i < 1000; i++)
            {
                var n = new Node("Node " + i);
                root.AddChild(n);
                for (int j = 0; j < 10; j++)
                {
                    var subNode = new Node("SubNode " + i + "-" + j);
                    n.AddChild(subNode);
                    for (int k = 0; k < 10; k++)
                    {
                        var subSubNode = new Node("SubNode " + i + "-" + j + "-" + k);
                        subNode.AddChild(subSubNode);
                    }
                }
            }
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion

        private void ft_DragOverNode(object sender, FastTreeNS.DragOverItemEventArgs e)
        {
            var draggedNodes = (e.Data.GetData(typeof(HashSet<object>)) as HashSet<object>).ToList().Cast<Node>();
            var targetNode = e.Tag as Node;

            //check if targetNode is child of draggedNodes
            foreach(var n in draggedNodes)
            if (targetNode.IsChildOf(n) || n == targetNode)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            //
            e.Effect = e.AllowedEffect;
            if(e.X > e.TextRect.Left +  50)
                e.InsertEffect = FastTreeNS.InsertEffect.AddAsChild;
            else
            if(e.Y < e.TextRect.Top + 10)
                e.InsertEffect = FastTreeNS.InsertEffect.InsertBefore;
            else
                e.InsertEffect = FastTreeNS.InsertEffect.InsertAfter;
        }

        private void ft_DropOverNode(object sender, FastTreeNS.DragOverItemEventArgs e)
        {
            var draggedNodes = (e.Data.GetData(typeof(HashSet<object>)) as HashSet<object>).ToList().Cast<Node>();
            var targetNode = e.Tag as Node;
            //
            switch(e.InsertEffect)
            {
                case FastTreeNS.InsertEffect.AddAsChild: AddChild(draggedNodes, targetNode); break;
                case FastTreeNS.InsertEffect.InsertBefore: InsertBefore(draggedNodes, targetNode); break;
                case FastTreeNS.InsertEffect.InsertAfter: InsertAfter(draggedNodes, targetNode); break;
            }

            ft.Build(root);
        }

        private void InsertAfter(IEnumerable<Node> draggedNodes, Node targetNode)
        {
            //build list of nodes without childs
            var list = ExcludeChild(draggedNodes);

            //inserting
            foreach(var n in list)
            if(n != targetNode)
            {
                n.Parent.RemoveChild(n);
                var index = targetNode.Parent.Childs.IndexOf(targetNode) + 1;
                targetNode.Parent.InsertChild(n, index);
            }
        }

        private void InsertBefore(IEnumerable<Node> draggedNodes, Node targetNode)
        {
            //build list of nodes without childs
            var list = ExcludeChild(draggedNodes);

            //inserting
            foreach (var n in list)
            if (n != targetNode)
            {
                n.Parent.RemoveChild(n);
                var index = targetNode.Parent.Childs.IndexOf(targetNode);
                targetNode.Parent.InsertChild(n, index);
            }
        }

        private void AddChild(IEnumerable<Node> draggedNodes, Node targetNode)
        {
            //build list of nodes without childs
            var list = ExcludeChild(draggedNodes);

            //change parent
            foreach (var n in list)
            {
                n.Parent.Childs.Remove(n);
                targetNode.AddChild(n);
            }
        }

        private static List<Node> ExcludeChild(IEnumerable<Node> draggedNodes)
        {
            //build list of nodes without childs
            var list = new List<Node>();
            foreach (var n in draggedNodes)
            {
                var was = false;
                foreach (var p in draggedNodes)
                    if (n.IsChildOf(p))
                    {
                        was = true;
                        break;
                    }
                if (!was)
                    list.Add(n);
            }
            return list;
        }
    }
}
