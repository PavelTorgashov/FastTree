namespace Tester
{
    partial class FastListReadonlyAndDisabledItemsSample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FastListReadonlyAndDisabledItemsSample));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.fl = new FastTreeNS.FastList();
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
            this.fl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fl.AutoScroll = true;
            this.fl.AutoScrollMinSize = new System.Drawing.Size(0, 2002);
            this.fl.BackColor = System.Drawing.SystemColors.Window;
            this.fl.FullItemSelect = true;
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
            this.fl.ShowCheckBoxes = true;
            this.fl.Size = new System.Drawing.Size(428, 418);
            this.fl.TabIndex = 0;
            this.fl.ItemTextNeeded += new System.EventHandler<FastTreeNS.StringItemEventArgs>(this.fl_ItemTextNeeded);
            this.fl.ItemForeColorNeeded += new System.EventHandler<FastTreeNS.ColorItemEventArgs>(this.fl_ItemForeColorNeeded);
            this.fl.CanUnselectItemNeeded += new System.EventHandler<FastTreeNS.BoolItemEventArgs>(this.fl_CanSelectItemNeeded);
            this.fl.CanSelectItemNeeded += new System.EventHandler<FastTreeNS.BoolItemEventArgs>(this.fl_CanSelectItemNeeded);
            this.fl.CanUncheckItemNeeded += new System.EventHandler<FastTreeNS.BoolItemEventArgs>(this.fl_CanCheckItemNeeded);
            this.fl.CanCheckItemNeeded += new System.EventHandler<FastTreeNS.BoolItemEventArgs>(this.fl_CanCheckItemNeeded);
            this.fl.CanExpandItemNeeded += new System.EventHandler<FastTreeNS.BoolItemEventArgs>(this.fl_CanSelectItemNeeded);
            this.fl.CanCollapseItemNeeded += new System.EventHandler<FastTreeNS.BoolItemEventArgs>(this.fl_CanSelectItemNeeded);
            // 
            // FastListReadonlyAndDisabledItemsSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 442);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.fl);
            this.Name = "FastListReadonlyAndDisabledItemsSample";
            this.Text = "FastListReadonlyAndDisabledItemsSample";
            this.ResumeLayout(false);

        }

        #endregion

        private FastTreeNS.FastList fl;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}