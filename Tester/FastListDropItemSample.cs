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
    public partial class FastListDropItemSample : Form
    {
        private List<string> list;

        public FastListDropItemSample()
        {
            InitializeComponent();

            CreateList();

            fl.ItemCount = list.Count;
        }

        private void CreateList()
        {
            list = new List<string>();

            for (int i = 0; i < 1000; i++)
                list.Add("Item " + i);
        }

        private void fl_ItemTextNeeded(object sender, FastTreeNS.StringItemEventArgs e)
        {
            e.Result = list[e.ItemIndex];
        }

        private void lb_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                (sender as Label).DoDragDrop(sender, DragDropEffects.All);
        }

        private void fl_DragOverItem(object sender, DragOverItemEventArgs e)
        {
            var lb = e.Data.GetData(typeof (Label)) as Label;

            if (lb == lbRegular)
            {
                e.Effect = e.AllowedEffect;
                return;
            }

            if (lb == lbAfter13 && list[e.ItemIndex] == "Item 13")
            {
                e.Effect = e.AllowedEffect;
                e.InsertEffect = InsertEffect.InsertAfter;
                return;
            }

            if (lb == lbEndOfList)
            {
                e.Effect = e.AllowedEffect;
                e.InsertEffect = InsertEffect.InsertAfter;
                e.ItemIndex = list.Count - 1;
                return;
            }

            if (lb == lbReplace)
            {
                e.Effect = DragDropEffects.Move;
                e.InsertEffect = InsertEffect.Replace;
                return;
            }

            e.Effect = DragDropEffects.None;
        }

        private void fl_DropOverItem(object sender, DragOverItemEventArgs e)
        {
            var text = (e.Data.GetData(typeof(Label)) as Label).Text;

            switch(e.InsertEffect)
            {
                case InsertEffect.InsertBefore:
                    list.Insert(e.ItemIndex, text);
                    fl.ItemCount = list.Count;
                    break;
                case InsertEffect.InsertAfter:
                    list.Insert(e.ItemIndex + 1, text);
                    fl.ItemCount = list.Count;
                    break;
                case InsertEffect.Replace:
                    if (MessageBox.Show("Are you sure to replace item?", "Replace", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        list[e.ItemIndex] = text;
                        fl.Invalidate();
                    }
                    break;
            }
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion
    }
}
