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
    public partial class FastListCustomize : Form
    {
        public FastListCustomize()
        {
            InitializeComponent();

            Application.Idle += (o, e) => fl.Invalidate();
        }

        private void fl_ItemTextNeeded(object sender, FastTreeNS.StringItemEventArgs e)
        {
            if(e.ItemIndex == 0)
                e.Result = "Time " + DateTime.Now.TimeOfDay;
            else
                e.Result = "Item " + e.ItemIndex;
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion

        private void fl_ItemIndentNeeded(object sender, FastTreeNS.IntItemEventArgs e)
        {
            if (e.ItemIndex == 5)
                e.Result = 30;
        }

        private void fl_ItemHeightNeeded(object sender, FastTreeNS.IntItemEventArgs e)
        {
            if (e.ItemIndex == 10)
                e.Result = 40;
        }

        private void fl_ItemForeColorNeeded(object sender, FastTreeNS.ColorItemEventArgs e)
        {
            if (e.ItemIndex == 1)
                e.Result = Color.Red;
            if (e.ItemIndex == 10)
                e.Result = Color.White;
        }

        private void fl_ItemBackColorNeeded(object sender, FastTreeNS.ColorItemEventArgs e)
        {
            if (e.ItemIndex == 10)
                e.Result = Color.SteelBlue;
            if(e.ItemIndex == 8)
                e.Result = DateTime.Now.Millisecond > 500 ? Color.Transparent : Color.Tomato;
        }

        private void fl_ItemIconNeeded(object sender, FastTreeNS.ImageItemEventArgs e)
        {
            if(e.ItemIndex == 1)
                e.Result = DateTime.Now.Millisecond > 500 ? null : imageList1.Images[0];
        }

        private void fl_PaintItem(object sender, FastTreeNS.PaintItemContentEventArgs e)
        {
            if (e.Info.ItemIndex > 12 && e.Info.ItemIndex < 16)
            {
                //draw default background
                fl.DrawItemBackgound(e.Graphics, e.Info);
                //draw default selection
                if (fl.SelectedItemIndexes.Contains(e.Info.ItemIndex))
                    fl.DrawSelection(e.Graphics, e.Info);
                //draw icon
                fl.DrawItemIcons(e.Graphics, e.Info);
                //draw custom bold text
                using (var font = new Font(Font, FontStyle.Bold))
                    e.Graphics.DrawString(e.Info.Text, font, Brushes.Black, e.Info.TextRect);
            }else
            if (e.Info.ItemIndex == 18)
            {
                //draw whole item
                fl.DrawItemWhole(e.Graphics, e.Info);
                //draw border
                e.Graphics.DrawRectangle(Pens.Gray, e.Info.TextAndIconRect);
            }else
            if (e.Info.ItemIndex == 3)
            {
                //draw whole item
                fl.DrawItemWhole(e.Graphics, e.Info);
                //draw second icon
                e.Graphics.DrawImage(imageList1.Images[1], new Point(e.Info.X - 16, e.Info.Rect.Top));
            }
            else
            //default drawing
            fl.DrawItemWhole(e.Graphics, e.Info);
        }
    }
}
