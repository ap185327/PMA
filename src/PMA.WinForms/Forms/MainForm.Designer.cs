
namespace PMA.WinForms.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportCatalogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowMorphPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowMorphRuleInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowMorphSolutionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.AutoSymbolReplaceStripButton = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.LayerToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.LayerToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.DepthToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.StartToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.MorphologicalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SyntacticToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TempToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.AnimToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StateToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuStrip.SuspendLayout();
            this.ToolStrip.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.ToolsToolStripMenuItem,
            this.WindowsToolStripMenuItem,
            this.HelpToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(971, 24);
            this.MenuStrip.TabIndex = 0;
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(132, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportEntriesToolStripMenuItem,
            this.ImportCatalogsToolStripMenuItem,
            this.OptionsToolStripMenuItem});
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.ToolsToolStripMenuItem.Text = "Tools";
            // 
            // ImportEntriesToolStripMenuItem
            // 
            this.ImportEntriesToolStripMenuItem.Name = "ImportEntriesToolStripMenuItem";
            this.ImportEntriesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.ImportEntriesToolStripMenuItem.Text = "Import Entries";
            this.ImportEntriesToolStripMenuItem.Click += new System.EventHandler(this.ImportEntryToolStripMenuItem_Click);
            // 
            // ImportCatalogsToolStripMenuItem
            // 
            this.ImportCatalogsToolStripMenuItem.Name = "ImportCatalogsToolStripMenuItem";
            this.ImportCatalogsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.ImportCatalogsToolStripMenuItem.Text = "Import Catalogs";
            this.ImportCatalogsToolStripMenuItem.Click += new System.EventHandler(this.ImportCatalogToolStripMenuItem_Click);
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.OptionsToolStripMenuItem.Text = "Options";
            this.OptionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // WindowsToolStripMenuItem
            // 
            this.WindowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowMorphPropertiesToolStripMenuItem,
            this.ShowMorphRuleInfoToolStripMenuItem,
            this.ShowMorphSolutionsToolStripMenuItem});
            this.WindowsToolStripMenuItem.Name = "WindowsToolStripMenuItem";
            this.WindowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.WindowsToolStripMenuItem.Text = "Windows";
            // 
            // ShowMorphPropertiesToolStripMenuItem
            // 
            this.ShowMorphPropertiesToolStripMenuItem.Name = "ShowMorphPropertiesToolStripMenuItem";
            this.ShowMorphPropertiesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ShowMorphPropertiesToolStripMenuItem.Text = "Show Morph. Properties";
            this.ShowMorphPropertiesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ShowMorphPropertiesToolStripMenuItem_CheckedChanged);
            this.ShowMorphPropertiesToolStripMenuItem.Click += new System.EventHandler(this.ShowMorphPropertiesToolStripMenuItem_Click);
            // 
            // ShowMorphRuleInfoToolStripMenuItem
            // 
            this.ShowMorphRuleInfoToolStripMenuItem.Name = "ShowMorphRuleInfoToolStripMenuItem";
            this.ShowMorphRuleInfoToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ShowMorphRuleInfoToolStripMenuItem.Text = "Show Morph. Rule Info";
            this.ShowMorphRuleInfoToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ShowMorphRuleInfoToolStripMenuItem_CheckedChanged);
            this.ShowMorphRuleInfoToolStripMenuItem.Click += new System.EventHandler(this.ShowMorphRuleInfoToolStripMenuItem_Click);
            // 
            // ShowMorphSolutionsToolStripMenuItem
            // 
            this.ShowMorphSolutionsToolStripMenuItem.Name = "ShowMorphSolutionsToolStripMenuItem";
            this.ShowMorphSolutionsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.ShowMorphSolutionsToolStripMenuItem.Text = "Show Morph. Solutions";
            this.ShowMorphSolutionsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.ShowMorphSolutionsToolStripMenuItem_CheckedChanged);
            this.ShowMorphSolutionsToolStripMenuItem.Click += new System.EventHandler(this.ShowMorphSolutionsToolStripMenuItem_Click);
            // 
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutToolStripMenuItem});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            this.HelpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpToolStripMenuItem.Text = "Help";
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.AboutToolStripMenuItem.Text = "About";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // ToolStrip
            // 
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AutoSymbolReplaceStripButton,
            this.ToolStripSeparator,
            this.LayerToolStripLabel,
            this.LayerToolStripComboBox,
            this.DepthToolStripLabel,
            this.ToolStripSeparator2,
            this.StartToolStripSplitButton,
            this.TempToolStripTextBox});
            this.ToolStrip.Location = new System.Drawing.Point(0, 24);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(971, 25);
            this.ToolStrip.TabIndex = 2;
            // 
            // AutoSymbolReplaceStripButton
            // 
            this.AutoSymbolReplaceStripButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.AutoSymbolReplaceStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AutoSymbolReplaceStripButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.AutoSymbolReplaceStripButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AutoSymbolReplaceStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AutoSymbolReplaceStripButton.Name = "AutoSymbolReplaceStripButton";
            this.AutoSymbolReplaceStripButton.Size = new System.Drawing.Size(23, 22);
            this.AutoSymbolReplaceStripButton.Text = "aa";
            this.AutoSymbolReplaceStripButton.Click += new System.EventHandler(this.AutoSymbolReplaceStripButton_Click);
            // 
            // ToolStripSeparator
            // 
            this.ToolStripSeparator.Name = "ToolStripSeparator";
            this.ToolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // LayerToolStripLabel
            // 
            this.LayerToolStripLabel.Name = "LayerToolStripLabel";
            this.LayerToolStripLabel.Size = new System.Drawing.Size(38, 22);
            this.LayerToolStripLabel.Text = "Layer:";
            // 
            // LayerToolStripComboBox
            // 
            this.LayerToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LayerToolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.LayerToolStripComboBox.Name = "LayerToolStripComboBox";
            this.LayerToolStripComboBox.Size = new System.Drawing.Size(75, 25);
            this.LayerToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.LayerToolStripComboBox_SelectedIndexChanged);
            // 
            // DepthToolStripLabel
            // 
            this.DepthToolStripLabel.Name = "DepthToolStripLabel";
            this.DepthToolStripLabel.Size = new System.Drawing.Size(42, 22);
            this.DepthToolStripLabel.Text = "Depth:";
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            //  
            // StartToolStripSplitButton
            // 
            this.StartToolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MorphologicalToolStripMenuItem,
            this.SyntacticToolStripMenuItem});
            this.StartToolStripSplitButton.Image = global::PMA.WinForms.Properties.Resources.start;
            this.StartToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StartToolStripSplitButton.Name = "StartToolStripSplitButton";
            this.StartToolStripSplitButton.Size = new System.Drawing.Size(117, 22);
            this.StartToolStripSplitButton.Text = "Morphological";
            this.StartToolStripSplitButton.ToolTipText = "Morphological Analysis";
            this.StartToolStripSplitButton.ButtonClick += new System.EventHandler(this.StartToolStripSplitButton_ButtonClick);
            // 
            // MorphologicalToolStripMenuItem
            // 
            this.MorphologicalToolStripMenuItem.Image = global::PMA.WinForms.Properties.Resources.start;
            this.MorphologicalToolStripMenuItem.Name = "MorphologicalToolStripMenuItem";
            this.MorphologicalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.MorphologicalToolStripMenuItem.Text = "Morphological";
            this.MorphologicalToolStripMenuItem.Click += new System.EventHandler(this.MorphologicalToolStripMenuItem_Click);
            // 
            // SyntacticToolStripMenuItem
            // 
            this.SyntacticToolStripMenuItem.Enabled = false;
            this.SyntacticToolStripMenuItem.Image = global::PMA.WinForms.Properties.Resources.start;
            this.SyntacticToolStripMenuItem.Name = "SyntacticToolStripMenuItem";
            this.SyntacticToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SyntacticToolStripMenuItem.Text = "Syntactic";
            // 
            // TempToolStripTextBox
            // 
            this.TempToolStripTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.TempToolStripTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.TempToolStripTextBox.ForeColor = System.Drawing.SystemColors.GrayText;
            this.TempToolStripTextBox.Name = "TempToolStripTextBox";
            this.TempToolStripTextBox.Size = new System.Drawing.Size(500, 25);
            this.TempToolStripTextBox.Text = "enter pali-word";
            this.TempToolStripTextBox.ToolTipText = "Enter pali-word to analyze";
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AnimToolStripStatusLabel,
            this.toolStripStatusLabel1,
            this.StateToolStripStatusLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 392);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(971, 22);
            this.StatusStrip.TabIndex = 4;
            // 
            // AnimToolStripStatusLabel
            // 
            this.AnimToolStripStatusLabel.AutoSize = false;
            this.AnimToolStripStatusLabel.Enabled = false;
            this.AnimToolStripStatusLabel.Image = global::PMA.WinForms.Properties.Resources.loading;
            this.AnimToolStripStatusLabel.Name = "AnimToolStripStatusLabel";
            this.AnimToolStripStatusLabel.Size = new System.Drawing.Size(25, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // StateToolStripStatusLabel
            // 
            this.StateToolStripStatusLabel.Name = "StateToolStripStatusLabel";
            this.StateToolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.StateToolStripStatusLabel.Text = "Ready";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(971, 414);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = new System.Drawing.Size(400, 150);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pali Morphological Analyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportCatalogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportEntriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WindowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowMorphPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowMorphSolutionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripSplitButton StartToolStripSplitButton;
        private System.Windows.Forms.ToolStripMenuItem SyntacticToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox TempToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem MorphologicalToolStripMenuItem;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel AnimToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel StateToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem ShowMorphRuleInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox LayerToolStripComboBox;
        private System.Windows.Forms.ToolStripLabel LayerToolStripLabel;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        private System.Windows.Forms.ToolStripButton AutoSymbolReplaceStripButton;
        private System.Windows.Forms.ToolStripLabel DepthToolStripLabel;

        #endregion
    }
}