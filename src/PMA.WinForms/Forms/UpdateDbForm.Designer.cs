namespace PMA.WinForms.Forms
{
    partial class UpdateDbForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateDbForm));
            this.ImportMorphCombinationsCheckBox = new System.Windows.Forms.CheckBox();
            this.ImportSandhiRulesCheckBox = new System.Windows.Forms.CheckBox();
            this.ImportMorphRulesCheckBox = new System.Windows.Forms.CheckBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.OpenFileLabel = new System.Windows.Forms.Label();
            this.TempOpenFileComboBox = new System.Windows.Forms.ComboBox();
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.TempLogListView = new System.Windows.Forms.ListView();
            this.TimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EventColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClearLogButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.DataTableGroupBox = new System.Windows.Forms.GroupBox();
            this.ImportSandhiGroupsCheckBox = new System.Windows.Forms.CheckBox();
            this.LogLabel = new System.Windows.Forms.Label();
            this.ResetButton = new System.Windows.Forms.Button();
            this.DataTableGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImportMorphCombinationsCheckBox
            // 
            this.ImportMorphCombinationsCheckBox.AutoSize = true;
            this.ImportMorphCombinationsCheckBox.Checked = true;
            this.ImportMorphCombinationsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ImportMorphCombinationsCheckBox.Location = new System.Drawing.Point(6, 19);
            this.ImportMorphCombinationsCheckBox.Name = "ImportMorphCombinationsCheckBox";
            this.ImportMorphCombinationsCheckBox.Size = new System.Drawing.Size(125, 17);
            this.ImportMorphCombinationsCheckBox.TabIndex = 1;
            this.ImportMorphCombinationsCheckBox.Text = "Morph. Combinations";
            this.ImportMorphCombinationsCheckBox.UseVisualStyleBackColor = true;
            this.ImportMorphCombinationsCheckBox.CheckedChanged += new System.EventHandler(this.ImportMorphCombinationsCheckBox_CheckedChanged);
            // 
            // ImportSandhiRulesCheckBox
            // 
            this.ImportSandhiRulesCheckBox.AutoSize = true;
            this.ImportSandhiRulesCheckBox.Checked = true;
            this.ImportSandhiRulesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ImportSandhiRulesCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ImportSandhiRulesCheckBox.Location = new System.Drawing.Point(163, 44);
            this.ImportSandhiRulesCheckBox.Name = "ImportSandhiRulesCheckBox";
            this.ImportSandhiRulesCheckBox.Size = new System.Drawing.Size(127, 17);
            this.ImportSandhiRulesCheckBox.TabIndex = 3;
            this.ImportSandhiRulesCheckBox.Text = "SandhiGroupId Rules";
            this.ImportSandhiRulesCheckBox.UseVisualStyleBackColor = true;
            this.ImportSandhiRulesCheckBox.CheckedChanged += new System.EventHandler(this.ImportSandhiRulesCheckBox_CheckedChanged);
            // 
            // ImportMorphRulesCheckBox
            // 
            this.ImportMorphRulesCheckBox.AutoSize = true;
            this.ImportMorphRulesCheckBox.Checked = true;
            this.ImportMorphRulesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ImportMorphRulesCheckBox.Location = new System.Drawing.Point(6, 42);
            this.ImportMorphRulesCheckBox.Name = "ImportMorphRulesCheckBox";
            this.ImportMorphRulesCheckBox.Size = new System.Drawing.Size(89, 17);
            this.ImportMorphRulesCheckBox.TabIndex = 4;
            this.ImportMorphRulesCheckBox.Text = "Morph. Rules";
            this.ImportMorphRulesCheckBox.UseVisualStyleBackColor = true;
            this.ImportMorphRulesCheckBox.CheckedChanged += new System.EventHandler(this.ImportMorphRulesCheckBox_CheckedChanged);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(352, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // OpenFileLabel
            // 
            this.OpenFileLabel.AutoSize = true;
            this.OpenFileLabel.Location = new System.Drawing.Point(12, 82);
            this.OpenFileLabel.Name = "OpenFileLabel";
            this.OpenFileLabel.Size = new System.Drawing.Size(160, 13);
            this.OpenFileLabel.TabIndex = 8;
            this.OpenFileLabel.Text = "Open file with PMA tables (.xlsx):";
            // 
            // TempOpenFileComboBox
            // 
            this.TempOpenFileComboBox.FormattingEnabled = true;
            this.TempOpenFileComboBox.Location = new System.Drawing.Point(12, 101);
            this.TempOpenFileComboBox.Name = "TempOpenFileComboBox";
            this.TempOpenFileComboBox.Size = new System.Drawing.Size(334, 21);
            this.TempOpenFileComboBox.TabIndex = 9;
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(352, 99);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(75, 23);
            this.OpenFileButton.TabIndex = 7;
            this.OpenFileButton.Text = "Open File";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
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
            this.TempLogListView.TabIndex = 10;
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
            // ClearLogButton
            // 
            this.ClearLogButton.Location = new System.Drawing.Point(352, 128);
            this.ClearLogButton.Name = "ClearLogButton";
            this.ClearLogButton.Size = new System.Drawing.Size(75, 23);
            this.ClearLogButton.TabIndex = 11;
            this.ClearLogButton.Text = "Clear Log";
            this.ClearLogButton.UseVisualStyleBackColor = true;
            this.ClearLogButton.Click += new System.EventHandler(this.ClearLogButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(352, 41);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 12;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // DataTableGroupBox
            // 
            this.DataTableGroupBox.Controls.Add(this.ImportSandhiGroupsCheckBox);
            this.DataTableGroupBox.Controls.Add(this.ImportMorphCombinationsCheckBox);
            this.DataTableGroupBox.Controls.Add(this.ImportMorphRulesCheckBox);
            this.DataTableGroupBox.Controls.Add(this.ImportSandhiRulesCheckBox);
            this.DataTableGroupBox.Location = new System.Drawing.Point(12, 12);
            this.DataTableGroupBox.Name = "DataTableGroupBox";
            this.DataTableGroupBox.Size = new System.Drawing.Size(334, 67);
            this.DataTableGroupBox.TabIndex = 13;
            this.DataTableGroupBox.TabStop = false;
            this.DataTableGroupBox.Text = "Database tables";
            // 
            // ImportSandhiGroupsCheckBox
            // 
            this.ImportSandhiGroupsCheckBox.AutoSize = true;
            this.ImportSandhiGroupsCheckBox.Checked = true;
            this.ImportSandhiGroupsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ImportSandhiGroupsCheckBox.Location = new System.Drawing.Point(163, 19);
            this.ImportSandhiGroupsCheckBox.Name = "ImportSandhiGroupsCheckBox";
            this.ImportSandhiGroupsCheckBox.Size = new System.Drawing.Size(134, 17);
            this.ImportSandhiGroupsCheckBox.TabIndex = 17;
            this.ImportSandhiGroupsCheckBox.Text = "SandhiGroupId Groups";
            this.ImportSandhiGroupsCheckBox.UseVisualStyleBackColor = true;
            this.ImportSandhiGroupsCheckBox.CheckedChanged += new System.EventHandler(this.ImportSandhiGroupsCheckBox_CheckedChanged);
            // 
            // LogLabel
            // 
            this.LogLabel.AutoSize = true;
            this.LogLabel.Location = new System.Drawing.Point(12, 138);
            this.LogLabel.Name = "LogLabel";
            this.LogLabel.Size = new System.Drawing.Size(28, 13);
            this.LogLabel.TabIndex = 15;
            this.LogLabel.Text = "Log:";
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(352, 70);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 16;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // DbUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(439, 333);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.LogLabel);
            this.Controls.Add(this.DataTableGroupBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ClearLogButton);
            this.Controls.Add(this.TempLogListView);
            this.Controls.Add(this.TempOpenFileComboBox);
            this.Controls.Add(this.OpenFileLabel);
            this.Controls.Add(this.OpenFileButton);
            this.Controls.Add(this.StartButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(455, 4000);
            this.MinimumSize = new System.Drawing.Size(455, 372);
            this.Name = "UpdateDbForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update Database";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DbUpdateForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.DbUpdateForm_VisibleChanged);
            this.DataTableGroupBox.ResumeLayout(false);
            this.DataTableGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox ImportMorphCombinationsCheckBox;
        private System.Windows.Forms.CheckBox ImportSandhiRulesCheckBox;
        private System.Windows.Forms.CheckBox ImportMorphRulesCheckBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.Label OpenFileLabel;
        private System.Windows.Forms.ComboBox TempOpenFileComboBox;
        private System.Windows.Forms.ListView TempLogListView;
        private System.Windows.Forms.ColumnHeader TimeColumnHeader;
        private System.Windows.Forms.ColumnHeader DescriptionColumnHeader;
        private System.Windows.Forms.ColumnHeader EventColumnHeader;
        private System.Windows.Forms.Button ClearLogButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.GroupBox DataTableGroupBox;
        private System.Windows.Forms.Label LogLabel;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.CheckBox ImportSandhiGroupsCheckBox;
    }
}