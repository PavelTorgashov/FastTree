namespace Tester
{
    partial class FastListDragItemSample
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FastListDragItemSample));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.fl = new FastTreeNS.FastList();
            this.lb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(446, 119);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid1.SelectedObject = this.fl;
            this.propertyGrid1.Size = new System.Drawing.Size(245, 311);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // fl
            // 
            this.fl.AllowDragItems = true;
            this.fl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fl.AutoScroll = true;
            this.fl.AutoScrollMinSize = new System.Drawing.Size(0, 2002);
            this.fl.BackColor = System.Drawing.SystemColors.Window;
            this.fl.ImageCheckBoxOff = ((System.Drawing.Image)(resources.GetObject("fl.ImageCheckBoxOff")));
            this.fl.ImageCheckBoxOn = ((System.Drawing.Image)(resources.GetObject("fl.ImageCheckBoxOn")));
            this.fl.ImageCollapse = ((System.Drawing.Image)(resources.GetObject("fl.ImageCollapse")));
            this.fl.ImageDefaultIcon = ((System.Drawing.Image)(resources.GetObject("fl.ImageDefaultIcon")));
            this.fl.ImageExpand = ((System.Drawing.Image)(resources.GetObject("fl.ImageExpand")));
            this.fl.IsEditMode = false;
            this.fl.ItemCount = 100;
            this.fl.Location = new System.Drawing.Point(12, 12);
            this.fl.MultiSelect = true;
            this.fl.Name = "fl";
            this.fl.ShowIcons = true;
            this.fl.Size = new System.Drawing.Size(428, 418);
            this.fl.TabIndex = 0;
            this.fl.ItemTextNeeded += new System.EventHandler<FastTreeNS.StringItemEventArgs>(this.fl_ItemTextNeeded);
            this.fl.ItemDrag += new System.EventHandler<FastTreeNS.ItemDragEventArgs>(this.fl_ItemDrag);
            // 
            // lb
            // 
            this.lb.AllowDrop = true;
            this.lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lb.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lb.ForeColor = System.Drawing.Color.Gray;
            this.lb.Location = new System.Drawing.Point(446, 12);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(245, 94);
            this.lb.TabIndex = 2;
            this.lb.Text = "Drag items here";
            this.lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb.DragDrop += new System.Windows.Forms.DragEventHandler(this.lb_DragDrop);
            this.lb.DragEnter += new System.Windows.Forms.DragEventHandler(this.lb_DragEnter);
            // 
            // FastListDragItemSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 442);
            this.Controls.Add(this.lb);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.fl);
            this.Name = "FastListDragItemSample";
            this.Text = "FastListDragItemSample";
            this.ResumeLayout(false);

        }

        #endregion

        private FastTreeNS.FastList fl;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Label lb;
    }
}