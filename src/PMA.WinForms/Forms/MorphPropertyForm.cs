// <copyright file="MorphPropertyForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.WinForms.Controls;
using PMA.WinForms.EventArguments;
using PMA.WinForms.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PMA.WinForms.Forms
{
    public partial class MorphPropertyForm : Form
    {
        /// <summary>
        /// The morphological solution view model.
        /// </summary>
        private readonly IMorphPropertyViewModel _morphPropertyViewModel;

        /// <summary>
        /// The morphological parameter dynamic property grid.
        /// </summary>
        private DynamicPropertyGrid _propertyGridParameters;

        /// <summary>
        /// The entry text box.
        /// </summary>
        private WatermarkTextBox _entryTextBox;

        /// <summary>
        /// The left text box.
        /// </summary>
        private WatermarkTextBox _leftTextBox;

        /// <summary>
        /// The right text box.
        /// </summary>
        private WatermarkTextBox _rightTextBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="MorphPropertyForm">MorphPropertyForm</see> class.
        /// </summary>
        public MorphPropertyForm()
        {
            _morphPropertyViewModel = Program.Scope.Resolve<IMorphPropertyViewModel>();
            _morphPropertyViewModel.IsActive = true;

            InitializeComponent();
            OverrideStrings();
            SetDefaultValues();
            SubscribeEvents();
        }

        #region Initialization methods

        /// <summary>
        /// Overrides control strings.
        /// </summary>
        private void OverrideStrings()
        {
            Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.Title");
            BaseLabel.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.BaseLabel");
            LeftCheckBox.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.LeftCheckBox");
            RightCheckBox.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.RightCheckBox");
            IsVirtualCheckBox.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.IsVirtualCheckBox");
            GetEntryIdButton.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.GetEntryIdButton");
            ResetButton.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.ResetButton");
            GetLeftIdButton.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.GetLeftIdButton");
            GetRightIdButton.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.GetRightIdButton");
            StartButton.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.StartButton");
            SaveButton.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.SaveButton");
            DeleteButton.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.DeleteButton");
        }

        /// <summary>
        /// Sets default values for control and fields.
        /// </summary>
        private void SetDefaultValues()
        {
            // PropertyGridParameters
            _propertyGridParameters = new DynamicPropertyGrid
            {
                Anchor = TempPropertyGridParameters.Anchor,
                Location = TempPropertyGridParameters.Location,
                Name = "PropertyGridParameters",
                Size = TempPropertyGridParameters.Size,
                TabIndex = TempPropertyGridParameters.TabIndex
            };
            Controls.Remove(TempPropertyGridParameters);
            TempPropertyGridParameters.Dispose();
            Controls.Add(_propertyGridParameters);

            foreach (var controlViewModel in _morphPropertyViewModel.Properties)
            {
                _propertyGridParameters.AddProperty(controlViewModel.Name, controlViewModel.Category, controlViewModel.Description, controlViewModel.TermEntries.ToArray(), controlViewModel.SelectedTerm);
            }

            // EntryTextBox
            _entryTextBox = new WatermarkTextBox(_morphPropertyViewModel.InputWatermark)
            {
                Anchor = TempEntryTextBox.Anchor,
                CharacterCasing = TempEntryTextBox.CharacterCasing,
                Location = TempEntryTextBox.Location,
                Name = "EntryTextBox",
                Size = TempEntryTextBox.Size,
                TabIndex = TempEntryTextBox.TabIndex
            };
            Controls.Remove(TempEntryTextBox);
            TempEntryTextBox.Dispose();
            Controls.Add(_entryTextBox);

            // RightTextBox
            _rightTextBox = new WatermarkTextBox(_morphPropertyViewModel.InputWatermark)
            {
                Anchor = TempRightTextBox.Anchor,
                CharacterCasing = TempRightTextBox.CharacterCasing,
                Location = TempRightTextBox.Location,
                Name = "RightTextBox",
                Size = TempRightTextBox.Size,
                TabIndex = TempRightTextBox.TabIndex,
                Enabled = false
            };
            Controls.Remove(TempRightTextBox);
            TempRightTextBox.Dispose();
            Controls.Add(_rightTextBox);

            // LeftTextBox
            _leftTextBox = new WatermarkTextBox(_morphPropertyViewModel.InputWatermark)
            {
                Anchor = TempLeftTextBox.Anchor,
                CharacterCasing = TempLeftTextBox.CharacterCasing,
                Location = TempLeftTextBox.Location,
                Name = "LeftTextBox",
                Size = TempLeftTextBox.Size,
                TabIndex = TempLeftTextBox.TabIndex,
                Enabled = false
            };
            Controls.Remove(TempLeftTextBox);
            TempLeftTextBox.Dispose();
            Controls.Add(_leftTextBox);

            BaseComboBox.Items.Add(Properties.Resources.ResourceManager.GetString("MorphBase.Unknown")!);
            BaseComboBox.Items.Add(Properties.Resources.ResourceManager.GetString("MorphBase.None")!);
            BaseComboBox.Items.Add(Properties.Resources.ResourceManager.GetString("MorphBase.Left")!);
            BaseComboBox.Items.Add(Properties.Resources.ResourceManager.GetString("MorphBase.Right")!);
            BaseComboBox.Items.Add(Properties.Resources.ResourceManager.GetString("MorphBase.Both")!);

            ActiveControl = BaseComboBox;

            BaseComboBox.SelectedIndex = (int)_morphPropertyViewModel.Base;
            EntryIdLabel.Text = string.Format(Properties.Resources.ResourceManager.GetString("MorphPropertyForm.EntryIdLabel")!, _morphPropertyViewModel.EntryId);
            GetEntryIdButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.Entry);
            LockIdCheckBox.Checked = _morphPropertyViewModel.IsLockEntryIdChecked;

            UpdateIsVirtualCheckBox();
            UpdateButtons();
            UpdateLeftControls();
            UpdateRightControls();
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _entryTextBox.TextChanged += TextBox_TextChanged;
            _leftTextBox.TextChanged += TextBox_TextChanged;
            _rightTextBox.TextChanged += TextBox_TextChanged;
            _propertyGridParameters.DynamicPropertyValueChanged += PropertyGridParameters_DynamicPropertyValueChanged;
            _morphPropertyViewModel.PropertyChanged += MorphPropertyViewModel_PropertyChanged;

            foreach (var controlViewModel in _morphPropertyViewModel.Properties)
            {
                controlViewModel.PropertyChanged += MorphPropertyControlViewModel_PropertyChanged;
            }
        }

        #endregion Initialization methods

        /// <summary>
        /// Event handler for the view model property control changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphPropertyControlViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var controlViewModel = (IMorphPropertyControlViewModel)sender;

            switch (e.PropertyName)
            {
                case nameof(controlViewModel.SelectedTerm):
                    _propertyGridParameters.UpdatePropertyValue(controlViewModel.Index, controlViewModel.TermEntries.IndexOf(controlViewModel.SelectedTerm));
                    break;
                case nameof(controlViewModel.TermEntries):
                    _propertyGridParameters.UpdatePropertyValues(controlViewModel.Index, controlViewModel.TermEntries.ToArray());
                    break;
            }
        }

        /// <summary>
        /// Event handler for the view model property changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphPropertyViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.InvokeIfNeeded(() =>
            {
                switch (e.PropertyName)
                {
                    case nameof(IMorphPropertyViewModel.Entry):
                        UpdateButtons();
                        GetEntryIdButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.Entry);
                        _entryTextBox.Text = _morphPropertyViewModel.Entry;
                        break;
                    case nameof(IMorphPropertyViewModel.LeftEntry):
                    case nameof(IMorphPropertyViewModel.LeftId):
                    case nameof(IMorphPropertyViewModel.IsLeftChecked):
                        UpdateLeftControls();
                        UpdateButtons();
                        break;
                    case nameof(IMorphPropertyViewModel.RightEntry):
                    case nameof(IMorphPropertyViewModel.RightId):
                    case nameof(IMorphPropertyViewModel.IsRightChecked):
                        UpdateRightControls();
                        UpdateButtons();
                        break;
                    case nameof(IMorphPropertyViewModel.EntryId):
                        UpdateButtons();
                        EntryIdLabel.Text =
                            string.Format(
                                Properties.Resources.ResourceManager.GetString("MorphPropertyForm.EntryIdLabel")!,
                                _morphPropertyViewModel.EntryId);
                        break;
                    case nameof(IMorphPropertyViewModel.Base):
                        BaseComboBox.SelectedIndex = (int)_morphPropertyViewModel.Base;
                        UpdateLeftControls();
                        UpdateRightControls();
                        break;
                    case nameof(IMorphPropertyViewModel.IsVirtual):
                        UpdateIsVirtualCheckBox();
                        UpdateButtons();
                        break;
                    case nameof(IMorphPropertyViewModel.IsBusy):
                    case nameof(IMorphPropertyViewModel.ExecuteCommandDisabled):
                        UpdateButtons();
                        break;
                    case nameof(IMorphPropertyViewModel.IsLockEntryIdChecked):
                        LockIdCheckBox.Checked = _morphPropertyViewModel.IsLockEntryIdChecked;
                        break;
                    case nameof(IMorphPropertyViewModel.GetEntryIdViewModel):
                        if (_morphPropertyViewModel.GetEntryIdViewModel is not null)
                        {
                            using var form = new GetEntryIdForm(_morphPropertyViewModel.GetEntryIdViewModel);

                            if (!form.IsDisposed)
                            {
                                form.ShowDialog();
                            }
                        }
                        break;
                }
            });
        }

        /// <summary>
        /// Updates the controls for the left part of morphological entry.
        /// </summary>
        private void UpdateLeftControls()
        {
            LeftCheckBox.Checked = _morphPropertyViewModel.IsLeftChecked;
            _leftTextBox.Text = _morphPropertyViewModel.LeftEntry;
            LeftIdLabel.Text = string.Format(Properties.Resources.ResourceManager.GetString("MorphPropertyForm.LeftIdLabel")!, _morphPropertyViewModel.LeftId);


            switch (_morphPropertyViewModel.Base)
            {
                case MorphBase.None:
                    LeftCheckBox.Enabled = false;
                    _leftTextBox.Enabled = false;
                    GetLeftIdButton.Enabled = false;
                    LeftIdLabel.ForeColor = DefaultForeColor;
                    break;
                default:
                    LeftCheckBox.Enabled = true;
                    _leftTextBox.Enabled = _morphPropertyViewModel.IsLeftChecked;
                    GetLeftIdButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.LeftEntry) && _morphPropertyViewModel.IsLeftChecked;
                    LeftIdLabel.ForeColor = _morphPropertyViewModel.IsLeftChecked && _morphPropertyViewModel.LeftId == 0 ? Color.Red : DefaultForeColor;
                    break;
            }
        }

        /// <summary>
        /// Updates the controls for the right part of morphological entry.
        /// </summary>
        private void UpdateRightControls()
        {
            RightCheckBox.Checked = _morphPropertyViewModel.IsRightChecked;
            _rightTextBox.Text = _morphPropertyViewModel.RightEntry;
            RightIdLabel.Text = string.Format(Properties.Resources.ResourceManager.GetString("MorphPropertyForm.RightIdLabel")!, _morphPropertyViewModel.RightId);

            switch (_morphPropertyViewModel.Base)
            {
                case MorphBase.None:
                    RightCheckBox.Enabled = false;
                    _rightTextBox.Enabled = false;
                    GetRightIdButton.Enabled = false;
                    RightIdLabel.ForeColor = DefaultForeColor;
                    break;
                default:
                    RightCheckBox.Enabled = true;
                    _rightTextBox.Enabled = _morphPropertyViewModel.IsRightChecked;
                    GetRightIdButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.RightEntry) && _morphPropertyViewModel.IsRightChecked;
                    RightIdLabel.ForeColor = _morphPropertyViewModel.IsRightChecked && _morphPropertyViewModel.RightId == 0 ? Color.Red : DefaultForeColor;
                    break;
            }
        }

        /// <summary>
        /// Updates the button controls.
        /// </summary>
        private void UpdateButtons()
        {
            if (_morphPropertyViewModel.IsBusy)
            {
                ResetButton.Enabled = false;
                DeleteButton.Enabled = false;
                SaveButton.Enabled = false;

                StartButton.Image = Properties.Resources.stop;
                StartButton.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.StopButton");

                StartButton.Enabled = !_morphPropertyViewModel.ExecuteCommandDisabled;
            }
            else
            {
                ResetButton.Enabled = true;
                DeleteButton.Enabled = _morphPropertyViewModel.EntryId != 0;
                SaveButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.Entry) &&
                                     LeftIdLabel.ForeColor != Color.Red &&
                                     RightIdLabel.ForeColor != Color.Red &&
                                     IsVirtualCheckBox.ForeColor != Color.Red;

                StartButton.Image = Properties.Resources.start;
                StartButton.Text = Properties.Resources.ResourceManager.GetString("MorphPropertyForm.StartButton");

                StartButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.Entry);
            }
        }

        /// <summary>
        /// Updates IsVirtual checkbox.
        /// </summary>
        private void UpdateIsVirtualCheckBox()
        {
            if (!_morphPropertyViewModel.IsVirtual.HasValue)
            {
                IsVirtualCheckBox.CheckState = CheckState.Indeterminate;
                IsVirtualCheckBox.ForeColor = Color.Red;
                return;
            }

            IsVirtualCheckBox.ForeColor = DefaultForeColor;
            IsVirtualCheckBox.CheckState = _morphPropertyViewModel.IsVirtual.Value
                ? CheckState.Checked
                : CheckState.Unchecked;
        }

        /// <summary>
        /// Event handler for the PropertyGridParameters dynamic property value changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void PropertyGridParameters_DynamicPropertyValueChanged(object sender, DynamicPropertyEventArgs e)
        {
            int newSelectedIndex = e.NewValueIndex;

            var property = _morphPropertyViewModel.Properties.Single(x => x.Index == e.PropertyIndex);

            property.SelectedTerm = property.TermEntries[newSelectedIndex];
        }

        /// <summary>
        /// Event handler for the MorphPropertyForm location changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphPropertyForm_LocationChanged(object sender, EventArgs e)
        {
            this.Sticking();
        }

        /// <summary>
        /// Event handler for the text changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = (WatermarkTextBox)sender;

            if (textBox.IsWatermarkShown) return;

            switch (textBox.Name)
            {
                case "LeftTextBox":
                    _morphPropertyViewModel.LeftEntry = textBox.Text;
                    break;
                case "RightTextBox":
                    _morphPropertyViewModel.RightEntry = textBox.Text;
                    break;
                default:
                    _morphPropertyViewModel.Entry = textBox.Text;
                    break;
            }
        }

        /// <summary>
        /// Event handler for the BaseComboBox selected index changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void BaseComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _morphPropertyViewModel.Base = (MorphBase)BaseComboBox.SelectedIndex;

            BaseLabel.BackColor = _morphPropertyViewModel.Base == MorphBase.Unknown
                ? Color.LightYellow
                : DefaultBackColor;
        }

        /// <summary>
        /// Event handler for the IsVirtualCheckBox checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void IsVirtualCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            _morphPropertyViewModel.IsVirtual = IsVirtualCheckBox.CheckState switch
            {
                CheckState.Checked => true,
                CheckState.Unchecked => false,
                _ => null
            };
        }

        /// <summary>
        /// Event handler for the LeftCheckBox checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void LeftCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _morphPropertyViewModel.IsLeftChecked = LeftCheckBox.Checked;
        }

        /// <summary>
        /// Event handler for the RightCheckBox checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void RightCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _morphPropertyViewModel.IsRightChecked = RightCheckBox.Checked;
        }

        /// <summary>
        /// Event handler for the LockIdCheckBox checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void LockIdCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _morphPropertyViewModel.IsLockEntryIdChecked = LockIdCheckBox.Checked;
        }

        /// <summary>
        /// Event handler for the ResetButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ResetButton_Click(object sender, EventArgs e)
        {
            _morphPropertyViewModel.ResetCommand.Execute(null);
            _propertyGridParameters.Refresh();
        }

        /// <summary>
        /// Event handler for the GetIdButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void GetIdButton_Click(object sender, EventArgs e)
        {
            var getIdButton = (Button)sender;

            switch (getIdButton.Name)
            {
                case "GetLeftIdButton":
                    _morphPropertyViewModel.GetLeftIdCommand.Execute(null);
                    break;
                case "GetRightIdButton":
                    _morphPropertyViewModel.GetRightIdCommand.Execute(null);
                    break;
                default:
                    _morphPropertyViewModel.GetEntryIdCommand.Execute(null);
                    break;
            }
        }

        /// <summary>
        /// Event handler for the SaveButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            _morphPropertyViewModel.SaveCommand.Execute(null);
        }

        /// <summary>
        /// Event handler for the DeleteButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            _morphPropertyViewModel.DeleteCommand.Execute(null);
        }

        /// <summary>
        /// Event handler for the StartButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (StartButton.Text == Properties.Resources.ResourceManager.GetString("MorphPropertyForm.StartButton"))
            {
                _morphPropertyViewModel.ExecuteCommand.Execute(true);
            }
            else
            {
                _morphPropertyViewModel.ExecuteCommand.Execute(false);
            }
        }
    }
}
