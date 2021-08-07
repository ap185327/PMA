// <copyright file="DbUpdateForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using PMA.WinForms.Controls;
using PMA.WinForms.Extensions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PMA.WinForms.Forms
{
    public partial class UpdateDbForm : Form
    {
        /// <summary>
        /// The database update view model.
        /// </summary>
        private readonly IUpdateDbViewModel _dbUpdateViewModel;

        /// <summary>
        /// The translate service.
        /// </summary>
        private readonly ITranslateService _translateService;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The log message service.
        /// </summary>
        private readonly ILogMessageService _logMessageService;

        /// <summary>
        /// The log view list control.
        /// </summary>
        private LogListView _logListView;

        /// <summary>
        /// The open file dialog combo box control.
        /// </summary>
        private OpenFileDialogComboBox _openFileComboBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateDbForm"/> class.
        /// </summary>
        public UpdateDbForm()
        {
            _dbUpdateViewModel = Program.Scope.Resolve<IUpdateDbViewModel>();
            _translateService = Program.Scope.Resolve<ITranslateService>();
            _settingService = Program.Scope.Resolve<ISettingService>();
            _logMessageService = Program.Scope.Resolve<ILogMessageService>();

            InitializeComponent();
            OverrideStrings();
            SetDefaultValues();
        }

        #region Initialization methods

        /// <summary>
        /// Overrides control strings.
        /// </summary>
        private void OverrideStrings()
        {
            Text = _translateService.Translate(Name);
            DataTableGroupBox.Text = _translateService.Translate($"{Name}.{DataTableGroupBox.Name}");
            ImportMorphCombinationsCheckBox.Text = _translateService.Translate($"{Name}.{ImportMorphCombinationsCheckBox.Name}");
            ImportSandhiGroupsCheckBox.Text = _translateService.Translate($"{Name}.{ImportSandhiGroupsCheckBox.Name}");
            ImportSandhiRulesCheckBox.Text = _translateService.Translate($"{Name}.{ImportSandhiRulesCheckBox.Name}");
            ImportMorphRulesCheckBox.Text = _translateService.Translate($"{Name}.{ImportMorphRulesCheckBox.Name}");
            OpenFileLabel.Text = _translateService.Translate($"{Name}.{OpenFileLabel.Name}");
            LogLabel.Text = _translateService.Translate($"{Name}.{LogLabel.Name}");
            TimeColumnHeader.Text = _translateService.Translate($"{Name}.TimeColumnHeader");
            EventColumnHeader.Text = _translateService.Translate($"{Name}.EventColumnHeader");
            DescriptionColumnHeader.Text = _translateService.Translate($"{Name}.DescriptionColumnHeader");
            StartButton.Text = _translateService.Translate($"{Name}.{StartButton.Name}");
            CloseButton.Text = _translateService.Translate($"{Name}.{CloseButton.Name}");
            ResetButton.Text = _translateService.Translate($"{Name}.{ResetButton.Name}");
            OpenFileButton.Text = _translateService.Translate($"{Name}.{OpenFileButton.Name}");
            ClearLogButton.Text = _translateService.Translate($"{Name}.{ClearLogButton.Name}");
        }

        /// <summary>
        /// Sets default values for control and fields.
        /// </summary>
        private void SetDefaultValues()
        {
            // LogListView
            Controls.Remove(TempLogListView);
            _logListView = new LogListView
            {
                Alignment = TempLogListView.Alignment,
                Anchor = TempLogListView.Anchor,
                FullRowSelect = TempLogListView.FullRowSelect,
                HideSelection = TempLogListView.HideSelection,
                Location = TempLogListView.Location,
                Name = "LogListView",
                Size = TempLogListView.Size,
                TabIndex = TempLogListView.TabIndex,
                UseCompatibleStateImageBehavior = TempLogListView.UseCompatibleStateImageBehavior,
                View = TempLogListView.View
            };
            TempLogListView.Dispose();
            _logListView.Columns.AddRange(new[] { TimeColumnHeader, EventColumnHeader, DescriptionColumnHeader });
            Controls.Add(_logListView);

            // OpenFileDialogComboBox
            Controls.Remove(TempOpenFileComboBox);
            _openFileComboBox = new OpenFileDialogComboBox
            {
                FormattingEnabled = TempOpenFileComboBox.FormattingEnabled,
                Location = TempOpenFileComboBox.Location,
                Name = "OpenFileComboBox",
                Size = TempOpenFileComboBox.Size,
                TabIndex = TempOpenFileComboBox.TabIndex
            };
            Controls.Add(_openFileComboBox);
            TempOpenFileComboBox.Dispose();
        }

        #endregion Initialization methods

        /// <summary>
        /// Event handler for the view model property changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DbUpdateViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.InvokeIfNeeded(() =>
            {
                switch (e.PropertyName)
                {
                    case "IsSandhiGroupDbTableChecked":
                        {
                            ImportSandhiGroupsCheckBox.Checked = _dbUpdateViewModel.IsSandhiGroupDbTableChecked;
                            break;
                        }
                    case "IsSandhiRuleDbTableChecked":
                        {
                            ImportSandhiRulesCheckBox.Checked = _dbUpdateViewModel.IsSandhiRuleDbTableChecked;
                            break;
                        }
                    case "IsMorphRuleDbTableChecked":
                        {
                            ImportMorphRulesCheckBox.Checked = _dbUpdateViewModel.IsMorphRuleDbTableChecked;
                            break;
                        }
                    case "IsMorphCombinationDbTableChecked":
                        {
                            ImportMorphCombinationsCheckBox.Checked = _dbUpdateViewModel.IsMorphCombinationDbTableChecked;
                            break;
                        }
                    case "DataFilePath":
                        {
                            string path = _dbUpdateViewModel.DataFilePath;

                            if (!_openFileComboBox.Items.Contains(path))
                            {
                                _openFileComboBox.Items.Insert(0, path);
                            }

                            _openFileComboBox.Text = path;

                            break;
                        }
                    case "IsBusy":
                        {
                            ImportMorphCombinationsCheckBox.Enabled = !_dbUpdateViewModel.IsBusy;
                            ImportSandhiRulesCheckBox.Enabled = !_dbUpdateViewModel.IsBusy;
                            ImportSandhiGroupsCheckBox.Enabled = !_dbUpdateViewModel.IsBusy;
                            ImportMorphRulesCheckBox.Enabled = !_dbUpdateViewModel.IsBusy;
                            _openFileComboBox.Enabled = !_dbUpdateViewModel.IsBusy;
                            ResetButton.Enabled = !_dbUpdateViewModel.IsBusy;
                            CloseButton.Enabled = !_dbUpdateViewModel.IsBusy;
                            OpenFileButton.Enabled = !_dbUpdateViewModel.IsBusy;
                            ClearLogButton.Enabled = !_dbUpdateViewModel.IsBusy;

                            if (!_dbUpdateViewModel.IsBusy)
                            {
                                StartButton.Enabled = true;
                                StartButton.Text = _translateService.Translate($"{Name}.StartButton");
                                _dbUpdateViewModel.DataFilePath = _openFileComboBox.Text;
                            }
                            else
                            {
                                StartButton.Text = _translateService.Translate($"{Name}.StopButton");
                            }

                            break;
                        }
                }
            });
        }

        /// <summary>
        /// Event handler for the log message received.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void LogMessageService_MessageReceived(object sender, LogMessageEventArgs e)
        {
            _logListView.AddItem(e.Message.Level, _translateService.Translate(e.Message.Level), e.Message.Text);
        }

        /// <summary>
        /// Event handler for the StartButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (StartButton.Text == _translateService.Translate($"{Name}.StartButton"))
            {
                _dbUpdateViewModel.IsSandhiGroupDbTableChecked = ImportSandhiGroupsCheckBox.Checked;
                _dbUpdateViewModel.IsSandhiRuleDbTableChecked = ImportSandhiRulesCheckBox.Checked;
                _dbUpdateViewModel.IsMorphRuleDbTableChecked = ImportMorphRulesCheckBox.Checked;
                _dbUpdateViewModel.IsMorphCombinationDbTableChecked = ImportMorphCombinationsCheckBox.Checked;
                _dbUpdateViewModel.DataFilePath = _openFileComboBox.Text;

                _dbUpdateViewModel.StartCommand.Execute(null);
            }
            else
            {
                StartButton.Enabled = false;
                _dbUpdateViewModel.StopCommand.Execute(null);
            }
        }

        /// <summary>
        /// Event handler for the DbUpdateForm closing.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DbUpdateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CloseButton.Enabled)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Event handler for the OpenFileButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            _openFileComboBox.OpenFile(_translateService.Translate($"{Name}.OpenFileTitle"), _translateService.Translate($"{Name}.OpenFileFilter"));
        }

        /// <summary>
        /// Event handler for the CloseButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event handler for the ResetButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ResetButton_Click(object sender, EventArgs e)
        {
            _dbUpdateViewModel.ResetCommand.Execute(null);
        }

        /// <summary>
        /// Event handler for the ClearLogButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            _logListView.Items.Clear();
        }

        /// <summary>
        /// Event handler for the ImportMorphCombinationsCheckBox checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportMorphCombinationsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _dbUpdateViewModel.IsMorphCombinationDbTableChecked = ImportMorphCombinationsCheckBox.Checked;
            UpdateStartButton();
        }

        /// <summary>
        /// Event handler for the ImportMorphRulesCheckBox checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportMorphRulesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _dbUpdateViewModel.IsMorphRuleDbTableChecked = ImportMorphRulesCheckBox.Checked;
            UpdateStartButton();
        }

        /// <summary>
        /// Event handler for the ImportSandhiGroupsCheckBox checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportSandhiGroupsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _dbUpdateViewModel.IsSandhiGroupDbTableChecked = ImportSandhiGroupsCheckBox.Checked;
            UpdateStartButton();
        }

        /// <summary>
        /// Event handler for the ImportSandhiRulesCheckBox checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportSandhiRulesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _dbUpdateViewModel.IsSandhiRuleDbTableChecked = ImportSandhiRulesCheckBox.Checked;
            UpdateStartButton();
        }

        /// <summary>
        /// Event handler for the DbUpdateForm visible changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DbUpdateForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                _dbUpdateViewModel.OnAppearing();
                SetSettings();
                SubscribeEvents();
            }
            else
            {
                SaveSettings();
                UnsubscribeEvents();
                _dbUpdateViewModel.OnDisappearing();
            }
        }

        /// <summary>
        /// Sets setting values for controls and fields.
        /// </summary>
        private void SetSettings()
        {
            Height = _settingService.GetValue<int>($"{Name}.Height");
            TimeColumnHeader.Width = _settingService.GetValue<int>($"{Name}.TimeColumnHeader.Width");
            EventColumnHeader.Width = _settingService.GetValue<int>($"{Name}.EventColumnHeader.Width");
            DescriptionColumnHeader.Width = _settingService.GetValue<int>($"{Name}.DescriptionColumnHeader.Width");

            _openFileComboBox.Items.Add(_dbUpdateViewModel.DataFilePath);
            _openFileComboBox.Text = _dbUpdateViewModel.DataFilePath;

            ImportSandhiGroupsCheckBox.Checked = _dbUpdateViewModel.IsSandhiGroupDbTableChecked;
            ImportSandhiRulesCheckBox.Checked = _dbUpdateViewModel.IsSandhiRuleDbTableChecked;
            ImportMorphRulesCheckBox.Checked = _dbUpdateViewModel.IsMorphRuleDbTableChecked;
            ImportMorphCombinationsCheckBox.Checked = _dbUpdateViewModel.IsMorphCombinationDbTableChecked;

            UpdateStartButton();
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _dbUpdateViewModel.PropertyChanged += DbUpdateViewModel_PropertyChanged;
            _logMessageService.MessageReceived += LogMessageService_MessageReceived;
        }

        /// <summary>
        /// Saves control and field settings.
        /// </summary>
        private void SaveSettings()
        {
            _settingService.SetValue($"{Name}.Height", Height);
            _settingService.SetValue($"{Name}.TimeColumnHeader.Width", TimeColumnHeader.Width);
            _settingService.SetValue($"{Name}.EventColumnHeader.Width", EventColumnHeader.Width);
            _settingService.SetValue($"{Name}.DescriptionColumnHeader.Width", DescriptionColumnHeader.Width);
        }

        /// <summary>
        /// Unsubscribes events.
        /// </summary>
        private void UnsubscribeEvents()
        {
            _dbUpdateViewModel.PropertyChanged -= DbUpdateViewModel_PropertyChanged;
            _logMessageService.MessageReceived -= LogMessageService_MessageReceived;
        }

        /// <summary>
        /// Updates start button enabled state.
        /// </summary>
        private void UpdateStartButton()
        {
            if (ImportSandhiGroupsCheckBox.Checked ||
                ImportSandhiRulesCheckBox.Checked ||
                ImportMorphRulesCheckBox.Checked ||
                ImportMorphCombinationsCheckBox.Checked)
            {
                StartButton.Enabled = true;
            }
            else
            {
                StartButton.Enabled = false;
            }
        }
    }
}