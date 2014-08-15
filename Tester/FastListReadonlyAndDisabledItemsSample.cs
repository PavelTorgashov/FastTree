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
    public partial class FastListReadonlyAndDisabledItemsSample : Form
    {
        public FastListReadonlyAndDisabledItemsSample()
        {
            InitializeComponent();

            fl.ItemCount = 1000000;
        }

        private void fl_ItemTextNeeded(object sender, FastTreeNS.StringItemEventArgs e)
        {
            switch(e.ItemIndex % 20)
            {
                case 5: e.Result = "Readonly item " + e.ItemIndex + ". You can not check/uncheck this item."; break;
                case 15: e.Result = "Disabled item " + e.ItemIndex + ". You can not select this item."; break;
                default: e.Result = "Regular item " + e.ItemIndex; break;
            }
        }

        private void fl_CanCheckItemNeeded(object sender, FastTreeNS.BoolItemEventArgs e)
        {
            switch (e.ItemIndex % 20)
            {
                case 5: e.Result = false; break;
                case 15: e.Result = false; break;
            }
        }

        private void fl_CanSelectItemNeeded(object sender, FastTreeNS.BoolItemEventArgs e)
        {
            switch (e.ItemIndex % 20)
            {
                case 15: e.Result = false; break;
            }
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion

        private void fl_ItemForeColorNeeded(object sender, FastTreeNS.ColorItemEventArgs e)
        {
            switch (e.ItemIndex % 20)
            {
                case 5: e.Result = Color.Red; break;
                case 15: e.Result = Color.Silver; break;
            }
        }
    }
}
