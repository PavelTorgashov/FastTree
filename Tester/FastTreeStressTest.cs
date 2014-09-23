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
    public partial class FastTreeStressTest : Form
    {
        public FastTreeStressTest()
        {
            InitializeComponent();

            ft.Build("Node");
        }

        private void ft_NodeChildrenNeeded(object sender, FastTreeNS.NodeChildrenNeededEventArgs e)
        {
            e.Children = GenerateNodes(e.Node as string, 100000);
        }

        IEnumerable<string> GenerateNodes(string parent, int count)
        {
            for (int i = 0; i < count; i++)
                yield return string.Concat(parent, "_", i);
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion
    }
}
