namespace PMA.WinForms.Forms
{
    partial class ImportEntryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportEntryForm));
            this.StartButton = new System.Windows.Forms.Button();
            this.OpenFileLabel = new System.Windows.Forms.Label();
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.TempOpenFileComboBox = new System.Windows.Forms.ComboBox();
            this.LogLabel = new System.Windows.Forms.Label();
            this.ClearLogButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.AnalyzeProgressBar = new System.Windows.Forms.ProgressBar();
            this.AnalyzeBeforeImportCheckBox = new System.Windows.Forms.CheckBox();
            this.TempLogListView = new System.Windows.Forms.ListView();
            this.TimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EventColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(352, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // OpenFileLabel
            // 
            this.OpenFileLabel.AutoSize = true;
            this.OpenFileLabel.Location = new System.Drawing.Point(12, 82);
            this.OpenFileLabel.Name = "OpenFileLabel";
            this.OpenFileLabel.Size = new System.Drawing.Size(205, 13);
            this.OpenFileLabel.TabIndex = 1;
            this.OpenFileLabel.Text = "Open file with morphological entries (.xlsx):";
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(352, 99);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(75, 23);
            this.OpenFileButton.TabIndex = 2;
            this.OpenFileButton.Text = "Open File";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // TempOpenFileComboBox
            // 
            this.TempOpenFileComboBox.FormattingEnabled = true;
            this.TempOpenFileComboBox.Location = new System.Drawing.Point(12, 101);
            this.TempOpenFileComboBox.Name = "TempOpenFileComboBox";
            this.TempOpenFileComboBox.Size = new System.Drawing.Size(334, 21);
            this.TempOpenFileComboBox.TabIndex = 3;
            // 
            // LogLabel
            // 
            this.LogLabel.AutoSize = true;
            this.LogLabel.Location = new System.Drawing.Point(12, 138);
            this.LogLabel.Name = "LogLabel";
            this.LogLabel.Size = new System.Drawing.Size(28, 13);
            this.LogLabel.TabIndex = 4;
            this.LogLabel.Text = "Log:";
            // 
            // ClearLogButton
            // 
            this.ClearLogButton.Location = new System.Drawing.Point(352, 128);
            this.ClearLogButton.Name = "ClearLogButton";
            this.ClearLogButton.Size = new System.Drawing.Size(75, 23);
            this.ClearLogButton.TabIndex = 5;
            this.ClearLogButton.Text = "Clear Log";
            this.ClearLogButton.UseVisualStyleBackColor = true;
            this.ClearLogButton.Click += new System.EventHandler(this.ClearLogButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(352, 41);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // AnalyzeProgressBar
            // 
            this.AnalyzeProgressBar.Location = new System.Drawing.Point(12, 41);
            this.AnalyzeProgressBar.Name = "AnalyzeProgressBar";
            this.AnalyzeProgressBar.Size = new System.Drawing.Size(334, 23);
            this.AnalyzeProgressBar.Step = 1;
            this.AnalyzeProgressBar.TabIndex = 12;
            // 
            // AnalyzeBeforeImportCheckBox
            // 
            this.AnalyzeBeforeImportCheckBox.AutoSize = true;
            this.AnalyzeBeforeImportCheckBox.Location = new System.Drawing.Point(12, 16);
            this.AnalyzeBeforeImportCheckBox.Name = "AnalyzeBeforeImportCheckBox";
            this.AnalyzeBeforeImportCheckBox.Size = new System.Drawing.Size(175, 17);
            this.AnalyzeBeforeImportCheckBox.TabIndex = 13;
            this.AnalyzeBeforeImportCheckBox.Text = "Analyze entries before importing";
            this.AnalyzeBeforeImportCheckBox.UseVisualStyleBackColor = true;
            // 
            // TempLogListView
            // 
            this.TempLogListView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.TempLogListView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.TempLogListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TimeColumnHeader,
            this.EventColumnHeader,
            this.DescriptionColumnHeader});
            this.TempLogListView.FullRowSelect = true;
            this.TempLogListView.HideSelection = false;
            this.TempLogListView.Location = new System.Drawing.Point(12, 157);
            this.TempLogListView.Name = "TempLogListView";
            this.TempLogListView.Size = new System.Drawing.Size(415, 164);
            this.TempLogListView.TabIndex = 14;
            this.TempLogListView.UseCompatibleStateImageBehavior = false;
            this.TempLogListView.View = System.Windows.Forms.View.Details;
            // 
            // TimeColumnHeader
            // 
            this.TimeColumnHeader.Text = "Time";
            this.TimeColumnHeader.Width = 75;
            // 
            // EventColumnHeader
            // 
            this.EventColumnHeader.Text = "Event";
            this.EventColumnHeader.Width = 54;
            // 
            // DescriptionColumnHeader
            // 
            this.DescriptionColumnHeader.Text = "Description";
            this.DescriptionColumnHeader.Width = 265;
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(352, 70);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 7;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // ImportEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 333);
            this.Controls.Add(this.TempLogListView);
            this.Controls.Add(this.AnalyzeBeforeImportCheckBox);
            this.Controls.Add(this.AnalyzeProgressBar);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ClearLogButton);
            this.Controls.Add(this.LogLabel);
            this.Controls.Add(this.TempOpenFileComboBox);
            this.Controls.Add(this.OpenFileButton);
            this.Controls.Add(this.OpenFileLabel);
            this.Controls.Add(this.StartButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(455, 4000);
            this.MinimumSize = new System.Drawing.Size(455, 372);
            this.Name = "ImportEntryForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import morphological entries";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportEntryForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.ImportEntryForm_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label OpenFileLabel;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.ComboBox TempOpenFileComboBox;
        private System.Windows.Forms.Label LogLabel;
        private System.Windows.Forms.Button ClearLogButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ProgressBar AnalyzeProgressBar;
        private System.Windows.Forms.CheckBox AnalyzeBeforeImportCheckBox;
        private System.Windows.Forms.ListView TempLogListView;
        private System.Windows.Forms.ColumnHeader TimeColumnHeader;
        private System.Windows.Forms.ColumnHeader EventColumnHeader;
        private System.Windows.Forms.ColumnHeader DescriptionColumnHeader;
        private System.Windows.Forms.Button ResetButton;
    }
}