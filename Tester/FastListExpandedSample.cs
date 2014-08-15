using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FastTreeNS;

namespace Tester
{
    public partial class FastListExpandedSample : Form
    {
        private int expandedItemIndex = -1;

        public FastListExpandedSample()
        {
            InitializeComponent();

            fl.ItemCount = 100000;
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion

        private void fl_ItemExpandedStateChanged(object sender, FastTreeNS.ItemExpandedStateChangedEventArgs e)
        {
            expandedItemIndex = e.Expanded ? e.ItemIndex : -1;
            fl.BuildNeeded();
        }

        private void fl_ItemHeightNeeded(object sender, IntItemEventArgs e)
        {
            if (e.ItemIndex == expandedItemIndex)
                e.Result = 40;
            else
                e.Result = fl.ItemHeightDefault;
        }

        private void fl_PaintItemContent(object sender, PaintItemContentEventArgs e)
        {
            fl.DrawItemBackgound(e.Graphics, e.Info);
            fl.DrawItemIcons(e.Graphics, e.Info);

            if (e.Info.ItemIndex == expandedItemIndex)
            {
                //custom drawing
                var rect = new Rectangle(e.Info.X_Text, e.Info.Y, e.Info.X_End - e.Info.X_Text + 1, e.Info.Height);

                using (var brush = new SolidBrush(Color.PapayaWhip))
                    e.Graphics.FillRectangle(brush, rect);

                using (var pen = new Pen(Color.Navy))
                    e.Graphics.DrawRectangle(pen, rect);

                using (var brush = new SolidBrush(e.Info.ForeColor))
                {
                    var text = string.Format("It is expanded block\r\nItemIndex: {0}\r\nY: {1}    Height: {2}", e.Info.ItemIndex, e.Info.Y, e.Info.Height);
                    e.Graphics.DrawString(text, fl.Font, brush, rect);
                }
            }
            else
            {
                //default drawing
                e.Info.Text = "Item " + e.Info.ItemIndex;
                fl.DrawItemContent(e.Graphics, e.Info);
            }
        }

        private void fl_ItemExpandedNeeded(object sender, BoolItemEventArgs e)
        {
            e.Result = e.ItemIndex == expandedItemIndex;
        }
    }
}
