using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester
{
    public partial class FastListDragItemSample : Form
    {
        public FastListDragItemSample()
        {
            InitializeComponent();

            fl.ItemCount = 1000000;
        }

        private void fl_ItemTextNeeded(object sender, FastTreeNS.StringItemEventArgs e)
        {
            e.Result = "Item " + e.ItemIndex;
        }

        private void fl_ItemDrag(object sender, FastTreeNS.ItemDragEventArgs e)
        {
            fl.DoDragDrop(e.ItemIndex, DragDropEffects.Copy);
        }

        private void lb_DragDrop(object sender, DragEventArgs e)
        {
            var itemIndex = e.Data.GetData(typeof (HashSet<int>)) as HashSet<int>;

            lb.Text = itemIndex.Aggregate("", (s, i) => s + i + ";");
        }

        private void lb_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion
    }
}
