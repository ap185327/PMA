// <copyright file="MorphPropertyForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using PMA.Domain.Enums;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.WinForms.Controls;
using PMA.WinForms.EventArguments;
using PMA.WinForms.Extensions;
using PMA.WinForms.Helpers;
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
        /// The translate service.
        /// </summary>
        private readonly ITranslateService _translateService;

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
            _translateService = Program.Scope.Resolve<ITranslateService>();

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
            Text = _translateService.Translate(Name);
            BaseLabel.Text = _translateService.Translate($"{Name}.{BaseLabel.Name}");
            LeftCheckBox.Text = _translateService.Translate($"{Name}.{LeftCheckBox.Name}");
            RightCheckBox.Text = _translateService.Translate($"{Name}.{RightCheckBox.Name}");
            IsVirtualCheckBox.Text = _translateService.Translate($"{Name}.{IsVirtualCheckBox.Name}");
            GetEntryIdButton.Text = _translateService.Translate($"{Name}.{GetEntryIdButton.Name}");
            ResetButton.Text = _translateService.Translate($"{Name}.{ResetButton.Name}");
            GetLeftIdButton.Text = _translateService.Translate($"{Name}.{GetLeftIdButton.Name}");
            GetRightIdButton.Text = _translateService.Translate($"{Name}.{GetRightIdButton.Name}");
            StartButton.Text = _translateService.Translate($"{Name}.{StartButton.Name}");
            SaveButton.Text = _translateService.Translate($"{Name}.{SaveButton.Name}");
            DeleteButton.Text = _translateService.Translate($"{Name}.{DeleteButton.Name}");
        }

        /// <summary>
        /// Sets default values for control and fields.
        /// </summary>
        private void SetDefaultValues()
        {
            // PropertyGridParameters
            _propertyGridParameters = new DynamicPropertyGrid()
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
                _propertyGridParameters.AddProperty(controlViewModel.Name, controlViewModel.Category, controlViewModel.Description, controlViewModel.TermEntries.ToArray(), controlViewModel.SelectedIndex);
            }

            // EntryTextBox
            _entryTextBox = new WatermarkTextBox(_morphPropertyViewModel.InputWatermark, _morphPropertyViewModel.AutoSymbolReplace)
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
            _rightTextBox = new WatermarkTextBox(_morphPropertyViewModel.InputWatermark, _morphPropertyViewModel.AutoSymbolReplace)
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
            _leftTextBox = new WatermarkTextBox(_morphPropertyViewModel.InputWatermark, _morphPropertyViewModel.AutoSymbolReplace)
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

            BaseComboBox.Items.Add(_translateService.Translate(MorphBase.Unknown));
            BaseComboBox.Items.Add(_translateService.Translate(MorphBase.None));
            BaseComboBox.Items.Add(_translateService.Translate(MorphBase.Left));
            BaseComboBox.Items.Add(_translateService.Translate(MorphBase.Right));
            BaseComboBox.Items.Add(_translateService.Translate(MorphBase.Both));

            ActiveControl = BaseComboBox;

            BaseComboBox.SelectedIndex = (int)_morphPropertyViewModel.Base;
            IsVirtualCheckBox.Checked = _morphPropertyViewModel.IsVirtual;
            EntryIdLabel.Text = _translateService.Translate($"{Name}.{EntryIdLabel.Name}", _morphPropertyViewModel.EntryId);
            GetEntryIdButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.Entry);
            LockIdCheckBox.Checked = _morphPropertyViewModel.IsLockEntryIdChecked;

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
            _morphPropertyViewModel.ShowModalDialog += MorphPropertyViewModel_ShowModalDialog;
            _morphPropertyViewModel.PropertyChanged += MorphPropertyViewModel_PropertyChanged;

            foreach (var controlViewModel in _morphPropertyViewModel.Properties)
            {
                controlViewModel.PropertyChanged += MorphPropertyControlViewModel_PropertyChanged;
            }
        }

        #endregion Initialization methods

        /// <summary>
        /// Event handler for the view model property dialog shown.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphPropertyViewModel_ShowModalDialog(object sender, ModalDialogEventArgs e)
        {
            var buttons = MessageBoxHelper.GetButtons(e.Buttons);
            var icon = MessageBoxHelper.GetIcon(e.Type);

            var result = MessageBox.Show(e.Message, e.Title, buttons, icon);

            int index = MessageBoxHelper.GetButtonIndex(result, buttons);

            _morphPropertyViewModel.PressModalDialogButtonCommand.Execute(index);
        }

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
                case "SelectedIndex":
                    {
                        _propertyGridParameters.UpdatePropertyValue(controlViewModel.Index, controlViewModel.SelectedIndex);
                        break;
                    }
                case "TermEntries":
                    {
                        _propertyGridParameters.UpdatePropertyValues(controlViewModel.Index, controlViewModel.TermEntries.ToArray());
                        break;
                    }
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
                    case "Entry":
                        {
                            UpdateButtons();
                            GetEntryIdButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.Entry);
                            _entryTextBox.Text = _morphPropertyViewModel.Entry;
                            break;
                        }
                    case "LeftEntry":
                    case "LeftId":
                    case "IsLeftChecked":
                        {
                            UpdateLeftControls();
                            break;
                        }
                    case "RightEntry":
                    case "RightId":
                    case "IsRightChecked":
                        {
                            UpdateRightControls();
                            break;
                        }
                    case "EntryId":
                        {
                            UpdateButtons();
                            EntryIdLabel.Text = _translateService.Translate($"{Name}.{EntryIdLabel.Name}", _morphPropertyViewModel.EntryId);
                            break;
                        }
                    case "Base":
                        {
                            BaseComboBox.SelectedIndex = (int)_morphPropertyViewModel.Base;
                            UpdateLeftControls();
                            UpdateRightControls();
                            break;
                        }
                    case "IsVirtual":
                        {
                            IsVirtualCheckBox.Checked = _morphPropertyViewModel.IsVirtual;
                            break;
                        }
                    case "IsBusy":
                        {
                            UpdateButtons();
                            break;
                        }
                    case "IsLockEntryIdChecked":
                        {
                            LockIdCheckBox.Checked = _morphPropertyViewModel.IsLockEntryIdChecked;
                            break;
                        }
                    case "AutoSymbolReplace":
                        {
                            _entryTextBox.AutoSymbolReplace = _morphPropertyViewModel.AutoSymbolReplace;
                            _leftTextBox.AutoSymbolReplace = _morphPropertyViewModel.AutoSymbolReplace;
                            _rightTextBox.AutoSymbolReplace = _morphPropertyViewModel.AutoSymbolReplace;
                            break;
                        }
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
            LeftIdLabel.Text = _translateService.Translate($"{Name}.{LeftIdLabel.Name}", _morphPropertyViewModel.LeftId);


            switch (_morphPropertyViewModel.Base)
            {
                case MorphBase.None:
                    {
                        LeftCheckBox.Enabled = false;
                        _leftTextBox.Enabled = false;
                        GetLeftIdButton.Enabled = false;
                        LeftIdLabel.ForeColor = Color.Black;
                        break;
                    }
                default:
                    {
                        LeftCheckBox.Enabled = true;
                        _leftTextBox.Enabled = _morphPropertyViewModel.IsLeftChecked;
                        GetLeftIdButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.LeftEntry) && _morphPropertyViewModel.IsLeftChecked;
                        LeftIdLabel.ForeColor = _morphPropertyViewModel.IsLeftChecked && _morphPropertyViewModel.LeftId == 0 ? Color.Red : Color.Black;
                        break;
                    }
            }
        }

        /// <summary>
        /// Updates the controls for the right part of morphological entry.
        /// </summary>
        private void UpdateRightControls()
        {
            RightCheckBox.Checked = _morphPropertyViewModel.IsRightChecked;
            _rightTextBox.Text = _morphPropertyViewModel.RightEntry;
            RightIdLabel.Text = _translateService.Translate($"{Name}.{RightIdLabel.Name}", _morphPropertyViewModel.RightId);

            switch (_morphPropertyViewModel.Base)
            {
                case MorphBase.None:
                    {
                        RightCheckBox.Enabled = false;
                        _rightTextBox.Enabled = false;
                        GetRightIdButton.Enabled = false;
                        RightIdLabel.ForeColor = Color.Black;
                        break;
                    }
                default:
                    {
                        RightCheckBox.Enabled = true;
                        _rightTextBox.Enabled = _morphPropertyViewModel.IsRightChecked;
                        GetRightIdButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.RightEntry) && _morphPropertyViewModel.IsRightChecked;
                        RightIdLabel.ForeColor = _morphPropertyViewModel.IsRightChecked && _morphPropertyViewModel.RightId == 0 ? Color.Red : Color.Black;
                        break;
                    }
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
                StartButton.Text = _translateService.Translate($"{Name}.StopButton");
            }
            else
            {
                ResetButton.Enabled = true;
                DeleteButton.Enabled = _morphPropertyViewModel.EntryId != 0;
                SaveButton.Enabled = !string.IsNullOrEmpty(_morphPropertyViewModel.Entry);

                StartButton.Enabled = SaveButton.Enabled;
                StartButton.Image = Properties.Resources.start;
                StartButton.Text = _translateService.Translate($"{Name}.{StartButton.Name}");
            }
        }

        /// <summary>
        /// Event handler for the PropertyGridParameters dynamic property value changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void PropertyGridParameters_DynamicPropertyValueChanged(object sender, DynamicPropertyEventArgs e)
        {
            _morphPropertyViewModel.Properties[e.PropertyIndex].SelectedIndex = e.NewValueIndex;
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
                    {
                        _morphPropertyViewModel.LeftEntry = textBox.Text;
                        break;
                    }
                case "RightTextBox":
                    {
                        _morphPropertyViewModel.RightEntry = textBox.Text;
                        break;
                    }
                default:
                    {
                        _morphPropertyViewModel.Entry = textBox.Text;
                        break;
                    }
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
        }

        /// <summary>
        /// Event handler for the IsVirtualCheckBox checked changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void IsVirtualCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _morphPropertyViewModel.IsVirtual = IsVirtualCheckBox.Checked;
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
                    {
                        _morphPropertyViewModel.GetLeftIdCommand.Execute(null);
                        break;
                    }
                case "GetRightIdButton":
                    {
                        _morphPropertyViewModel.GetRightIdCommand.Execute(null);
                        break;
                    }
                default:
                    {
                        _morphPropertyViewModel.GetEntryIdCommand.Execute(null);
                        break;
                    }
            }

            using var form = new GetEntryIdForm();

            if (!form.IsDisposed)
            {
                form.ShowDialog();
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
            if (StartButton.Text == _translateService.Translate($"{Name}.{StartButton.Name}"))
            {
                _morphPropertyViewModel.StartCommand.Execute(null);
            }
            else
            {
                StartButton.Enabled = false;
                _morphPropertyViewModel.StopCommand.Execute(null);
            }
        }

        /// <summary>
        /// Event handler for the MorphPropertyForm visible changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphPropertyForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                _morphPropertyViewModel.OnAppearing();
            }
            else
            {
                _morphPropertyViewModel.OnDisappearing();
            }
        }
    }
}
