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
    public partial class FastListVirtualCheckboxesSample : Form
    {
        private List<MyItem> list;

        public FastListVirtualCheckboxesSample()
        {
            InitializeComponent();

            CreateList();

            fl.ItemCount = list.Count;
        }

        private void CreateList()
        {
            list = new List<MyItem>();

            for (int i = 0; i < 100000; i++)
                list.Add(new MyItem{Name = "Item " + i, Checked = (i % 3) == 0});
        }

        private void fl_ItemTextNeeded(object sender, FastTreeNS.StringItemEventArgs e)
        {
            e.Result = list[e.ItemIndex].Name;
        }

        private void fl_ItemCheckStateNeeded(object sender, FastTreeNS.BoolItemEventArgs e)
        {
            e.Result = list[e.ItemIndex].Checked;
        }

        private void fl_ItemCheckedStateChanged(object sender, FastTreeNS.ItemCheckedStateChangedEventArgs e)
        {
            list[e.ItemIndex].Checked = e.Checked;
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion
    }

    class MyItem
    {
        public string Name;
        public bool Checked;
    }
}
