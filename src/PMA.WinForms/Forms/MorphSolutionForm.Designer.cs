namespace PMA.WinForms.Forms
{
    partial class MorphSolutionForm
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
            this.SolutionTreeView = new System.Windows.Forms.TreeView();
            this.SandhiLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SolutionTreeView
            // 
            this.SolutionTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SolutionTreeView.Location = new System.Drawing.Point(12, 35);
            this.SolutionTreeView.Name = "SolutionTreeView";
            this.SolutionTreeView.ShowNodeToolTips = true;
            this.SolutionTreeView.Size = new System.Drawing.Size(160, 64);
            this.SolutionTreeView.TabIndex = 2;
            this.SolutionTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.SolutionTreeView_BeforeExpand);
            this.SolutionTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SolutionTreeView_AfterSelect);
            this.SolutionTreeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SolutionTreeView_MouseDoubleClick);
            // 
            // SandhiLabel
            // 
            this.SandhiLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SandhiLabel.Location = new System.Drawing.Point(12, 9);
            this.SandhiLabel.Name = "SandhiLabel";
            this.SandhiLabel.Size = new System.Drawing.Size(160, 23);
            this.SandhiLabel.TabIndex = 3;
            this.SandhiLabel.Text = "devo = deva + o";
            // 
            // MorphSolutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 111);
            this.Controls.Add(this.SandhiLabel);
            this.Controls.Add(this.SolutionTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(200, 150);
            this.Name = "MorphSolutionForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Morphlogical Solutions";
            this.LocationChanged += new System.EventHandler(this.MorphSolutionForm_LocationChanged);
            this.VisibleChanged += new System.EventHandler(this.MorphSolutionForm_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView SolutionTreeView;
        private System.Windows.Forms.Label SandhiLabel;
    }
}