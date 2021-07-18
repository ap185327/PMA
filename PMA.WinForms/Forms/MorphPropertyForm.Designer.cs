namespace PMA.WinForms.Forms
{
    partial class MorphPropertyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MorphPropertyForm));
            this.GetEntryIdButton = new System.Windows.Forms.Button();
            this.RightIdLabel = new System.Windows.Forms.Label();
            this.TempEntryTextBox = new System.Windows.Forms.TextBox();
            this.TempRightTextBox = new System.Windows.Forms.TextBox();
            this.TempLeftTextBox = new System.Windows.Forms.TextBox();
            this.BaseLabel = new System.Windows.Forms.Label();
            this.GetRightIdButton = new System.Windows.Forms.Button();
            this.BaseComboBox = new System.Windows.Forms.ComboBox();
            this.GetLeftIdButton = new System.Windows.Forms.Button();
            this.LeftCheckBox = new System.Windows.Forms.CheckBox();
            this.EntryIdLabel = new System.Windows.Forms.Label();
            this.RightCheckBox = new System.Windows.Forms.CheckBox();
            this.LeftIdLabel = new System.Windows.Forms.Label();
            this.TempPropertyGridParameters = new System.Windows.Forms.PropertyGrid();
            this.IsVirtualCheckBox = new System.Windows.Forms.CheckBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.LockIdCheckBox = new System.Windows.Forms.CheckBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetEntryIdButton
            // 
            this.GetEntryIdButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GetEntryIdButton.Location = new System.Drawing.Point(246, 12);
            this.GetEntryIdButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GetEntryIdButton.Name = "GetEntryIdButton";
            this.GetEntryIdButton.Size = new System.Drawing.Size(54, 27);
            this.GetEntryIdButton.TabIndex = 39;
            this.GetEntryIdButton.Text = "Get ID";
            this.GetEntryIdButton.UseVisualStyleBackColor = true;
            this.GetEntryIdButton.Click += new System.EventHandler(this.GetIdButton_Click);
            // 
            // RightIdLabel
            // 
            this.RightIdLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RightIdLabel.Location = new System.Drawing.Point(169, 110);
            this.RightIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RightIdLabel.Name = "RightIdLabel";
            this.RightIdLabel.Size = new System.Drawing.Size(70, 15);
            this.RightIdLabel.TabIndex = 38;
            this.RightIdLabel.Text = "ID: 123456";
            // 
            // TempEntryTextBox
            // 
            this.TempEntryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TempEntryTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.TempEntryTextBox.Location = new System.Drawing.Point(84, 14);
            this.TempEntryTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TempEntryTextBox.Name = "TempEntryTextBox";
            this.TempEntryTextBox.Size = new System.Drawing.Size(78, 23);
            this.TempEntryTextBox.TabIndex = 37;
            this.TempEntryTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // TempRightTextBox
            // 
            this.TempRightTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TempRightTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.TempRightTextBox.Location = new System.Drawing.Point(84, 105);
            this.TempRightTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TempRightTextBox.Name = "TempRightTextBox";
            this.TempRightTextBox.Size = new System.Drawing.Size(78, 23);
            this.TempRightTextBox.TabIndex = 26;
            this.TempRightTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // TempLeftTextBox
            // 
            this.TempLeftTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TempLeftTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.TempLeftTextBox.Location = new System.Drawing.Point(84, 75);
            this.TempLeftTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TempLeftTextBox.Name = "TempLeftTextBox";
            this.TempLeftTextBox.Size = new System.Drawing.Size(78, 23);
            this.TempLeftTextBox.TabIndex = 27;
            this.TempLeftTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // BaseLabel
            // 
            this.BaseLabel.AutoSize = true;
            this.BaseLabel.Location = new System.Drawing.Point(14, 47);
            this.BaseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BaseLabel.Name = "BaseLabel";
            this.BaseLabel.Size = new System.Drawing.Size(34, 15);
            this.BaseLabel.TabIndex = 28;
            this.BaseLabel.Text = "Base:";
            // 
            // GetRightIdButton
            // 
            this.GetRightIdButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GetRightIdButton.Location = new System.Drawing.Point(246, 103);
            this.GetRightIdButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GetRightIdButton.Name = "GetRightIdButton";
            this.GetRightIdButton.Size = new System.Drawing.Size(54, 27);
            this.GetRightIdButton.TabIndex = 35;
            this.GetRightIdButton.Text = "Get ID";
            this.GetRightIdButton.UseVisualStyleBackColor = true;
            this.GetRightIdButton.Click += new System.EventHandler(this.GetIdButton_Click);
            // 
            // BaseComboBox
            // 
            this.BaseComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BaseComboBox.FormattingEnabled = true;
            this.BaseComboBox.Location = new System.Drawing.Point(84, 44);
            this.BaseComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BaseComboBox.Name = "BaseComboBox";
            this.BaseComboBox.Size = new System.Drawing.Size(78, 23);
            this.BaseComboBox.TabIndex = 29;
            this.BaseComboBox.SelectedIndexChanged += new System.EventHandler(this.BaseComboBox_SelectedIndexChanged);
            // 
            // GetLeftIdButton
            // 
            this.GetLeftIdButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GetLeftIdButton.Location = new System.Drawing.Point(246, 72);
            this.GetLeftIdButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GetLeftIdButton.Name = "GetLeftIdButton";
            this.GetLeftIdButton.Size = new System.Drawing.Size(54, 27);
            this.GetLeftIdButton.TabIndex = 34;
            this.GetLeftIdButton.Text = "Get ID";
            this.GetLeftIdButton.UseVisualStyleBackColor = true;
            this.GetLeftIdButton.Click += new System.EventHandler(this.GetIdButton_Click);
            // 
            // LeftCheckBox
            // 
            this.LeftCheckBox.AutoSize = true;
            this.LeftCheckBox.Location = new System.Drawing.Point(14, 77);
            this.LeftCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LeftCheckBox.Name = "LeftCheckBox";
            this.LeftCheckBox.Size = new System.Drawing.Size(49, 19);
            this.LeftCheckBox.TabIndex = 30;
            this.LeftCheckBox.Text = "Left:";
            this.LeftCheckBox.UseVisualStyleBackColor = true;
            this.LeftCheckBox.CheckedChanged += new System.EventHandler(this.LeftCheckBox_CheckedChanged);
            this.LeftCheckBox.EnabledChanged += new System.EventHandler(this.LeftCheckBox_CheckedChanged);
            // 
            // EntryIdLabel
            // 
            this.EntryIdLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EntryIdLabel.Location = new System.Drawing.Point(169, 17);
            this.EntryIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.EntryIdLabel.Name = "EntryIdLabel";
            this.EntryIdLabel.Size = new System.Drawing.Size(70, 15);
            this.EntryIdLabel.TabIndex = 33;
            this.EntryIdLabel.Text = "ID: 123456";
            // 
            // RightCheckBox
            // 
            this.RightCheckBox.AutoSize = true;
            this.RightCheckBox.Location = new System.Drawing.Point(14, 108);
            this.RightCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RightCheckBox.Name = "RightCheckBox";
            this.RightCheckBox.Size = new System.Drawing.Size(57, 19);
            this.RightCheckBox.TabIndex = 31;
            this.RightCheckBox.Text = "Right:";
            this.RightCheckBox.UseVisualStyleBackColor = true;
            this.RightCheckBox.CheckedChanged += new System.EventHandler(this.RightCheckBox_CheckedChanged);
            this.RightCheckBox.EnabledChanged += new System.EventHandler(this.RightCheckBox_CheckedChanged);
            // 
            // LeftIdLabel
            // 
            this.LeftIdLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LeftIdLabel.Location = new System.Drawing.Point(169, 78);
            this.LeftIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LeftIdLabel.Name = "LeftIdLabel";
            this.LeftIdLabel.Size = new System.Drawing.Size(70, 15);
            this.LeftIdLabel.TabIndex = 32;
            this.LeftIdLabel.Text = "ID: 123456";
            // 
            // TempPropertyGridParameters
            // 
            this.TempPropertyGridParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TempPropertyGridParameters.Location = new System.Drawing.Point(14, 136);
            this.TempPropertyGridParameters.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TempPropertyGridParameters.Name = "TempPropertyGridParameters";
            this.TempPropertyGridParameters.Size = new System.Drawing.Size(287, 339);
            this.TempPropertyGridParameters.TabIndex = 44;
            // 
            // IsVirtualCheckBox
            // 
            this.IsVirtualCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.IsVirtualCheckBox.AutoSize = true;
            this.IsVirtualCheckBox.Location = new System.Drawing.Point(14, 488);
            this.IsVirtualCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.IsVirtualCheckBox.Name = "IsVirtualCheckBox";
            this.IsVirtualCheckBox.Size = new System.Drawing.Size(60, 19);
            this.IsVirtualCheckBox.TabIndex = 45;
            this.IsVirtualCheckBox.Text = "Virtual";
            this.IsVirtualCheckBox.UseVisualStyleBackColor = true;
            this.IsVirtualCheckBox.CheckedChanged += new System.EventHandler(this.IsVirtualCheckBox_CheckedChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.Location = new System.Drawing.Point(145, 482);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 27);
            this.SaveButton.TabIndex = 41;
            this.SaveButton.Text = "Save";
            this.SaveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ResetButton.Location = new System.Drawing.Point(246, 42);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(54, 27);
            this.ResetButton.TabIndex = 25;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // LockIdCheckBox
            // 
            this.LockIdCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LockIdCheckBox.AutoSize = true;
            this.LockIdCheckBox.Location = new System.Drawing.Point(169, 47);
            this.LockIdCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LockIdCheckBox.Name = "LockIdCheckBox";
            this.LockIdCheckBox.Size = new System.Drawing.Size(65, 19);
            this.LockIdCheckBox.TabIndex = 48;
            this.LockIdCheckBox.Text = "Lock ID";
            this.LockIdCheckBox.UseVisualStyleBackColor = true;
            this.LockIdCheckBox.CheckedChanged += new System.EventHandler(this.LockIdCheckBox_CheckedChanged);
            // 
            // StartButton
            // 
            this.StartButton.Image = ((System.Drawing.Image)(resources.GetObject("StartButton.Image")));
            this.StartButton.Location = new System.Drawing.Point(14, 12);
            this.StartButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(63, 27);
            this.StartButton.TabIndex = 49;
            this.StartButton.Text = "Start";
            this.StartButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.Location = new System.Drawing.Point(226, 482);
            this.DeleteButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 27);
            this.DeleteButton.TabIndex = 42;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // MorphPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 516);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.LockIdCheckBox);
            this.Controls.Add(this.IsVirtualCheckBox);
            this.Controls.Add(this.TempPropertyGridParameters);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.GetEntryIdButton);
            this.Controls.Add(this.RightIdLabel);
            this.Controls.Add(this.TempEntryTextBox);
            this.Controls.Add(this.TempRightTextBox);
            this.Controls.Add(this.TempLeftTextBox);
            this.Controls.Add(this.BaseLabel);
            this.Controls.Add(this.GetRightIdButton);
            this.Controls.Add(this.BaseComboBox);
            this.Controls.Add(this.GetLeftIdButton);
            this.Controls.Add(this.LeftCheckBox);
            this.Controls.Add(this.EntryIdLabel);
            this.Controls.Add(this.RightCheckBox);
            this.Controls.Add(this.LeftIdLabel);
            this.Controls.Add(this.ResetButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(289, 294);
            this.Name = "MorphPropertyForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Morpological Properties";
            this.LocationChanged += new System.EventHandler(this.MorphPropertyForm_LocationChanged);
            this.VisibleChanged += new System.EventHandler(this.MorphPropertyForm_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button GetEntryIdButton;
        private System.Windows.Forms.Label RightIdLabel;
        private System.Windows.Forms.TextBox TempEntryTextBox;
        private System.Windows.Forms.TextBox TempRightTextBox;
        private System.Windows.Forms.TextBox TempLeftTextBox;
        private System.Windows.Forms.Label BaseLabel;
        private System.Windows.Forms.Button GetRightIdButton;
        private System.Windows.Forms.ComboBox BaseComboBox;
        private System.Windows.Forms.Button GetLeftIdButton;
        private System.Windows.Forms.CheckBox LeftCheckBox;
        private System.Windows.Forms.Label EntryIdLabel;
        private System.Windows.Forms.CheckBox RightCheckBox;
        private System.Windows.Forms.Label LeftIdLabel;
        private System.Windows.Forms.PropertyGrid TempPropertyGridParameters;
        private System.Windows.Forms.CheckBox IsVirtualCheckBox;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.CheckBox LockIdCheckBox;
        private System.Windows.Forms.Button StartButton;
    }
}