namespace Tester
{
    partial class FastTreeFileExplorerSample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FastTreeFileExplorerSample));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.ft = new FastTreeNS.FastTree();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(446, 14);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid1.SelectedObject = this.ft;
            this.propertyGrid1.Size = new System.Drawing.Size(245, 416);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // ft
            // 
            this.ft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ft.AutoScroll = true;
            this.ft.AutoScrollMinSize = new System.Drawing.Size(0, 62);
            this.ft.BackColor = System.Drawing.SystemColors.Window;
            this.ft.IsEditMode = false;
            this.ft.ItemCount = 3;
            this.ft.Location = new System.Drawing.Point(12, 14);
            this.ft.MultiSelect = true;
            this.ft.Name = "ft";
            this.ft.ShowExpandBoxes = true;
            this.ft.ShowIcons = true;
            this.ft.Size = new System.Drawing.Size(428, 416);
            this.ft.TabIndex = 2;
            this.ft.NodeIconNeeded += new System.EventHandler<FastTreeNS.ImageNodeEventArgs>(this.ft_NodeIconNeeded);
            // 
            // FastTreeFileExplorerSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 442);
            this.Controls.Add(this.ft);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "FastTreeFileExplorerSample";
            this.Text = "FastTreeFileExplorerSample";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private FastTreeNS.FastTree ft;
    }
}