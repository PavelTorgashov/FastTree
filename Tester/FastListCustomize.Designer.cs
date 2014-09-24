namespace Tester
{
    partial class FastListCustomize
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FastListCustomize));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.fl = new FastTreeNS.FastList();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(446, 14);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid1.SelectedObject = this.fl;
            this.propertyGrid1.Size = new System.Drawing.Size(245, 416);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // fl
            // 
            this.fl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fl.AutoScroll = true;
            this.fl.AutoScrollMinSize = new System.Drawing.Size(0, 190002);
            this.fl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.fl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fl.FullItemSelect = true;
            this.fl.ImageCheckBoxOff = ((System.Drawing.Image)(resources.GetObject("fl.ImageCheckBoxOff")));
            this.fl.ImageCheckBoxOn = ((System.Drawing.Image)(resources.GetObject("fl.ImageCheckBoxOn")));
            this.fl.ImageCollapse = ((System.Drawing.Image)(resources.GetObject("fl.ImageCollapse")));
            this.fl.ImageDefaultIcon = ((System.Drawing.Image)(resources.GetObject("fl.ImageDefaultIcon")));
            this.fl.ImageExpand = ((System.Drawing.Image)(resources.GetObject("fl.ImageExpand")));
            this.fl.IsEditMode = false;
            this.fl.ItemCount = 10000;
            this.fl.ItemHeightDefault = 18;
            this.fl.ItemIndentDefault = 17;
            this.fl.Location = new System.Drawing.Point(12, 12);
            this.fl.MultiSelect = true;
            this.fl.Name = "fl";
            this.fl.ShowIcons = true;
            this.fl.Size = new System.Drawing.Size(428, 418);
            this.fl.TabIndex = 0;
            this.fl.ItemHeightNeeded += new System.EventHandler<FastTreeNS.IntItemEventArgs>(this.fl_ItemHeightNeeded);
            this.fl.ItemIndentNeeded += new System.EventHandler<FastTreeNS.IntItemEventArgs>(this.fl_ItemIndentNeeded);
            this.fl.ItemTextNeeded += new System.EventHandler<FastTreeNS.StringItemEventArgs>(this.fl_ItemTextNeeded);
            this.fl.ItemIconNeeded += new System.EventHandler<FastTreeNS.ImageItemEventArgs>(this.fl_ItemIconNeeded);
            this.fl.ItemBackColorNeeded += new System.EventHandler<FastTreeNS.ColorItemEventArgs>(this.fl_ItemBackColorNeeded);
            this.fl.ItemForeColorNeeded += new System.EventHandler<FastTreeNS.ColorItemEventArgs>(this.fl_ItemForeColorNeeded);
            this.fl.PaintItem += new System.EventHandler<FastTreeNS.PaintItemContentEventArgs>(this.fl_PaintItem);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "play_button_blue_16_2.png");
            // 
            // FastListCustomize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 442);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.fl);
            this.Name = "FastListCustomize";
            this.Text = "FastListCustomize";
            this.ResumeLayout(false);

        }

        #endregion

        private FastTreeNS.FastList fl;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ImageList imageList1;
    }
}