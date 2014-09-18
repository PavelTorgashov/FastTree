namespace Tester
{
    partial class FastListDropItemSample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FastListDropItemSample));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.fl = new FastTreeNS.FastList();
            this.lbRegular = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbAfter13 = new System.Windows.Forms.Label();
            this.lbReplace = new System.Windows.Forms.Label();
            this.lbEndOfList = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(446, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid1.SelectedObject = this.fl;
            this.propertyGrid1.Size = new System.Drawing.Size(245, 418);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // fl
            // 
            this.fl.AllowDrop = true;
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
            this.fl.Location = new System.Drawing.Point(176, 12);
            this.fl.MultiSelect = true;
            this.fl.Name = "fl";
            this.fl.ShowIcons = true;
            this.fl.Size = new System.Drawing.Size(264, 418);
            this.fl.TabIndex = 0;
            this.fl.ItemTextNeeded += new System.EventHandler<FastTreeNS.StringItemEventArgs>(this.fl_ItemTextNeeded);
            this.fl.DragOverItem += new System.EventHandler<FastTreeNS.DragOverItemEventArgs>(this.fl_DragOverItem);
            this.fl.DropOverItem += new System.EventHandler<FastTreeNS.DragOverItemEventArgs>(this.fl_DropOverItem);
            // 
            // lbRegular
            // 
            this.lbRegular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lbRegular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbRegular.Location = new System.Drawing.Point(12, 65);
            this.lbRegular.Name = "lbRegular";
            this.lbRegular.Size = new System.Drawing.Size(151, 34);
            this.lbRegular.TabIndex = 2;
            this.lbRegular.Text = "This label can be inserted anywhere";
            this.lbRegular.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbRegular.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lb_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Drag these labels into FastList:";
            // 
            // lbAfter13
            // 
            this.lbAfter13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lbAfter13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbAfter13.Location = new System.Drawing.Point(12, 113);
            this.lbAfter13.Name = "lbAfter13";
            this.lbAfter13.Size = new System.Drawing.Size(151, 34);
            this.lbAfter13.TabIndex = 4;
            this.lbAfter13.Text = "This label can be inserted only after Item 13";
            this.lbAfter13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbAfter13.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lb_MouseMove);
            // 
            // lbReplace
            // 
            this.lbReplace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lbReplace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbReplace.Location = new System.Drawing.Point(12, 162);
            this.lbReplace.Name = "lbReplace";
            this.lbReplace.Size = new System.Drawing.Size(151, 34);
            this.lbReplace.TabIndex = 5;
            this.lbReplace.Text = "This label always replaces items";
            this.lbReplace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbReplace.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lb_MouseMove);
            // 
            // lbEndOfList
            // 
            this.lbEndOfList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lbEndOfList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbEndOfList.Location = new System.Drawing.Point(12, 210);
            this.lbEndOfList.Name = "lbEndOfList";
            this.lbEndOfList.Size = new System.Drawing.Size(151, 34);
            this.lbEndOfList.TabIndex = 6;
            this.lbEndOfList.Text = "This label will be added at end of list";
            this.lbEndOfList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbEndOfList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lb_MouseMove);
            // 
            // FastListDropItemSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 442);
            this.Controls.Add(this.lbEndOfList);
            this.Controls.Add(this.lbReplace);
            this.Controls.Add(this.lbAfter13);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbRegular);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.fl);
            this.Name = "FastListDropItemSample";
            this.Text = "FastListDropItemSample";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastTreeNS.FastList fl;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Label lbRegular;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbAfter13;
        private System.Windows.Forms.Label lbReplace;
        private System.Windows.Forms.Label lbEndOfList;
    }
}