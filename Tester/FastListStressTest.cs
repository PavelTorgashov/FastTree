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
    public partial class FastListStressTest : Form
    {
        public FastListStressTest()
        {
            InitializeComponent();
        }

        private void fl_ItemTextNeeded(object sender, FastTreeNS.StringItemEventArgs e)
        {
            e.Result = "Item " + e.ItemIndex;
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion
    }
}
