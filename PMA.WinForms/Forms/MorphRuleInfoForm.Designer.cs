namespace PMA.WinForms.Forms
{
    partial class MorphRuleInfoForm
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Morphological Rules", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("SandhiGroupId Rules", System.Windows.Forms.HorizontalAlignment.Left);
            this.RuleListView = new System.Windows.Forms.ListView();
            this.IdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // RuleListView
            // 
            this.RuleListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RuleListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IdColumnHeader,
            this.DescriptionColumnHeader});
            listViewGroup1.Header = "Morphological Rules";
            listViewGroup1.Name = "MorphRuleListViewGroup";
            listViewGroup2.Header = "SandhiGroupId Rules";
            listViewGroup2.Name = "SandhiRuleListViewGroup";
            this.RuleListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.RuleListView.HideSelection = false;
            this.RuleListView.Location = new System.Drawing.Point(12, 12);
            this.RuleListView.MultiSelect = false;
            this.RuleListView.Name = "RuleListView";
            this.RuleListView.ShowItemToolTips = true;
            this.RuleListView.Size = new System.Drawing.Size(141, 87);
            this.RuleListView.TabIndex = 7;
            this.RuleListView.UseCompatibleStateImageBehavior = false;
            this.RuleListView.View = System.Windows.Forms.View.Details;
            this.RuleListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.RuleListView_ColumnWidthChanged);
            // 
            // IdColumnHeader
            // 
            this.IdColumnHeader.Text = "ID";
            this.IdColumnHeader.Width = 45;
            // 
            // DescriptionColumnHeader
            // 
            this.DescriptionColumnHeader.Text = "Description";
            this.DescriptionColumnHeader.Width = 335;
            // 
            // MorphRuleInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(164, 111);
            this.Controls.Add(this.RuleListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(180, 150);
            this.Name = "MorphRuleInfoForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Morphological Rule Information";
            this.LocationChanged += new System.EventHandler(this.MorphRuleInfoForm_LocationChanged);
            this.VisibleChanged += new System.EventHandler(this.MorphRuleInfoForm_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView RuleListView;
        private System.Windows.Forms.ColumnHeader IdColumnHeader;
        private System.Windows.Forms.ColumnHeader DescriptionColumnHeader;
    }
}