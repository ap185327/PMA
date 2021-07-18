namespace PMA.WinForms.Forms
{
    partial class GetEntryIdForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetEntryIdForm));
            this.EntryListView = new System.Windows.Forms.ListView();
            this.IdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EntryColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InfoColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BaseColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LeftColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RightColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IsVirtualColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // EntryListView
            // 
            this.EntryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IdColumnHeader,
            this.EntryColumnHeader,
            this.InfoColumnHeader,
            this.BaseColumnHeader,
            this.LeftColumnHeader,
            this.RightColumnHeader,
            this.IsVirtualColumnHeader});
            this.EntryListView.ContextMenuStrip = this.ContextMenuStrip;
            this.EntryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntryListView.FullRowSelect = true;
            this.EntryListView.HideSelection = false;
            this.EntryListView.Location = new System.Drawing.Point(0, 0);
            this.EntryListView.Name = "EntryListView";
            this.EntryListView.Size = new System.Drawing.Size(442, 222);
            this.EntryListView.TabIndex = 0;
            this.EntryListView.UseCompatibleStateImageBehavior = false;
            this.EntryListView.View = System.Windows.Forms.View.Details;
            this.EntryListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.EntryListView_ItemSelectionChanged);
            this.EntryListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.EntryListView_MouseDoubleClick);
            // 
            // IdColumnHeader
            // 
            this.IdColumnHeader.Text = "ID";
            // 
            // EntryColumnHeader
            // 
            this.EntryColumnHeader.Text = "Entry";
            // 
            // InfoColumnHeader
            // 
            this.InfoColumnHeader.Text = "Morph-Info";
            this.InfoColumnHeader.Width = 225;
            // 
            // BaseColumnHeader
            // 
            this.BaseColumnHeader.Text = "Base";
            // 
            // LeftColumnHeader
            // 
            this.LeftColumnHeader.Text = "LeftEntry";
            // 
            // RightColumnHeader
            // 
            this.RightColumnHeader.Text = "RightEntry";
            // 
            // IsVirtualColumnHeader
            // 
            this.IsVirtualColumnHeader.Text = "IsVirtual";
            // 
            // ContextMenuStrip
            // 
            this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteToolStripMenuItem});
            this.ContextMenuStrip.Name = "contextMenuStrip";
            this.ContextMenuStrip.Size = new System.Drawing.Size(108, 26);
            // 
            // DeleteToolStripMenuItem
            // 
            this.DeleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("DeleteToolStripMenuItem.Image")));
            this.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            this.DeleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.DeleteToolStripMenuItem.Text = "Delete";
            this.DeleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // GetEntryIdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 222);
            this.Controls.Add(this.EntryListView);
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "GetEntryIdForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GetIdForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetEntryIdForm_FormClosing);
            this.ContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView EntryListView;
        private System.Windows.Forms.ColumnHeader IdColumnHeader;
        private System.Windows.Forms.ColumnHeader EntryColumnHeader;
        private System.Windows.Forms.ColumnHeader InfoColumnHeader;
        private System.Windows.Forms.ColumnHeader BaseColumnHeader;
        private System.Windows.Forms.ColumnHeader LeftColumnHeader;
        private System.Windows.Forms.ColumnHeader RightColumnHeader;
        private System.Windows.Forms.ColumnHeader IsVirtualColumnHeader;
        private System.Windows.Forms.ToolStripMenuItem DeleteToolStripMenuItem;
    }
}