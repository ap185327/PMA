// <copyright file="MainForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using PMA.WinForms.Controls;
using PMA.WinForms.Extensions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PMA.WinForms.Forms
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The main view model.
        /// </summary>
        private readonly IMainViewModel _mainViewModel;

        /// <summary>
        /// The translate service.
        /// </summary>
        private readonly ITranslateService _translateService;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The morphological solution form.
        /// </summary>
        private MorphSolutionForm _morphSolutionForm;

        /// <summary>
        /// The morphological property form.
        /// </summary>
        private MorphPropertyForm _morphPropertyForm;

        /// <summary>
        /// The morphological rule information form.
        /// </summary>
        private MorphRuleInfoForm _morphRuleInfoForm;

        /// <summary>
        /// The enter tool strip text box.
        /// </summary>
        private ToolStripSpringTextBox _enterToolStripTextBox;

        /// <summary>
        /// The depth tool strip number control.
        /// </summary>
        private ToolStripNumberControl _depthToolStripNumberControl;

        /// <summary>
        /// Whether morphological analysis has been started or not.
        /// </summary>
        private bool _isStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            _mainViewModel = Program.Scope.Resolve<IMainViewModel>();
            _translateService = Program.Scope.Resolve<ITranslateService>();
            _settingService = Program.Scope.Resolve<ISettingService>();

            InitializeComponent();
            OverrideStrings();
            CreateChildForms();
            SetDefaultValues();
            SetSettings();
            SubscribeEvents();
        }

        #region Initialization methods

        /// <summary>
        /// Overrides control strings.
        /// </summary>
        private void OverrideStrings()
        {
            Text = _translateService.Translate(Name, Program.Version);
            FileToolStripMenuItem.Text = _translateService.Translate($"{Name}.{FileToolStripMenuItem.Name}");
            ExitToolStripMenuItem.Text = _translateService.Translate($"{Name}.{ExitToolStripMenuItem.Name}");
            ToolsToolStripMenuItem.Text = _translateService.Translate($"{Name}.{ToolsToolStripMenuItem.Name}");
            ImportEntriesToolStripMenuItem.Text = _translateService.Translate($"{Name}.{ImportEntriesToolStripMenuItem.Name}");
            ImportCatalogsToolStripMenuItem.Text = _translateService.Translate($"{Name}.{ImportCatalogsToolStripMenuItem.Name}");
            OptionsToolStripMenuItem.Text = _translateService.Translate($"{Name}.{OptionsToolStripMenuItem.Name}");
            WindowsToolStripMenuItem.Text = _translateService.Translate($"{Name}.{WindowsToolStripMenuItem.Name}");
            ShowMorphSolutionsToolStripMenuItem.Text = _translateService.Translate($"{Name}.{ShowMorphSolutionsToolStripMenuItem.Name}");
            ShowMorphPropertiesToolStripMenuItem.Text = _translateService.Translate($"{Name}.{ShowMorphPropertiesToolStripMenuItem.Name}");
            ShowMorphRuleInfoToolStripMenuItem.Text = _translateService.Translate($"{Name}.{ShowMorphRuleInfoToolStripMenuItem.Name}");
            HelpToolStripMenuItem.Text = _translateService.Translate($"{Name}.{HelpToolStripMenuItem.Name}");
            AboutToolStripMenuItem.Text = _translateService.Translate($"{Name}.{AboutToolStripMenuItem.Name}");
            StartToolStripSplitButton.Text = _translateService.Translate($"{Name}.{StartToolStripSplitButton.Name}");
            MorphologicalToolStripMenuItem.Text = _translateService.Translate($"{Name}.{MorphologicalToolStripMenuItem.Name}");
            SyntacticToolStripMenuItem.Text = _translateService.Translate($"{Name}.{SyntacticToolStripMenuItem.Name}");
            LayerToolStripLabel.Text = _translateService.Translate($"{Name}.{LayerToolStripLabel.Name}");
            DepthToolStripLabel.Text = _translateService.Translate($"{Name}.{DepthToolStripLabel.Name}");
            AutoSymbolReplaceStripButton.ToolTipText = _translateService.Translate($"{Name}.{AutoSymbolReplaceStripButton.Name}.ToolTipText");
        }

        /// <summary>
        /// Creates child forms.
        /// </summary>
        private void CreateChildForms()
        {
            _morphSolutionForm = new MorphSolutionForm
            {
                MdiParent = this
            };

            _morphPropertyForm = new MorphPropertyForm
            {
                MdiParent = this
            };

            _morphRuleInfoForm = new MorphRuleInfoForm
            {
                MdiParent = this
            };
        }

        /// <summary>
        /// Sets default values for control and fields.
        /// </summary>
        private void SetDefaultValues()
        {
            StateToolStripStatusLabel.Text = _mainViewModel.StatusLabel;

            LayerToolStripComboBox.Items.Add(_translateService.Translate($"{Name}.Layer.Auto"));
            LayerToolStripComboBox.Items.Add(_translateService.Translate($"{Name}.Layer.1"));
            LayerToolStripComboBox.Items.Add(_translateService.Translate($"{Name}.Layer.2"));
            LayerToolStripComboBox.Items.Add(_translateService.Translate($"{Name}.Layer.3"));
            LayerToolStripComboBox.Items.Add(_translateService.Translate($"{Name}.Layer.4"));
            LayerToolStripComboBox.Items.Add(_translateService.Translate($"{Name}.Layer.5"));
            LayerToolStripComboBox.Items.Add(_translateService.Translate($"{Name}.Layer.6"));
            LayerToolStripComboBox.Items.Add(_translateService.Translate($"{Name}.Layer.7"));
            LayerToolStripComboBox.Items.Add(_translateService.Translate($"{Name}.Layer.8"));

            _depthToolStripNumberControl = new ToolStripNumberControl()
            {
                Name = "DepthToolStripNumberControl",
            };
            _depthToolStripNumberControl.Control.Minimum = 1;
            ToolStrip.Items.Insert(5, _depthToolStripNumberControl);

            StartToolStripSplitButton.Enabled = false;

            _enterToolStripTextBox = new ToolStripSpringTextBox(_mainViewModel.InputWatermark, _mainViewModel.AutoSymbolReplace)
            {
                Name = "EnterToolStripTextBox",
                CharacterCasing = CharacterCasing.Lower
            };
            ToolStrip.Items.Remove(TempToolStripTextBox);
            TempToolStripTextBox.Dispose();
            ToolStrip.Items.Add(_enterToolStripTextBox);
        }

        /// <summary>
        /// Sets setting values for controls and fields.
        /// </summary>
        private void SetSettings()
        {
            _mainViewModel.OnAppearing();

            Height = _settingService.GetValue<int>($"{Name}.Height");
            Width = _settingService.GetValue<int>($"{Name}.Width");

            ShowMorphSolutionsToolStripMenuItem.Checked = _settingService.GetValue<bool>($"{Name}.{ShowMorphSolutionsToolStripMenuItem.Name}.Checked");
            ShowMorphPropertiesToolStripMenuItem.Checked = _settingService.GetValue<bool>($"{Name}.{ShowMorphPropertiesToolStripMenuItem.Name}.Checked");
            ShowMorphRuleInfoToolStripMenuItem.Checked = _settingService.GetValue<bool>($"{Name}.{ShowMorphRuleInfoToolStripMenuItem.Name}.Checked");

            // MorphSolutionForm
            _morphSolutionForm.Height = _settingService.GetValue<int>($"{_morphSolutionForm.Name}.Height");
            _morphSolutionForm.Width = _settingService.GetValue<int>($"{_morphSolutionForm.Name}.Width");
            _morphSolutionForm.Left = _settingService.GetValue<int>($"{_morphSolutionForm.Name}.Left");
            _morphSolutionForm.Top = _settingService.GetValue<int>($"{_morphSolutionForm.Name}.Top");

            // MorphPropertyForm
            _morphPropertyForm.Height = _settingService.GetValue<int>($"{_morphPropertyForm.Name}.Height");
            _morphPropertyForm.Width = _settingService.GetValue<int>($"{_morphPropertyForm.Name}.Width");
            _morphPropertyForm.Left = _settingService.GetValue<int>($"{_morphPropertyForm.Name}.Left");
            _morphPropertyForm.Top = _settingService.GetValue<int>($"{_morphPropertyForm.Name}.Top");

            // MorphRuleInfoForm
            _morphRuleInfoForm.Height = _settingService.GetValue<int>($"{_morphRuleInfoForm.Name}.Height");
            _morphRuleInfoForm.Width = _settingService.GetValue<int>($"{_morphRuleInfoForm.Name}.Width");
            _morphRuleInfoForm.Left = _settingService.GetValue<int>($"{_morphRuleInfoForm.Name}.Left");
            _morphRuleInfoForm.Top = _settingService.GetValue<int>($"{_morphRuleInfoForm.Name}.Top");

            LayerToolStripComboBox.SelectedIndex = _mainViewModel.Layer;

            AutoSymbolReplaceStripButton.Text = _mainViewModel.AutoSymbolReplace ? "ā" : "aa";

            _depthToolStripNumberControl.Control.Value = _mainViewModel.MaxDepthLevel;
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _enterToolStripTextBox.TextChanged += EnterToolStripTextBox_TextChanged;
            _depthToolStripNumberControl.ValueChanged += DepthToolStripNumberControl_ValueChanged;
            _morphSolutionForm.FormClosing += MorphSolutionForm_FormClosing;
            _morphPropertyForm.FormClosing += MorphPropertyForm_FormClosing;
            _morphRuleInfoForm.FormClosing += MorphRuleInfoForm_FormClosing;
            _mainViewModel.PropertyChanged += MainViewModel_PropertyChanged;
        }

        #endregion Initialization methods

        /// <summary>
        /// Event handler for the view model property changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.InvokeIfNeeded(() =>
            {
                switch (e.PropertyName)
                {
                    case "AutoSymbolReplace":
                        {
                            _enterToolStripTextBox.AutoSymbolReplace = _mainViewModel.AutoSymbolReplace;
                            break;
                        }
                    case "InputWatermark":
                        {
                            _enterToolStripTextBox.Watermark = _mainViewModel.InputWatermark;
                            break;
                        }
                    case "IsBusy":
                        {
                            if (_mainViewModel.IsBusy)
                            {
                                if (_isStarted)
                                {
                                    MorphologicalToolStripMenuItem.Image = Properties.Resources.stop;
                                    StartToolStripSplitButton.Image = Properties.Resources.stop;

                                    // to avoid incorrect display of the element _enterToolStripTextBox
                                    // after changing the image of the element StartToolStripSplitButton.
                                    ToolStrip.Width++;
                                    ToolStrip.Width--;
                                }
                                else
                                {
                                    StartToolStripSplitButton.Enabled = false;
                                }
                                
                                AnimToolStripStatusLabel.Enabled = true;
                                LayerToolStripComboBox.Enabled = false;
                                _depthToolStripNumberControl.Enabled = false;
                            }
                            else
                            {

                                if (_isStarted)
                                {
                                    _isStarted = false;

                                    MorphologicalToolStripMenuItem.Image = Properties.Resources.start;
                                    StartToolStripSplitButton.Image = Properties.Resources.start;

                                    // to avoid incorrect display of the element _enterToolStripTextBox
                                    // after changing the image of the element StartToolStripSplitButton.
                                    ToolStrip.Width++;
                                    ToolStrip.Width--;
                                }

                                AnimToolStripStatusLabel.Enabled = false;
                                LayerToolStripComboBox.Enabled = true;
                                _depthToolStripNumberControl.Enabled = true;

                                StartToolStripSplitButton.Enabled = !_enterToolStripTextBox.IsWatermarkShown;
                            }

                            break;
                        }
                    case "StatusLabel":
                        {
                            StateToolStripStatusLabel.Text = _mainViewModel.StatusLabel;
                            break;
                        }
                }
            });
        }

        /// <summary>
        /// Event handler for the PMAForm closing.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _mainViewModel.OnDisappearing();

            SaveSettings();

            Environment.Exit(0);
        }

        /// <summary>
        /// Event handler for the LayerToolStripComboBox selected index changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void LayerToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _mainViewModel.Layer = LayerToolStripComboBox.SelectedIndex;
        }

        /// <summary>
        /// Event handler for the DepthToolStripNumberControl value changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DepthToolStripNumberControl_ValueChanged(object sender, EventArgs e)
        {
            _mainViewModel.MaxDepthLevel = Convert.ToInt32(_depthToolStripNumberControl.Control.Value);
        }

        /// <summary>
        /// Event handler for the AutoSymbolReplaceStripButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void AutoSymbolReplaceStripButton_Click(object sender, EventArgs e)
        {
            _mainViewModel.AutoSymbolReplace = !_mainViewModel.AutoSymbolReplace;

            AutoSymbolReplaceStripButton.Text = _mainViewModel.AutoSymbolReplace ? "ā" : "aa";
        }

        /// <summary>
        /// Event handler for the EnterToolStripTextBox text changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void EnterToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            StartToolStripSplitButton.Enabled = !string.IsNullOrEmpty(_enterToolStripTextBox.Text) && !_enterToolStripTextBox.IsWatermarkShown;

            _mainViewModel.InputText = _enterToolStripTextBox.Text;
        }

        /// <summary>
        /// Event handler for the StartToolStripSplitButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void StartToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            if (_mainViewModel.IsBusy)
            {
                StartToolStripSplitButton.Enabled = false;
                _mainViewModel.StopCommand.Execute(null);
            }
            else
            {
                _isStarted = true;
                _mainViewModel.StartCommand.Execute(null);
            }
        }

        /// <summary>
        /// Event handler for the MorphologicalToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphologicalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_mainViewModel.IsBusy)
            {
                StartToolStripSplitButton.Enabled = false;
                _mainViewModel.StopCommand.Execute(null);
            }
            else
            {
                _isStarted = true;
                _mainViewModel.StartCommand.Execute(null);
            }
        }

        /// <summary>
        /// Event handler for the ShowMorphSolutionsToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ShowMorphSolutionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMorphSolutionsToolStripMenuItem.Checked = !ShowMorphSolutionsToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Event handler for the ShowMorphPropertiesToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ShowMorphPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMorphPropertiesToolStripMenuItem.Checked = !ShowMorphPropertiesToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Event handler for the ShowMorphRuleInfoToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ShowMorphRuleInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMorphRuleInfoToolStripMenuItem.Checked = !ShowMorphRuleInfoToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Event handler for the ImportCatalogToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using UpdateDbForm form = new UpdateDbForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Event handler for the ImportEntryToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImportEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var form = new ImportEntryForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Event handler for the OptionsToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var form = new OptionForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Event handler for the AboutToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var form = new AboutForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Event handler for the ExitToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Child forms

        /// <summary>
        /// Shows the MorphSolutionForm.
        /// </summary>
        private void ShowMorphSolutionForm()
        {
            _morphSolutionForm.Show();
            ShowMorphSolutionsToolStripMenuItem.Checked = true;
        }

        /// <summary>
        /// Hides the MorphSolutionForm.
        /// </summary>
        private void HideMorphSolutionForm()
        {
            _morphSolutionForm.Hide();
            ShowMorphSolutionsToolStripMenuItem.Checked = false;
        }

        /// <summary>
        /// Shows the MorphPropertyForm.
        /// </summary>
        private void ShowMorphPropertyForm()
        {
            _morphPropertyForm.Show();
            ShowMorphPropertiesToolStripMenuItem.Checked = true;
        }

        /// <summary>
        /// Hides the MorphPropertyForm.
        /// </summary>
        private void HideMorphPropertyForm()
        {
            _morphPropertyForm.Hide();
            ShowMorphPropertiesToolStripMenuItem.Checked = false;
        }

        /// <summary>
        /// Shows the MorphRuleInfoForm.
        /// </summary>
        private void ShowMorphRuleInfoForm()
        {
            _morphRuleInfoForm.Show();
            ShowMorphRuleInfoToolStripMenuItem.Checked = true;
        }

        /// <summary>
        /// Hides the MorphRuleInfoForm.
        /// </summary>
        private void HideMorphRuleInfoForm()
        {
            _morphRuleInfoForm.Hide();
            ShowMorphRuleInfoToolStripMenuItem.Checked = false;
        }

        /// <summary>
        /// Event handler for the ShowMorphSolutionsToolStripMenuItem checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ShowMorphSolutionsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowMorphSolutionsToolStripMenuItem.Checked)
            {
                ShowMorphSolutionForm();
            }
            else
            {
                HideMorphSolutionForm();
            }
        }

        /// <summary>
        /// Event handler for the MorphSolutionForm closing.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphSolutionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                HideMorphSolutionForm();
            }
        }

        /// <summary>
        /// Event handler for the ShowMorphPropertiesToolStripMenuItem checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ShowMorphPropertiesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowMorphPropertiesToolStripMenuItem.Checked)
            {
                ShowMorphPropertyForm();
            }
            else
            {
                HideMorphPropertyForm();
            }
        }

        /// <summary>
        /// Event handler for the MorphPropertyForm closing.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphPropertyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                HideMorphPropertyForm();
            }
        }

        /// <summary>
        /// Event handler for the ShowMorphRuleInfoToolStripMenuItem checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ShowMorphRuleInfoToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowMorphRuleInfoToolStripMenuItem.Checked)
            {
                ShowMorphRuleInfoForm();
            }
            else
            {
                HideMorphRuleInfoForm();
            }
        }

        /// <summary>
        /// Event handler for the MorphRuleInfoForm closing.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphRuleInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                HideMorphRuleInfoForm();
            }
        }

        #endregion Child forms

        /// <summary>
        /// Saves control and field settings.
        /// </summary>
        private void SaveSettings()
        {
            _settingService.SetValue($"{Name}.Height", Height);
            _settingService.SetValue($"{Name}.Width", Width);
            _settingService.SetValue($"{Name}.{ShowMorphSolutionsToolStripMenuItem.Name}.Checked", ShowMorphSolutionsToolStripMenuItem.Checked);
            _settingService.SetValue($"{Name}.{ShowMorphPropertiesToolStripMenuItem.Name}.Checked", ShowMorphPropertiesToolStripMenuItem.Checked);
            _settingService.SetValue($"{Name}.{ShowMorphRuleInfoToolStripMenuItem.Name}.Checked", ShowMorphRuleInfoToolStripMenuItem.Checked);

            // MorphSolutionForm
            _settingService.SetValue($"{_morphSolutionForm.Name}.Height", _morphSolutionForm.Height);
            _settingService.SetValue($"{_morphSolutionForm.Name}.Width", _morphSolutionForm.Width);
            _settingService.SetValue($"{_morphSolutionForm.Name}.Left", _morphSolutionForm.Left);
            _settingService.SetValue($"{_morphSolutionForm.Name}.Top", _morphSolutionForm.Top);

            // MorphPropertyForm
            _settingService.SetValue($"{_morphPropertyForm.Name}.Height", _morphPropertyForm.Height);
            _settingService.SetValue($"{_morphPropertyForm.Name}.Width", _morphPropertyForm.Width);
            _settingService.SetValue($"{_morphPropertyForm.Name}.Left", _morphPropertyForm.Left);
            _settingService.SetValue($"{_morphPropertyForm.Name}.Top", _morphPropertyForm.Top);

            // MorphRuleInfoForm
            _settingService.SetValue($"{_morphRuleInfoForm.Name}.Height", _morphRuleInfoForm.Height);
            _settingService.SetValue($"{_morphRuleInfoForm.Name}.Width", _morphRuleInfoForm.Width);
            _settingService.SetValue($"{_morphRuleInfoForm.Name}.Left", _morphRuleInfoForm.Left);
            _settingService.SetValue($"{_morphRuleInfoForm.Name}.Top", _morphRuleInfoForm.Top);

            _settingService.Commit();
        }
    }
}
