namespace PMA.WinForms.Forms
{
    partial class OptionForm
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
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.TermGoupBox = new System.Windows.Forms.GroupBox();
            this.ShownTermLabel = new System.Windows.Forms.Label();
            this.AvailableTermLabel = new System.Windows.Forms.Label();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.ShownListBox = new System.Windows.Forms.ListBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.AvailableListBox = new System.Windows.Forms.ListBox();
            this.RatingGroupBox = new System.Windows.Forms.GroupBox();
            this.MorphRuleRatingLabel = new System.Windows.Forms.Label();
            this.FdictRatingLabel = new System.Windows.Forms.Label();
            this.RatingTrackBar = new System.Windows.Forms.TrackBar();
            this.RootTermComboBox = new System.Windows.Forms.ComboBox();
            this.RootTermLabel = new System.Windows.Forms.Label();
            this.ModeLabel = new System.Windows.Forms.Label();
            this.ModeComboBox = new System.Windows.Forms.ComboBox();
            this.TermGoupBox.SuspendLayout();
            this.RatingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RatingTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(430, 436);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(82, 30);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(340, 436);
            this.OkButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(82, 30);
            this.OkButton.TabIndex = 3;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // TermGoupBox
            // 
            this.TermGoupBox.Controls.Add(this.ShownTermLabel);
            this.TermGoupBox.Controls.Add(this.AvailableTermLabel);
            this.TermGoupBox.Controls.Add(this.RemoveButton);
            this.TermGoupBox.Controls.Add(this.ShownListBox);
            this.TermGoupBox.Controls.Add(this.AddButton);
            this.TermGoupBox.Controls.Add(this.AvailableListBox);
            this.TermGoupBox.Location = new System.Drawing.Point(13, 47);
            this.TermGoupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TermGoupBox.Name = "TermGoupBox";
            this.TermGoupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TermGoupBox.Size = new System.Drawing.Size(499, 270);
            this.TermGoupBox.TabIndex = 10;
            this.TermGoupBox.TabStop = false;
            this.TermGoupBox.Text = "Morphological Solution terms";
            // 
            // ShownTermLabel
            // 
            this.ShownTermLabel.AutoSize = true;
            this.ShownTermLabel.Location = new System.Drawing.Point(296, 28);
            this.ShownTermLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ShownTermLabel.Name = "ShownTermLabel";
            this.ShownTermLabel.Size = new System.Drawing.Size(100, 15);
            this.ShownTermLabel.TabIndex = 15;
            this.ShownTermLabel.Text = "Show these terms";
            // 
            // AvailableTermLabel
            // 
            this.AvailableTermLabel.AutoSize = true;
            this.AvailableTermLabel.Location = new System.Drawing.Point(7, 28);
            this.AvailableTermLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AvailableTermLabel.Name = "AvailableTermLabel";
            this.AvailableTermLabel.Size = new System.Drawing.Size(88, 15);
            this.AvailableTermLabel.TabIndex = 14;
            this.AvailableTermLabel.Text = "Available terms";
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(205, 80);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(88, 27);
            this.RemoveButton.TabIndex = 13;
            this.RemoveButton.Text = "<-- Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // ShownListBox
            // 
            this.ShownListBox.FormattingEnabled = true;
            this.ShownListBox.ItemHeight = 15;
            this.ShownListBox.Location = new System.Drawing.Point(300, 46);
            this.ShownListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ShownListBox.Name = "ShownListBox";
            this.ShownListBox.Size = new System.Drawing.Size(191, 214);
            this.ShownListBox.Sorted = true;
            this.ShownListBox.TabIndex = 11;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(205, 46);
            this.AddButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(88, 27);
            this.AddButton.TabIndex = 12;
            this.AddButton.Text = "Add -->";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // AvailableListBox
            // 
            this.AvailableListBox.FormattingEnabled = true;
            this.AvailableListBox.ItemHeight = 15;
            this.AvailableListBox.Items.AddRange(new object[] {
            "Case",
            "Formation",
            "Gender",
            "IsIrregular",
            "IsName",
            "IsNegative",
            "Language",
            "Mode",
            "Number",
            "Parent",
            "Part",
            "Person",
            "PoS1",
            "PoS2",
            "PoS3",
            "Tense",
            "Type1",
            "Type2",
            "Type3",
            "Voice",
            "WithAugment"});
            this.AvailableListBox.Location = new System.Drawing.Point(7, 46);
            this.AvailableListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.AvailableListBox.Name = "AvailableListBox";
            this.AvailableListBox.Size = new System.Drawing.Size(191, 214);
            this.AvailableListBox.Sorted = true;
            this.AvailableListBox.TabIndex = 10;
            // 
            // RatingGroupBox
            // 
            this.RatingGroupBox.Controls.Add(this.MorphRuleRatingLabel);
            this.RatingGroupBox.Controls.Add(this.FdictRatingLabel);
            this.RatingGroupBox.Controls.Add(this.RatingTrackBar);
            this.RatingGroupBox.Location = new System.Drawing.Point(13, 323);
            this.RatingGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RatingGroupBox.Name = "RatingGroupBox";
            this.RatingGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RatingGroupBox.Size = new System.Drawing.Size(499, 100);
            this.RatingGroupBox.TabIndex = 15;
            this.RatingGroupBox.TabStop = false;
            this.RatingGroupBox.Text = "Solution Rating";
            // 
            // MorphRuleRatingLabel
            // 
            this.MorphRuleRatingLabel.AutoSize = true;
            this.MorphRuleRatingLabel.Location = new System.Drawing.Point(4, 30);
            this.MorphRuleRatingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MorphRuleRatingLabel.Name = "MorphRuleRatingLabel";
            this.MorphRuleRatingLabel.Size = new System.Drawing.Size(102, 15);
            this.MorphRuleRatingLabel.TabIndex = 19;
            this.MorphRuleRatingLabel.Text = "Morph. rules: 75%";
            // 
            // FdictRatingLabel
            // 
            this.FdictRatingLabel.AutoSize = true;
            this.FdictRatingLabel.Location = new System.Drawing.Point(373, 30);
            this.FdictRatingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FdictRatingLabel.Name = "FdictRatingLabel";
            this.FdictRatingLabel.Size = new System.Drawing.Size(118, 15);
            this.FdictRatingLabel.TabIndex = 17;
            this.FdictRatingLabel.Text = "Freq. Dictionary: 25%";
            // 
            // RatingTrackBar
            // 
            this.RatingTrackBar.Location = new System.Drawing.Point(7, 53);
            this.RatingTrackBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RatingTrackBar.Maximum = 20;
            this.RatingTrackBar.Name = "RatingTrackBar";
            this.RatingTrackBar.Size = new System.Drawing.Size(484, 45);
            this.RatingTrackBar.TabIndex = 15;
            this.RatingTrackBar.Value = 17;
            this.RatingTrackBar.Scroll += new System.EventHandler(this.RatingTrackBar_Scroll);
            // 
            // RootTermComboBox
            // 
            this.RootTermComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RootTermComboBox.FormattingEnabled = true;
            this.RootTermComboBox.Location = new System.Drawing.Point(372, 9);
            this.RootTermComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RootTermComboBox.Name = "RootTermComboBox";
            this.RootTermComboBox.Size = new System.Drawing.Size(132, 23);
            this.RootTermComboBox.TabIndex = 17;
            // 
            // RootTermLabel
            // 
            this.RootTermLabel.AutoSize = true;
            this.RootTermLabel.Location = new System.Drawing.Point(267, 12);
            this.RootTermLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RootTermLabel.Name = "RootTermLabel";
            this.RootTermLabel.Size = new System.Drawing.Size(63, 15);
            this.RootTermLabel.TabIndex = 16;
            this.RootTermLabel.Text = "Root term:";
            // 
            // ModeLabel
            // 
            this.ModeLabel.AutoSize = true;
            this.ModeLabel.Location = new System.Drawing.Point(13, 12);
            this.ModeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ModeLabel.Name = "ModeLabel";
            this.ModeLabel.Size = new System.Drawing.Size(41, 15);
            this.ModeLabel.TabIndex = 18;
            this.ModeLabel.Text = "Mode:";
            // 
            // ModeComboBox
            // 
            this.ModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeComboBox.FormattingEnabled = true;
            this.ModeComboBox.Location = new System.Drawing.Point(79, 9);
            this.ModeComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(132, 23);
            this.ModeComboBox.TabIndex = 19;
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 475);
            this.Controls.Add(this.ModeComboBox);
            this.Controls.Add(this.ModeLabel);
            this.Controls.Add(this.RootTermComboBox);
            this.Controls.Add(this.RootTermLabel);
            this.Controls.Add(this.RatingGroupBox);
            this.Controls.Add(this.TermGoupBox);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "OptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.VisibleChanged += new System.EventHandler(this.OptionForm_VisibleChanged);
            this.TermGoupBox.ResumeLayout(false);
            this.TermGoupBox.PerformLayout();
            this.RatingGroupBox.ResumeLayout(false);
            this.RatingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RatingTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.GroupBox TermGoupBox;
        private System.Windows.Forms.Label ShownTermLabel;
        private System.Windows.Forms.Label AvailableTermLabel;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.ListBox ShownListBox;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.ListBox AvailableListBox;
        private System.Windows.Forms.GroupBox RatingGroupBox;
        private System.Windows.Forms.Label FdictRatingLabel;
        private System.Windows.Forms.TrackBar RatingTrackBar;
        private System.Windows.Forms.Label MorphRuleRatingLabel;
        private System.Windows.Forms.ComboBox RootTermComboBox;
        private System.Windows.Forms.Label RootTermLabel;
        private System.Windows.Forms.Label ModeLabel;
        private System.Windows.Forms.ComboBox ModeComboBox;
    }
}