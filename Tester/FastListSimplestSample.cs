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
    public partial class FastListSimplestSample : Form
    {
        private List<string> list;

        public FastListSimplestSample()
        {
            InitializeComponent();

            CreateList();

            fl.ItemCount = list.Count;
        }

        private void CreateList()
        {
            list = new List<string>();

            for (int i = 0; i < 1000000; i++)
                list.Add("Item " + i);
        }

        private void fl_ItemTextNeeded(object sender, FastTreeNS.StringItemEventArgs e)
        {
            e.Result = list[e.ItemIndex];
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion
    }
}
