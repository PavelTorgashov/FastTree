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
    public partial class FastTreeEditSample : Form
    {
        private Node root;

        public FastTreeEditSample()
        {
            InitializeComponent();

            CreateTree();
            ft.Build(root);
        }

        private void CreateTree()
        {
            root = new Node("Root");
            for (int i = 0; i < 100; i++)
            {
                var n = new Node("Node " + i);
                root.AddChild(n);
                for (int j = 0; j < 100; j++)
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

        private void ft_NodeTextPushed(object sender, FastTreeNS.NodeTextPushedEventArgs e)
        {
            (e.Node as Node).Title = e.Text;
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion
    }
}
