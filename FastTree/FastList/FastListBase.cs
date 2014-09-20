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
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace FastTreeNS
{
    [ToolboxItem(false)] 
    public class FastListBase : UserControl
    {
        private readonly List<int> yOfItems = new List<int>();
        private ToolTip tt;
        private int itemCount;

        [Browsable(false)]
        public HashSet<int> SelectedItemIndex { get; private set; }
        [Browsable(false)]
        public HashSet<int> CheckedItemIndex { get; private set; }

        [DefaultValue(true)]
        public bool ShowToolTips { get; set; }
        [DefaultValue(17)]
        public virtual int ItemHeightDefault { get; set; }
        [DefaultValue(10)]
        public virtual int ItemIndentDefault { get; set; }
        [DefaultValue(typeof(Size), "16, 16")]
        public Size IconSize { get; set; }
        [Browsable(false)]
        public bool IsEditMode { get; set; }
        [DefaultValue(false)]
        public bool ShowIcons { get; set; }
        [DefaultValue(false)]
        public bool ShowCheckBoxes { get; set; }
        [DefaultValue(false)]
        public bool ShowExpandBoxes { get; set; }
        public virtual Image ImageCheckBoxOn { get; set; }
        public virtual Image ImageCheckBoxOff { get; set; }
        public virtual Image ImageCollapse { get; set; }
        public virtual Image ImageExpand { get; set; }
        public virtual Image ImageDefaultIcon { get; set; }

        [DefaultValue(typeof(Color), "33, 53, 80")]
        public Color SelectionColor { get; set; }
        [DefaultValue(100)]
        public int SelectionColorOpaque { get; set; }
        [DefaultValue(false)]
        public bool MultiSelect { get; set; }
        [DefaultValue(2)]
        public int ItemInterval { get; set; }
        [DefaultValue(false)]
        public bool FullItemSelect { get; set; }
        [DefaultValue(false)]
        public bool AllowDragItems { get; set; }

        [Browsable(false)]
        public override bool AutoScroll { get { return true; } }

        public FastListBase()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

            tt = new ToolTip() {UseAnimation = false };

            SelectedItemIndex = new HashSet<int>();
            CheckedItemIndex = new HashSet<int>();
            InitDefaultProperties();
        }

        protected virtual void InitDefaultProperties()
        {
            IconSize = new Size(16, 16);
            ItemHeightDefault = 17;
            VerticalScroll.SmallChange = ItemHeightDefault;

            ImageCheckBoxOn = Resources.checkbox_yes;
            ImageCheckBoxOff = Resources.checkbox_no;
            ImageCollapse = Resources.collapse;
            ImageExpand = Resources.expand;
            ImageDefaultIcon = Resources.default_icon;
            SelectionColor = Color.FromArgb(33, 53, 80);
            SelectionColorOpaque = 100;
            ItemInterval = 2;
            BackColor = SystemColors.Window;
            ItemIndentDefault = 10;
            ShowToolTips = true;
        }

        public virtual int ItemCount 
        {
            get { return itemCount; }
            set
            {
                itemCount = value;
                Build();
            }
        }

        #region Drag&Drop

        private DragOverItemEventArgs lastDragAndDropEffect;

        protected override void OnDragOver(DragEventArgs e)
        {
            var p = new Point(e.X, e.Y);
            p = PointToClient(p);

            var itemIndex = YToIndexAround(p.Y + VerticalScroll.Value);
            var rect = CalcItemRect(itemIndex);

            var textRect = rect;
            if (visibleItemInfos.ContainsKey(itemIndex))
            {
                var info = visibleItemInfos[itemIndex];
                textRect = new Rectangle(info.X_Text, rect.Y, info.X_EndText - info.X_Text + 1, rect.Height);
            }

            var ea = new DragOverItemEventArgs(e.Data, e.KeyState, p.X, p.Y, e.AllowedEffect, e.Effect, rect, textRect){ItemIndex = itemIndex};

            OnDragOverItem(ea);

            if (ea.Effect != DragDropEffects.None)
                lastDragAndDropEffect = ea;
            else
                lastDragAndDropEffect = null;

            e.Effect = ea.Effect;

            //scroll
            if (ea.ItemIndex >= 0 && ea.ItemIndex < ItemCount && itemIndex != ea.ItemIndex)
            {
                rect = CalcItemRect(ea.ItemIndex);
                rect.Inflate(0, 2);
                rect.Offset(HorizontalScroll.Value, VerticalScroll.Value);
                ScrollToRectangle(rect);
            }
            else
            {
                if (p.Y <= Padding.Top + ItemHeightDefault/2)
                    ScrollUp();
                else if (p.Y >= ClientSize.Height - Padding.Bottom - +ItemHeightDefault/2)
                    ScrollDown();
            }

            Invalidate();

            //base.OnDragOver(e);
        }

        protected virtual void OnDragOverItem(DragOverItemEventArgs e)
        {
            if (e.Y < e.ItemRect.Y + e.ItemRect.Height / 2)
                e.InsertEffect = InsertEffect.InsertBefore;
            else
                e.InsertEffect = InsertEffect.InsertAfter;
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            if (lastDragAndDropEffect != null)
                OnDropOverItem(lastDragAndDropEffect);

            lastDragAndDropEffect = null;
            Invalidate();
        }

        protected virtual void OnDropOverItem(DragOverItemEventArgs e)
        {
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
            lastDragAndDropEffect = null;
            Invalidate();
        }

        public Rectangle CalcItemRect(int index)
        {
            Rectangle res;

            var i = index;
            if (i >= ItemCount)
                i = ItemCount - 1;

            if (i < 0)
                res = Rectangle.FromLTRB(ClientRectangle.Left + Padding.Left, ClientRectangle.Top + Padding.Top - 2, ClientRectangle.Right - Padding.Right, ClientRectangle.Top + Padding.Top - 1);
            else
            {
                var y = yOfItems[i];
                var h = yOfItems[i + 1] - y;

                res = Rectangle.FromLTRB(ClientRectangle.Left + Padding.Left, y, ClientRectangle.Right - Padding.Right,
                                         y + h);

                if (index >= itemCount)
                    res.Offset(0, (index - itemCount + 1)*ItemHeightDefault);
            }

            res.Offset(-HorizontalScroll.Value, -VerticalScroll.Value);

            return res;
        }

        #endregion Drop

        #region Keyboard

        protected override bool IsInputKey(Keys keyData)
        {
            if (!IsEditMode)
            {
                switch (keyData)
                {
                    case Keys.Home:
                    case Keys.End:
                    case Keys.PageDown:
                    case Keys.PageUp:
                    case Keys.Down:
                    case Keys.Up:
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Enter:
                    case Keys.Space:
                    case Keys.A | Keys.Control:
                        return true;

                    default:
                        return false;
                }
            }
            else
                return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Handled) return;

            switch(e.KeyCode)
            {
                case Keys.Up:
                    if (e.Control)
                        ScrollUp();
                    else
                        SelectPrev();
                    break;

                case Keys.Down:
                    if (e.Control)
                        ScrollDown();
                    else
                        SelectNext();
                    break;

                case Keys.PageUp:
                    if (e.Control)
                        ScrollPageUp();
                    else
                    if (SelectedItemIndex.Count > 0)
                    {
                        var i = SelectedItemIndex.First();
                        var y = yOfItems[i] - ClientRectMinusPaddings.Height;
                        i = YToIndex(y) + 1;
                        SelectItem(Math.Max(0, Math.Min(ItemCount - 1, i)));
                    }
                    break;

                case Keys.PageDown:
                    if (e.Control)
                        ScrollPageDown();
                    else
                    if (SelectedItemIndex.Count > 0)
                    {
                        var i = SelectedItemIndex.First();
                        var y = yOfItems[i] + ClientRectMinusPaddings.Height;
                        i = YToIndex(y);
                        SelectItem(i < 0 ? ItemCount - 1 : i);
                    }
                    break;

                case Keys.Home:
                    if (e.Control)
                        ScrollToItem(0);
                    else
                        SelectItem(0);
                    break;

                case Keys.End:
                    if (e.Control)
                        ScrollToItem(ItemCount - 1);
                    else
                        SelectItem(ItemCount - 1);
                    break;

                case Keys.Enter:
                case Keys.Space:
                    if (ShowCheckBoxes)
                    {
                        if (SelectedItemIndex.Count > 0)
                        {
                            var val = GetItemChecked(SelectedItemIndex.First());
                            if (val)
                                UncheckSelected();
                            else
                                CheckSelected();
                        }
                    }else
                    if(ShowExpandBoxes)
                    {
                        if (SelectedItemIndex.Count > 0)
                        {
                            var itemIndex = SelectedItemIndex.First();
                            if (GetItemExpanded(itemIndex))
                                CollapseItem(itemIndex);
                            else
                                ExpandItem(itemIndex);
                        }
                    }
                    break;

                case Keys.A :
                    if(e.Control)
                    {
                        SelectAll();
                    }
                    break;
            }
        }

        #endregion

        #region Mouse

        private int startDiapasonSelectedItemIndex;
        private bool mouseCanSelectArea;
        private Point startMouseSelectArea;
        private Rectangle mouseSelectArea;
        private Point lastMouseClick;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            mouseCanSelectArea = false;
            mouseSelectArea = Rectangle.Empty;
            lastMouseClick = e.Location;

            var item = PointToItemInfo(e.Location);

            if (item == null)
                return;

            if (e.Button == MouseButtons.Left && item.X_Icon <= e.Location.X)
            {
                //Select
                if (MultiSelect)
                {
                    startMouseSelectArea = e.Location;
                    startMouseSelectArea.Offset(HorizontalScroll.Value, VerticalScroll.Value);
                    mouseCanSelectArea = item.X_EndText < e.Location.X || !AllowDragItems;
                }

                if (!AllowDragItems || !MultiSelect)
                    OnMouseSelectItem(e, item);
            }

            if (e.Button == MouseButtons.Left && item.X_CheckBox <= e.Location.X && item.X_Icon > e.Location.X)
            {
                //Checkbox
                OnCheckboxClick(item);
                Invalidate();
            }

            if (e.Button == MouseButtons.Left && item.X_ExpandBox <= e.Location.X && item.X_CheckBox > e.Location.X)
            {
                //Expand
                OnExpandBoxClick(item);
                Invalidate();
            }
        }

        protected virtual void OnMouseSelectItem(MouseEventArgs e, VisibleItemInfo item)
        {
            if (MultiSelect)
            {
                startMouseSelectArea = e.Location;
                startMouseSelectArea.Offset(HorizontalScroll.Value, VerticalScroll.Value);
                mouseCanSelectArea = item.X_EndText < e.Location.X || !AllowDragItems;

                if (Control.ModifierKeys == Keys.Control)
                {
                    if (SelectedItemIndex.Contains(item.ItemIndex) && SelectedItemIndex.Count > 1)
                        UnselectItem(item.ItemIndex);
                    else
                        SelectItem(item.ItemIndex, false);

                    startDiapasonSelectedItemIndex = -1;
                }
                else if (Control.ModifierKeys == Keys.Shift)
                {
                    if (SelectedItemIndex.Count == 1)
                        startDiapasonSelectedItemIndex = SelectedItemIndex.First();

                    if (startDiapasonSelectedItemIndex >= 0)
                        SelectItems(Math.Min(startDiapasonSelectedItemIndex, item.ItemIndex),
                                    Math.Max(startDiapasonSelectedItemIndex, item.ItemIndex));
                }
            }

            if (!MultiSelect || Control.ModifierKeys == Keys.None)
                if (!SelectedItemIndex.Contains(item.ItemIndex) || SelectedItemIndex.Count > 1)
                    SelectItem(item.ItemIndex, true);

            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if(e.Button == MouseButtons.Left && mouseCanSelectArea)
            {
                if (Math.Abs(e.Location.X - startMouseSelectArea.X) > 0)
                {
                    var pos = e.Location;
                    pos.Offset(HorizontalScroll.Value, VerticalScroll.Value);
                    mouseSelectArea = new Rectangle(Math.Min(startMouseSelectArea.X, pos.X), Math.Min(startMouseSelectArea.Y, pos.Y), Math.Abs(startMouseSelectArea.X - pos.X), Math.Abs(startMouseSelectArea.Y - pos.Y));

                    var i1 = YToIndex(startMouseSelectArea.Y);
                    var i2 = YToIndex(pos.Y);
                    if (i1 >= 0 && i2 >= 0)
                    {
                        SelectItems(Math.Min(i1, i2), Math.Max(i1, i2));
                    }

                    if (e.Location.Y <= Padding.Top + ItemHeightDefault / 2)
                        ScrollUp();
                    else
                    if (e.Location.Y >= ClientSize.Height - Padding.Bottom - +ItemHeightDefault / 2)
                        ScrollDown();

                    Invalidate();
                }
                else
                    mouseSelectArea = Rectangle.Empty;
            }
            else
            if (e.Button == System.Windows.Forms.MouseButtons.Left && AllowDragItems && SelectedItemIndex.Count > 0 && (Math.Abs(lastMouseClick.X - e.Location.X) > 2 || Math.Abs(lastMouseClick.Y - e.Location.Y) > 2))
            {
                OnItemDrag(new HashSet<int>(SelectedItemIndex));
            }else
            if(e.Button == System.Windows.Forms.MouseButtons.None)
            {
                var p = PointToClient(MousePosition);
                var info = PointToItemInfo(p);
                if (info != null && info.X_EndText == info.X_End)
                {
                    if (tt.Tag != info.Text && ShowToolTips)
                        tt.Show(info.Text, this, info.X_Text - 3, info.Y - 2, 2000);
                    tt.Tag = info.Text;
                }
                else
                {
                    tt.Tag = null;
                    tt.Hide(this);
                }
            }
        }

        protected virtual void OnItemDrag(HashSet<int> itemIndex)
        {
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            var item = PointToItemInfo(e.Location);

            if (item != null)
            if (e.Button == MouseButtons.Left && item.X_Icon <= e.Location.X)
            {
                if (AllowDragItems && MultiSelect && mouseSelectArea == Rectangle.Empty)
                    OnMouseSelectItem(e, item);
            }

            mouseCanSelectArea = false;

            Invalidate();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            var item = PointToItemInfo(e.Location);

            if (item != null)
            if (e.Button == MouseButtons.Left && item.X_Icon <= e.Location.X)
            {
                if (GetItemExpanded(item.ItemIndex))
                    CollapseItem(item.ItemIndex);
                else
                    ExpandItem(item.ItemIndex);
            }
        }

        #endregion mouse

        #region Check, Expand

        protected virtual void OnExpandBoxClick(VisibleItemInfo info)
        {
            if (info.Expanded)
                CollapseItem(info.ItemIndex);
            else
                ExpandItem(info.ItemIndex);
        }

        public virtual bool CollapseItem(int itemIndex)
        {
            Invalidate();

            if(CanCollapseItem(itemIndex))
            {
                OnItemCollapsed(itemIndex);
                return true;
            }

            return false;
        }

        public virtual bool ExpandItem(int itemIndex)
        {
            Invalidate();

            if (CanExpandItem(itemIndex))
            {
                OnItemExpanded(itemIndex);
                return true;
            }

            return false;
        }

        protected virtual void OnItemCollapsed(int itemIndex)
        {
        }

        protected virtual void OnItemExpanded(int itemIndex)
        {
        }


        protected virtual void OnCheckboxClick(VisibleItemInfo info)
        {
            if (GetItemChecked(info.ItemIndex))
                UncheckItem(info.ItemIndex);
            else
                CheckItem(info.ItemIndex);
        }

        public virtual bool CheckItem(int itemIndex)
        {
            if (GetItemChecked(itemIndex))
                return true;

            Invalidate();

            if (CanCheckItem(itemIndex))
            {
                CheckedItemIndex.Add(itemIndex);
                OnItemChecked(itemIndex);
                return true;
            }

            return false;
        }

        public virtual bool CheckAll()
        {
            var res = true;
            for (int i = 0; i < ItemCount; i++)
                res &= CheckItem(i);

            return res;
        }

        public virtual bool UncheckItem(int itemIndex)
        {
            if (!GetItemChecked(itemIndex))
                return true;

            Invalidate();

            if (CanUncheckItem(itemIndex))
            {
                CheckedItemIndex.Remove(itemIndex);
                OnItemUnchecked(itemIndex);
                return true;
            }

            return false;
        }

        public virtual bool UncheckAll()
        {
            foreach (var i in CheckedItemIndex)
                if (!CanUncheckItem(i))
                    return false;

            var list = new List<int>(CheckedItemIndex);

            CheckedItemIndex.Clear();

            foreach (var i in list)
                OnItemUnchecked(i);

            Invalidate();

            return true;
        }

        protected virtual void OnItemChecked(int itemIndex)
        {
        }

        protected virtual void OnItemUnchecked(int itemIndex)
        {
        }

        protected virtual void CheckSelected()
        {
            foreach (var i in SelectedItemIndex)
                CheckItem(i);
        }

        protected virtual void UncheckSelected()
        {
            foreach (var i in SelectedItemIndex)
                UncheckItem(i);
        }

        #endregion Check, Expand

        #region Selection

        public virtual bool UnselectItem(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= ItemCount)
                return false;

            if (!SelectedItemIndex.Contains(itemIndex))
                return true;

            if (!CanUnselectItem(itemIndex))
                return false;

            SelectedItemIndex.Remove(itemIndex);
            OnItemUnselected(itemIndex);

            return true;
        }

        public virtual bool UnselectAll()
        {
            foreach(var i in SelectedItemIndex)
            if (!CanUnselectItem(i))
                return false;

            var list = new List<int>(SelectedItemIndex);

            SelectedItemIndex.Clear();

            foreach (var i in list)
                OnItemUnselected(i);

            Invalidate();

            return true;
        }

        public virtual bool SelectItem(int itemIndex, bool unselectOtherItems = true)
        {
            if (itemIndex < 0 || itemIndex >= ItemCount)
                return false;

            if (!CanSelectItem(itemIndex))
                return false;

            var contains = SelectedItemIndex.Contains(itemIndex);

            if(unselectOtherItems)
            {
                foreach (var i in SelectedItemIndex)
                    if(i != itemIndex)
                    if (!CanUnselectItem(i))
                        return false;

                var list = new List<int>(SelectedItemIndex);

                SelectedItemIndex.Clear();

                foreach (var i in list)
                    if (i != itemIndex)
                        OnItemUnselected(i);
            }

            SelectedItemIndex.Add(itemIndex);

            if (!contains)
                OnItemSelected(itemIndex);

            ScrollToItem(itemIndex);

            return true;
        }

        public virtual bool SelectItems(int from, int to)
        {
            foreach (var i in SelectedItemIndex)
                if (i < from || i > to)
                    if (!CanUnselectItem(i))
                        return false;

            var list = new List<int>(SelectedItemIndex);

            SelectedItemIndex.RemoveWhere(i=> i < from | i > to);

            foreach (var i in list)
                if (i < from || i > to)
                    OnItemUnselected(i);

            for(int i=from;i<=to;i++)
            if(!SelectedItemIndex.Contains(i))
            if(CanSelectItem(i))
            {
                SelectedItemIndex.Add(i);
                OnItemSelected(i);
            }

            Invalidate();

            return SelectedItemIndex.Count > 0;
        }

        public virtual bool SelectAll()
        {
            return SelectItems(0, ItemCount - 1);
        }

        public bool SelectNext(bool unselectOtherItems = true)
        {
            if (SelectedItemIndex.Count == 0)
                return false;

            var index = SelectedItemIndex.Max() + 1;
            if (index >= ItemCount)
                return false;

            var res = SelectItem(index, unselectOtherItems);
            if (res)
                ScrollToItem(index);

            return res;
        }

        public bool SelectPrev(bool unselectOtherItems = true)
        {
            if (SelectedItemIndex.Count == 0)
                return false;

            var index = SelectedItemIndex.Min() - 1;
            if (index < 0)
                return false;

            var res = SelectItem(index, unselectOtherItems);
            if (res)
                ScrollToItem(index);

            return res;
        }

        protected virtual void OnItemSelected(int itemIndex)
        {
        }

        protected virtual void OnItemUnselected(int itemIndex)
        {
        }

        #endregion

        #region Paint

        protected override void OnPaint(PaintEventArgs e)
        {
            //was build request
            if(buildNeeded)
            {
                Build();
                buildNeeded = false;
            }
            //
            e.Graphics.SetClip(ClientRectMinusPaddings);
            DrawItems(e.Graphics);

            if (lastDragAndDropEffect == null)
                DrawMouseSelectedArea(e.Graphics);
            else
                DrawDragOverInsertEffect(e.Graphics, lastDragAndDropEffect);

            if (!Enabled)
            {
                e.Graphics.SetClip(ClientRectangle);
                var color = Color.FromArgb(50, (BackColor.R + 127) >> 1, (BackColor.G + 127) >> 1, (BackColor.B + 127) >> 1);
                using(var brush = new SolidBrush(color))
                    e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        protected virtual void DrawDragOverInsertEffect(Graphics gr, DragOverItemEventArgs e)
        {
            var c1 = Color.FromArgb(SelectionColor.A == 255 ? SelectionColorOpaque : SelectionColor.A, SelectionColor);
            var c2 = Color.Transparent;

            if (!visibleItemInfos.ContainsKey(e.ItemIndex))
                return;
            var info = visibleItemInfos[e.ItemIndex];
            var rect = new Rectangle(info.X_ExpandBox, info.Y, 1000, info.Height);

            switch(e.InsertEffect)
            {
                case InsertEffect.Replace:
                    using (var brush = new SolidBrush(c1))
                        gr.FillRectangle(brush, rect);
                    break;

                case InsertEffect.InsertBefore:
                    if (e.ItemIndex <= 0)
                        rect.Offset(0, 2);
                    using (var pen = new Pen(c1, 2) { DashStyle = DashStyle.Dash })
                        gr.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
                    break;

                case InsertEffect.InsertAfter:
                    if (e.ItemIndex < 0)
                        rect.Offset(0, 2);
                    using (var pen = new Pen(c1, 2) { DashStyle = DashStyle.Dash })
                        gr.DrawLine(pen , rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                    break;

                case InsertEffect.AddAsChild:
                    if (e.ItemIndex >= 0 && e.ItemIndex < ItemCount)
                    {
                        var dx = GetItemIndent(e.ItemIndex) + 80;
                        rect.Offset(dx, 0);
                        using (var pen = new Pen(c1, 2) {DashStyle = DashStyle.Dash})
                            gr.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                    }
                    break;
            }
        }

        private void DrawMouseSelectedArea(Graphics gr)
        {
            if (mouseCanSelectArea && mouseSelectArea != Rectangle.Empty)
            {
                var c = Color.FromArgb(SelectionColor.A == 255 ? SelectionColorOpaque : SelectionColor.A, SelectionColor);
                var rect = new Rectangle(mouseSelectArea.Left - HorizontalScroll.Value, mouseSelectArea.Top - VerticalScroll.Value, mouseSelectArea.Width, mouseSelectArea.Height);
                using(var pen = new Pen(c))
                    gr.DrawRectangle(pen, rect);
            }

        }

        protected virtual void DrawItems(Graphics gr)
        {
            var i = Math.Max(0, PointToIndex(new Point(Padding.Left, Padding.Top)) - 1);

            visibleItemInfos.Clear();

            for (; i < ItemCount; i++)
            {
                var info = visibleItemInfos[i] = CalcVisibleItemInfo(gr, i);
                if (info.Y > ClientSize.Height)
                    break;

                DrawItem(gr, info);
            }
        }

        protected readonly Dictionary<int, VisibleItemInfo> visibleItemInfos = new Dictionary<int, VisibleItemInfo>();

        protected virtual void DrawItem(Graphics gr, VisibleItemInfo info)
        {
            DrawItemBackgound(gr, info);

            if (lastDragAndDropEffect == null)//do not draw selection when drag&drop over the control
            if (SelectedItemIndex.Contains(info.ItemIndex))
                DrawSelection(gr, info);

            DrawItemIcons(gr, info);
            DrawItemContent(gr, info);
        }

        [Browsable(false)]
        public Rectangle ClientRectMinusPaddings
        {
            get
            {
                var rect = ClientRectangle;
                return new Rectangle(rect.Left + Padding.Left, rect.Top + Padding.Top,
                                     rect.Width - Padding.Left - Padding.Right,
                                     rect.Height - Padding.Top - Padding.Bottom);
            }
        }

        public virtual void DrawSelection(Graphics gr, VisibleItemInfo info)
        {
            var c1 = Color.FromArgb(SelectionColor.A == 255 ? SelectionColorOpaque : SelectionColor.A, SelectionColor);
            var c2 = Color.FromArgb(c1.A / 2, SelectionColor);
            var rect = info.TextAndIconRect;

            if (FullItemSelect)
            {
                var cr = ClientRectMinusPaddings;
                rect = new Rectangle(cr.Left, rect.Top, cr.Width - 1, rect.Height);
            }

            if (rect.Width > 0 && rect.Height > 0)
            using (var brush = new LinearGradientBrush(rect, c2, c1, LinearGradientMode.Vertical))
            using (var pen = new Pen(c1))
            {
                gr.FillRectangle(brush, Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right + 1, rect.Bottom + 1));
                gr.DrawRectangle(pen, rect);
            }
        }

        public virtual void DrawItemIcons(Graphics gr, VisibleItemInfo info)
        {
            if (info.ExpandBoxVisible)
            {
                var img = (Bitmap)(info.Expanded ? ImageCollapse : ImageExpand);
                img.SetResolution(gr.DpiX, gr.DpiY);
                gr.DrawImage(img, info.X_ExpandBox, info.Y);
            }

            if (info.CheckBoxVisible)
            {
                var img = (Bitmap)(GetItemChecked(info.ItemIndex) ? ImageCheckBoxOn : ImageCheckBoxOff);
                img.SetResolution(gr.DpiX, gr.DpiY);
                gr.DrawImage(img, info.X_CheckBox, info.Y + 1);
            }

            if (ShowIcons && info.Icon != null)
            {
                var img = (Bitmap)info.Icon;
                img.SetResolution(gr.DpiX, gr.DpiY);
                gr.DrawImage(img, info.X_Icon, info.Y + 1);
            }
        }

        public virtual void DrawItemContent(Graphics gr, VisibleItemInfo info)
        {
            using (var brush = new SolidBrush(info.ForeColor))
                gr.DrawString(info.Text, Font, brush, info.X_Text, info.Y + 1);
        }

        public virtual void DrawItemBackgound(Graphics gr, VisibleItemInfo info)
        {
            var backColor = info.BackColor;

            if (backColor != Color.Empty)
            using (var brush = new SolidBrush(backColor))
                gr.FillRectangle(brush, info.TextAndIconRect);
        }

        protected virtual VisibleItemInfo CalcVisibleItemInfo(Graphics gr, int itemIndex)
        {
            var result = new VisibleItemInfo();
            result.Calc(this, itemIndex, gr);
            return result;
        }

        #endregion

        #region Coordinates

        /// <summary>
        /// Absolute Y coordinate of the control to item index
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public int YToIndex(int y)
        {
            if (y < Padding.Top)
                return -1;

            if (ItemCount <= 0)
                return -1;

            int i = yOfItems.BinarySearch(y + 1);
            if (i < 0)
            {
                i = ~i;
                i -= 1;
            }

            if (i >= ItemCount)
                return -1;

            return i;
        }

        protected int YToIndexAround(int y)
        {
            if (ItemCount <= 0)
                return -1;

            int i = yOfItems.BinarySearch(y + 1);
            if (i < 0)
            {
                i = ~i;
                i -= 1;
            }

            if (i < 0)
                i = 0;

            if (i >= ItemCount)
                i = ItemCount - 1;

            return i;
        }

        /// <summary>
        /// Control visible rect coordinates to item index
        /// </summary>
        public int PointToIndex(Point p)
        {
            if (p.X < Padding.Left || p.X > ClientRectangle.Right - Padding.Right)
                return -1;

            var y = p.Y + VerticalScroll.Value;

            return YToIndex(y);
        }

        /// <summary>
        /// Control visible rect coordinates to item info
        /// </summary>
        public virtual VisibleItemInfo PointToItemInfo(Point p)
        {
            var index = PointToIndex(p);
            VisibleItemInfo info = null;
            visibleItemInfos.TryGetValue(index, out info);

            return info;
        }

        /// <summary>
        ///   x0  x1  x2  x3      x4     x5
        ///   |   |   |   |       |      |
        ///   □   □   □   ItemText       
        /// </summary>
        public class VisibleItemInfo
        {
            public int ItemIndex;
            public int Y;
            public int Height;
            public string Text;
            public int X;
            public int X_ExpandBox;
            public int X_CheckBox;
            public int X_Icon;
            public int X_Text;
            public int X_EndText;
            public int X_End;
            public bool CheckBoxVisible;
            public bool ExpandBoxVisible;
            public Image Icon;
            public bool Expanded;
            public Color ForeColor;
            public Color BackColor;

            public Rectangle Rect
            {
                get { return new Rectangle(X, Y, X_EndText - X + 1, Height); }
            }

            public Rectangle TextAndIconRect
            {
                get { return new Rectangle(X_Icon, Y, X_EndText - X_Icon + 1, Height); }
            }

            public Rectangle TextRect
            {
                get { return new Rectangle(X_Text, Y, X_EndText - X_Text + 1, Height); }
            }

            public virtual void Calc(FastListBase list, int itemIndex, Graphics gr)
            {
                var vertScroll = list.VerticalScroll.Visible ? list.VerticalScroll.Value : 0;

                ItemIndex = itemIndex;
                CheckBoxVisible = list.ShowCheckBoxes && list.GetItemCheckBoxVisible(itemIndex);
                Icon = list.GetItemIcon(itemIndex);
                var y = list.yOfItems[itemIndex];
                Y = y - vertScroll;
                Height = list.yOfItems[itemIndex + 1] - y - list.ItemInterval;
                Text = list.GetItemText(itemIndex) ?? "";
                Expanded = list.GetItemExpanded(itemIndex);
                ExpandBoxVisible = list.ShowExpandBoxes && (Expanded ? list.CanCollapseItem(itemIndex) : list.CanExpandItem(itemIndex));
                BackColor = list.GetItemBackColor(itemIndex);
                ForeColor = list.GetItemForeColor(itemIndex);

                var x = list.GetItemIndent(itemIndex) + list.Padding.Left;
                X = x;
                X_ExpandBox = x;
                if (list.ShowExpandBoxes) x += list.ImageExpand.Width + 2;
                X_CheckBox = x;
                if (list.ShowCheckBoxes) x += list.ImageCheckBoxOn.Width + 2;
                X_Icon = x;
                if (list.ShowIcons) x += list.IconSize.Width + 2;
                X_Text = x;
                x += (int)gr.MeasureString(Text, list.Font).Width + 1;
                X_End = list.ClientSize.Width - list.Padding.Right - 2;
                X_EndText = Math.Min(x, X_End);
            }
        }

        #endregion

        #region Build

        protected virtual void Build()
        {
            yOfItems.Clear();

            var y = Padding.Top;

            for (int i = 0; i < itemCount;i++)
            {
                yOfItems.Add(y);
                y += GetItemHeight(i) + ItemInterval;
            }

            yOfItems.Add(y);

            SelectedItemIndex.RemoveWhere(i => i >= itemCount);
            CheckedItemIndex.RemoveWhere(i => i >= itemCount);

            AutoScrollMinSize = new Size(AutoScrollMinSize.Width, y + Padding.Bottom + 2);
            Invalidate();
        }

        bool buildNeeded;

        public virtual void BuildNeeded()
        {
            buildNeeded = true;
        }

        #endregion

        #region Get item info methods

        protected virtual int GetItemHeight(int itemIndex)
        {
            return  ItemHeightDefault;
        }

        protected virtual int GetItemIndent(int itemIndex)
        {
            return ItemIndentDefault;
        }

        protected virtual string GetItemText(int itemIndex)
        {
            return string.Empty;
        }

        protected virtual bool GetItemCheckBoxVisible(int itemIndex)
        {
            return ShowCheckBoxes;
        }

        protected virtual bool GetItemChecked(int itemIndex)
        {
            return CheckedItemIndex.Contains(itemIndex);
        }

        protected virtual Image GetItemIcon(int itemIndex)
        {
            return null;
        }

        protected virtual Color GetItemBackColor(int itemIndex)
        {
            return Color.Empty;
        }

        protected virtual Color GetItemForeColor(int itemIndex)
        {
            return ForeColor;
        }

        protected virtual bool GetItemExpanded(int itemIndex)
        {
            return false;
        }

        protected virtual bool CanUnselectItem(int itemIndex)
        {
            return true;
        }

        protected virtual bool CanSelectItem(int itemIndex)
        {
            return true;
        }

        protected virtual bool CanUncheckItem(int itemIndex)
        {
            return true;
        }

        protected virtual bool CanCheckItem(int itemIndex)
        {
            return true;
        }

        protected virtual bool CanExpandItem(int itemIndex)
        {
            return true;
        }

        protected virtual bool CanCollapseItem(int itemIndex)
        {
            return true;
        }

        #endregion

        #region Scroll

        private void ScrollUp()
        {
            if (!VerticalScroll.Visible)
                return;
            VerticalScroll.Value = Math.Max(VerticalScroll.Minimum, VerticalScroll.Value - VerticalScroll.SmallChange);
            UpdateScrollbars();
            Invalidate();
        }

        private void ScrollDown()
        {
            if (!VerticalScroll.Visible)
                return;
            VerticalScroll.Value = Math.Min(VerticalScroll.Maximum, VerticalScroll.Value + VerticalScroll.SmallChange);
            UpdateScrollbars();
            Invalidate();
        }

        private void ScrollPageUp()
        {
            if (!VerticalScroll.Visible)
                return;
            VerticalScroll.Value = Math.Max(VerticalScroll.Minimum, VerticalScroll.Value - VerticalScroll.LargeChange);
            UpdateScrollbars();
            Invalidate();
        }

        private void ScrollPageDown()
        {
            if (!VerticalScroll.Visible)
                return;
            VerticalScroll.Value = Math.Min(VerticalScroll.Maximum, VerticalScroll.Value + VerticalScroll.LargeChange);
            UpdateScrollbars();
            Invalidate();
        }

        protected new Size AutoScrollMinSize
        {
            set
            {
                if (!base.AutoScroll)
                    base.AutoScroll = true;
                Size newSize = value;
                base.AutoScrollMinSize = newSize;
            }

            get { return base.AutoScrollMinSize; }
        }

        /// <summary>
        /// Updates scrollbar position after Value changed
        /// </summary>
        public void UpdateScrollbars()
        {
            //some magic for update scrolls
            base.AutoScrollMinSize -= new Size(1, 0);
            base.AutoScrollMinSize += new Size(1, 0);
        }

        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int SB_ENDSCROLL = 0x8;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HSCROLL || m.Msg == WM_VSCROLL)
                if (m.WParam.ToInt32() != SB_ENDSCROLL)
                    Invalidate();

            base.WndProc(ref m);
        }

        public void ScrollToRectangle(Rectangle rect)
        {
            rect = new Rectangle(rect.X - HorizontalScroll.Value, rect.Y - VerticalScroll.Value, rect.Width, rect.Height);

            int v = VerticalScroll.Value;
            int h = HorizontalScroll.Value;

            if (rect.Bottom > ClientRectangle.Height)
                v += rect.Bottom - ClientRectangle.Height;
            else if (rect.Top < Padding.Top)
                v += rect.Top - Padding.Top;

            if (rect.Right > ClientRectangle.Width)
                h += rect.Right - ClientRectangle.Width;
            else if (rect.Left < Padding.Left)
                h += rect.Left - Padding.Left;
            //
            v = Math.Max(VerticalScroll.Minimum, v);
            h = Math.Max(HorizontalScroll.Minimum, h);
            //
            try
            {
                if (VerticalScroll.Visible)
                    VerticalScroll.Value = Math.Min(v, VerticalScroll.Maximum);
                if (HorizontalScroll.Visible)
                    HorizontalScroll.Value = Math.Min(h, HorizontalScroll.Maximum);
            }
            catch (ArgumentOutOfRangeException)
            {
                ;
            }

            UpdateScrollbars();
            Invalidate();
        }

        public void ScrollToItem(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= ItemCount)
                return;

            var y = yOfItems[itemIndex];
            var height = GetItemHeight(itemIndex);
            ScrollToRectangle(new Rectangle(0, y, ClientRectangle.Width, height));
        }

        #endregion

        #region Routines

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        #endregion
    }

    public class DragOverItemEventArgs : DragEventArgs
    {
        public int ItemIndex { get; set;}
        public InsertEffect InsertEffect { get; set; }
        public Rectangle ItemRect { get; private set; }
        public Rectangle TextRect { get; private set; }
        public object Tag { get; set; }

        public DragOverItemEventArgs(IDataObject data, int keyState, int x, int y, DragDropEffects allowedEffects, DragDropEffects effect, Rectangle itemRect, Rectangle textRect)
            : base(data, keyState, x, y, allowedEffects, effect)
        {
            this.ItemRect = itemRect;
            this.TextRect = textRect;
        }
    }

    public enum InsertEffect
    {
        None,
        InsertBefore,
        InsertAfter,
        Replace,
        AddAsChild
    }
}
