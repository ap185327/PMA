// <copyright file="ImportEntryForm.cs" company="Andrey Pospelov">
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
    public partial class ImportEntryForm : Form
    {
        /// <summary>
        /// The import morphological entry view model.
        /// </summary>
        private readonly IImportMorphEntryViewModel _importMorphEntryViewModel;

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
        /// Initializes a new instance of the <see cref="ImportEntryForm"/> class.
        /// </summary>
        public ImportEntryForm()
        {
            _importMorphEntryViewModel = Program.Scope.Resolve<IImportMorphEntryViewModel>();
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
            Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.Title");
            AnalyzeBeforeImportCheckBox.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.AnalyzeBeforeImportCheckBox");
            OpenFileLabel.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.OpenFileLabel");
            LogLabel.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.LogLabel");
            TimeColumnHeader.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.TimeColumnHeader");
            EventColumnHeader.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.EventColumnHeader");
            DescriptionColumnHeader.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.DescriptionColumnHeader");
            StartButton.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.StartButton");
            CloseButton.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.CloseButton");
            ResetButton.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.ResetButton");
            OpenFileButton.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.OpenFileButton");
            ClearLogButton.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.ClearLogButton");
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
        private void ImportMorphEntryViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.InvokeIfNeeded(() =>
            {
                switch (e.PropertyName)
                {
                    case "IsAnalyzeBeforeImportChecked":
                        {
                            AnalyzeBeforeImportCheckBox.Enabled = _importMorphEntryViewModel.IsAnalyzeBeforeImportChecked;
                            break;
                        }
                    case "AnalyzeProgressBarValue":
                        {
                            AnalyzeProgressBar.Value = _importMorphEntryViewModel.AnalyzeProgressBarValue;
                            break;
                        }
                    case "DataFilePath":
                        {
                            string path = _importMorphEntryViewModel.DataFilePath;

                            if (!_openFileComboBox.Items.Contains(path))
                            {
                                _openFileComboBox.Items.Insert(0, path);
                            }

                            _openFileComboBox.Text = path;

                            break;
                        }
                    case "IsBusy":
                        {
                            AnalyzeBeforeImportCheckBox.Enabled = !_importMorphEntryViewModel.IsBusy;
                            _openFileComboBox.Enabled = !_importMorphEntryViewModel.IsBusy;
                            ResetButton.Enabled = !_importMorphEntryViewModel.IsBusy;
                            CloseButton.Enabled = !_importMorphEntryViewModel.IsBusy;
                            OpenFileButton.Enabled = !_importMorphEntryViewModel.IsBusy;
                            ClearLogButton.Enabled = !_importMorphEntryViewModel.IsBusy;

                            if (!_importMorphEntryViewModel.IsBusy)
                            {
                                StartButton.Enabled = true;
                                StartButton.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.StartButton");
                                _importMorphEntryViewModel.DataFilePath = _openFileComboBox.Text;
                            }
                            else
                            {
                                StartButton.Text = Properties.Resources.ResourceManager.GetString("ImportEntryForm.StopButton");
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
            _logListView.AddItem(e.Message.Level, Properties.Resources.ResourceManager.GetString($"MessageLevel.{e.Message.Level}"), e.Message.Text);
        }

        /// <summary>
        /// Event handler for the StartButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (StartButton.Text == Properties.Resources.ResourceManager.GetString("ImportEntryForm.StartButton"))
            {
                _importMorphEntryViewModel.IsAnalyzeBeforeImportChecked = AnalyzeBeforeImportCheckBox.Checked;
                _importMorphEntryViewModel.DataFilePath = _openFileComboBox.Text;

                _importMorphEntryViewModel.StartCommand.Execute(null);
            }
            else
            {
                StartButton.Enabled = false;
                _importMorphEntryViewModel.StopCommand.Execute(null);
            }
        }

        /// <summary>
        /// Event handler for the ImportEntryForm closing.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportEntryForm_FormClosing(object sender, FormClosingEventArgs e)
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
            _openFileComboBox.OpenFile(Properties.Resources.ResourceManager.GetString("ImportEntryForm.OpenFileTitle"), Properties.Resources.ResourceManager.GetString("ImportEntryForm.OpenFileFilter"));
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
            _importMorphEntryViewModel.ResetCommand.Execute(null);
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
        /// Event handler for the ImportEntryForm visible changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportEntryForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                _importMorphEntryViewModel.IsActive = true;
                SetSettings();
                SubscribeEvents();
            }
            else
            {
                SaveSettings();
                UnsubscribeEvents();
                _importMorphEntryViewModel.IsActive = false;
            }
        }

        /// <summary>
        /// Sets setting values for controls and fields.
        /// </summary>
        private void SetSettings()
        {
            Height = _settingService.GetValue<int>("WinForms.ImportEntryForm.Height");

            _openFileComboBox.Items.Add(_importMorphEntryViewModel.DataFilePath);
            _openFileComboBox.Text = _importMorphEntryViewModel.DataFilePath;

            AnalyzeBeforeImportCheckBox.Checked = _importMorphEntryViewModel.IsAnalyzeBeforeImportChecked;
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _importMorphEntryViewModel.PropertyChanged += ImportMorphEntryViewModel_PropertyChanged;
            _logMessageService.MessageReceived += LogMessageService_MessageReceived;
        }

        /// <summary>
        /// Saves control and field settings.
        /// </summary>
        private void SaveSettings()
        {
            _settingService.SetValue("WinForms.ImportEntryForm.Height", Height);
        }

        /// <summary>
        /// Unsubscribes events.
        /// </summary>
        private void UnsubscribeEvents()
        {
            _importMorphEntryViewModel.PropertyChanged -= ImportMorphEntryViewModel_PropertyChanged;
            _logMessageService.MessageReceived -= LogMessageService_MessageReceived;
        }
    }
}
