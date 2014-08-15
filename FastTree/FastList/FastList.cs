//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  License: GNU Lesser General Public License (LGPLv3)
//
//  Email: pavel_torgashov@ukr.net.
//
//  Copyright (C) Pavel Torgashov, 2014. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FastTreeNS
{
    [DefaultEvent("ItemTextNeeded")]
    [ToolboxItem(true)]
    public class FastList : FastListBase
    {
        public FastList()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                ItemCount = 100;
                ItemTextNeeded += (o, a) => a.Result = "Item " + a.ItemIndex;
                SelectedItemIndex.Add(0);
            }
        }

        #region Events

        public event EventHandler<IntItemEventArgs> ItemHeightNeeded;
        public event EventHandler<IntItemEventArgs> ItemIndentNeeded;
        public event EventHandler<StringItemEventArgs> ItemTextNeeded;
        public event EventHandler<ImageItemEventArgs> ItemIconNeeded;
        public event EventHandler<BoolItemEventArgs> ItemCheckBoxVisibleNeeded;
        public event EventHandler<ColorItemEventArgs> ItemBackColorNeeded;
        public event EventHandler<ColorItemEventArgs> ItemForeColorNeeded;
        public event EventHandler<BoolItemEventArgs> ItemExpandedNeeded;
        public event EventHandler<BoolItemEventArgs> CanUnselectItemNeeded;
        public event EventHandler<BoolItemEventArgs> CanSelectItemNeeded;
        public event EventHandler<BoolItemEventArgs> CanUncheckItemNeeded;
        public event EventHandler<BoolItemEventArgs> CanCheckItemNeeded;
        public event EventHandler<BoolItemEventArgs> CanExpandItemNeeded;
        public event EventHandler<BoolItemEventArgs> CanCollapseItemNeeded;

        public event EventHandler<ItemCheckedStateChangedEventArgs> ItemCheckedStateChanged;
        public event EventHandler<ItemExpandedStateChangedEventArgs> ItemExpandedStateChanged;
        public event EventHandler<ItemSelectedStateChangedEventArgs> ItemSelectedStateChanged;

        public event EventHandler<PaintItemContentEventArgs> PaintItem;

        public event EventHandler<ItemDragEventArgs> ItemDrag;
        public event EventHandler<DragOverItemEventArgs> DragOverItem;
        public event EventHandler<DragOverItemEventArgs> DragDropOverItem;

        protected override int GetItemHeight(int itemIndex)
        {
            return GetIntItemProperty(itemIndex, ItemHeightNeeded, ItemHeightDefault);
        }

        protected override int GetItemIndent(int itemIndex)
        {
            return GetIntItemProperty(itemIndex, ItemIndentNeeded, ItemIndentDefault);
        }

        protected override string GetItemText(int itemIndex)
        {
            return GetStringItemProperty(itemIndex, ItemTextNeeded, string.Empty);
        }

        protected override Image GetItemIcon(int itemIndex)
        {
            return GetImageItemProperty(itemIndex, ItemIconNeeded, ImageDefaultIcon);
        }

        protected override bool GetItemCheckBoxVisible(int itemIndex)
        {
            return GetBoolItemProperty(itemIndex, ItemCheckBoxVisibleNeeded, ShowCheckBoxes);
        }

        protected override Color GetItemBackColor(int itemIndex)
        {
            return GetColorItemProperty(itemIndex, ItemBackColorNeeded, Color.Empty);
        }

        protected override Color GetItemForeColor(int itemIndex)
        {
            return GetColorItemProperty(itemIndex, ItemForeColorNeeded, ForeColor);
        }

        protected override bool GetItemExpanded(int itemIndex)
        {
            return GetBoolItemProperty(itemIndex, ItemExpandedNeeded, false);
        }

        protected override bool CanUnselectItem(int itemIndex)
        {
            return GetBoolItemProperty(itemIndex, CanUnselectItemNeeded, true);
        }

        protected override bool CanSelectItem(int itemIndex)
        {
            return GetBoolItemProperty(itemIndex, CanSelectItemNeeded, true);
        }

        protected override bool CanUncheckItem(int itemIndex)
        {
            return GetBoolItemProperty(itemIndex, CanUncheckItemNeeded, true);
        }

        protected override bool CanCheckItem(int itemIndex)
        {
            return GetBoolItemProperty(itemIndex, CanCheckItemNeeded, true);
        }

        protected override bool CanExpandItem(int itemIndex)
        {
            return GetBoolItemProperty(itemIndex, CanExpandItemNeeded, true);
        }

        protected override bool CanCollapseItem(int itemIndex)
        {
            return GetBoolItemProperty(itemIndex, CanCollapseItemNeeded, true);
        }

        protected override void OnItemChecked(int itemIndex)
        {
            if (ItemCheckedStateChanged != null)
                ItemCheckedStateChanged(this, new ItemCheckedStateChangedEventArgs { ItemIndex = itemIndex, Checked = true });

            base.OnItemChecked(itemIndex);
        }

        protected override void OnItemUnchecked(int itemIndex)
        {
            if (ItemCheckedStateChanged != null)
                ItemCheckedStateChanged(this, new ItemCheckedStateChangedEventArgs { ItemIndex = itemIndex, Checked = false });

            base.OnItemUnchecked(itemIndex);
        }

        protected override void OnItemExpanded(int itemIndex)
        {
            if (ItemExpandedStateChanged != null)
                ItemExpandedStateChanged(this, new ItemExpandedStateChangedEventArgs { ItemIndex = itemIndex, Expanded = true });

            base.OnItemExpanded(itemIndex);
        }

        protected override void OnItemCollapsed(int itemIndex)
        {
            if (ItemExpandedStateChanged != null)
                ItemExpandedStateChanged(this, new ItemExpandedStateChangedEventArgs { ItemIndex = itemIndex, Expanded = false });

            base.OnItemCollapsed(itemIndex);
        }

        protected override void OnItemSelected(int itemIndex)
        {
            if (ItemSelectedStateChanged != null)
                ItemSelectedStateChanged(this, new ItemSelectedStateChangedEventArgs { ItemIndex = itemIndex, Selected = true });

            base.OnItemSelected(itemIndex);
        }

        protected override void OnItemUnselected(int itemIndex)
        {
            if (ItemSelectedStateChanged != null)
                ItemSelectedStateChanged(this, new ItemSelectedStateChangedEventArgs { ItemIndex = itemIndex, Selected = false });

            base.OnItemUnselected(itemIndex);
        }

        protected override void OnItemDrag(HashSet<int> itemIndex)
        {
            if (ItemDrag != null)
                ItemDrag(this, new ItemDragEventArgs { ItemIndex = itemIndex });
            else
                DoDragDrop(itemIndex, DragDropEffects.Copy);

            base.OnItemDrag(itemIndex);
        }

        protected override void DrawItem(Graphics gr, VisibleItemInfo info)
        {
            if (PaintItem != null)
                PaintItem(this, new PaintItemContentEventArgs {Graphics = gr, Info = info});
            else
                base.DrawItem(gr, info);
        }

        protected override void OnDragOverItem(DragOverItemEventArgs e)
        {
            base.OnDragOverItem(e);

            if (DragOverItem != null)
                DragOverItem(this, e);
        }

        protected override void OnDragDropOverItem(DragOverItemEventArgs e)
        {
            if (DragDropOverItem != null)
                DragDropOverItem(this, e);

            base.OnDragDropOverItem(e);
        }

        #endregion

        #region Event Helpers

        private IntItemEventArgs intArg = new IntItemEventArgs();
        private BoolItemEventArgs boolArg = new BoolItemEventArgs();
        private StringItemEventArgs stringArg = new StringItemEventArgs();
        private ImageItemEventArgs imageArg = new ImageItemEventArgs();
        private ColorItemEventArgs colorArg = new ColorItemEventArgs();

        int GetIntItemProperty(int itemIndex, EventHandler<IntItemEventArgs> handler, int defaultValue)
        {
            if (handler != null)
            {
                intArg.ItemIndex = itemIndex;
                intArg.Result = defaultValue;
                handler(this, intArg);
                return intArg.Result;
            }

            return defaultValue;
        }

        string GetStringItemProperty(int itemIndex, EventHandler<StringItemEventArgs> handler, string defaultValue)
        {
            if (handler != null)
            {
                stringArg.ItemIndex = itemIndex;
                stringArg.Result = defaultValue;
                handler(this, stringArg);
                return stringArg.Result;
            }

            return defaultValue;
        }

        bool GetBoolItemProperty(int itemIndex, EventHandler<BoolItemEventArgs> handler, bool defaultValue)
        {
            if (handler != null)
            {
                boolArg.ItemIndex = itemIndex;
                boolArg.Result = defaultValue;
                handler(this, boolArg);
                return boolArg.Result;
            }

            return defaultValue;
        }

        Image GetImageItemProperty(int itemIndex, EventHandler<ImageItemEventArgs> handler, Image defaultValue)
        {
            if (handler != null)
            {
                imageArg.ItemIndex = itemIndex;
                imageArg.Result = defaultValue;
                handler(this, imageArg);
                return imageArg.Result;
            }

            return defaultValue;
        }

        Color GetColorItemProperty(int itemIndex, EventHandler<ColorItemEventArgs> handler, Color defaultValue)
        {
            if (handler != null)
            {
                colorArg.ItemIndex = itemIndex;
                colorArg.Result = defaultValue;
                handler(this, colorArg);
                return colorArg.Result;
            }

            return defaultValue;
        }

        #endregion Helpers
    }

    public class GenericItemResultEventArgs<T> : EventArgs
    {
        public int ItemIndex { get; internal set; }
        public T Result;
    }

    public class IntItemEventArgs : GenericItemResultEventArgs<int>
    {
    }

    public class StringItemEventArgs : GenericItemResultEventArgs<string>
    {
    }

    public class ImageItemEventArgs : GenericItemResultEventArgs<Image>
    {
    }

    public class ColorItemEventArgs : GenericItemResultEventArgs<Color>
    {
    }

    public class BoolItemEventArgs : GenericItemResultEventArgs<bool>
    {
    }

    public class ItemCheckedStateChangedEventArgs : EventArgs
    {
        public int ItemIndex;
        public bool Checked;
    }

    public class ItemExpandedStateChangedEventArgs : EventArgs
    {
        public int ItemIndex;
        public bool Expanded;
    }

    public class ItemSelectedStateChangedEventArgs : EventArgs
    {
        public int ItemIndex;
        public bool Selected;
    }

    public class PaintItemContentEventArgs:EventArgs
    {
        public Graphics Graphics;
        public FastListBase.VisibleItemInfo Info;
    }

    public class ItemDragEventArgs : EventArgs
    {
        public HashSet<int> ItemIndex;
    }
}
