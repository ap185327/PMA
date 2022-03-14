// <copyright file="OptionForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using PMA.Domain.Interfaces.ViewModels;
using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace PMA.WinForms.Forms
{
    public partial class OptionForm : Form
    {
        /// <summary>
        /// The option view model.
        /// </summary>
        private readonly IOptionViewModel _optionViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionForm"/> class.
        /// </summary>
        public OptionForm()
        {
            _optionViewModel = Program.Scope.Resolve<IOptionViewModel>();

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
            Text = Properties.Resources.ResourceManager.GetString("OptionForm.Title");
            TermGroupBox.Text = Properties.Resources.ResourceManager.GetString("OptionForm.TermGroupBox");
            AvailableTermLabel.Text = Properties.Resources.ResourceManager.GetString("OptionForm.AvailableTermLabel");
            ShownTermLabel.Text = Properties.Resources.ResourceManager.GetString("OptionForm.ShownTermLabel");
            AddButton.Text = Properties.Resources.ResourceManager.GetString("OptionForm.AddButton");
            RemoveButton.Text = Properties.Resources.ResourceManager.GetString("OptionForm.RemoveButton");
            ModeLabel.Text = Properties.Resources.ResourceManager.GetString("OptionForm.ModeLabel");
            RatingGroupBox.Text = Properties.Resources.ResourceManager.GetString("OptionForm.RatingGroupBox");
            OkButton.Text = Properties.Resources.ResourceManager.GetString("OptionForm.OkButton");
            CancelButton.Text = Properties.Resources.ResourceManager.GetString("OptionForm.CancelButton");
        }

        /// <summary>
        /// Sets default values for control and fields.
        /// </summary>
        private void SetDefaultValues()
        {
            ModeComboBox.Items.Add(Properties.Resources.ResourceManager.GetString("OptionForm.Mode.Debug")!);
            ModeComboBox.Items.Add(Properties.Resources.ResourceManager.GetString("OptionForm.Mode.Release")!);

            AvailableListBox.Items.Clear();
            ShownListBox.Items.Clear();
        }

        #endregion Initialization methods

        /// <summary>
        /// Event handler for the ShownTerms collection changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ShownTerms_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ShownListBox.DataSource = null;
            ShownListBox.DataSource = _optionViewModel.ShownTerms;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        string entry = (string)e.NewItems[0];

                        for (int i = 0; i < ShownListBox.Items.Count; i++)
                        {
                            if ((string)ShownListBox.Items[i] != entry) continue;

                            ShownListBox.SetSelected(i, true);
                            break;
                        }
                        RemoveButton.Enabled = true;
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        if (ShownListBox.Items.Count == 0)
                        {
                            RemoveButton.Enabled = false;
                            AddButton.Focus();
                        }
                        else
                        {
                            ShownListBox.SetSelected(0, true);
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Event handler for the AvailableTerms collection changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void AvailableTerms_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            AvailableListBox.DataSource = null;
            AvailableListBox.DataSource = _optionViewModel.AvailableTerms;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        string entry = (string)e.NewItems[0];

                        for (int i = 0; i < AvailableListBox.Items.Count; i++)
                        {
                            if ((string)AvailableListBox.Items[i] != entry) continue;

                            AvailableListBox.SetSelected(i, true);
                            break;
                        }
                        AddButton.Enabled = true;
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        if (AvailableListBox.Items.Count == 0)
                        {
                            AddButton.Enabled = false;
                            RemoveButton.Focus();
                        }
                        else
                        {
                            AvailableListBox.SetSelected(0, true);
                        }
                        break;
                    }
            }


        }

        /// <summary>
        /// Event handler for the AddButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            string entry = (string)AvailableListBox.SelectedItem;
            _optionViewModel.AddTermCommand.Execute(entry);
        }

        /// <summary>
        /// Event handler for the RemoveButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            string entry = (string)ShownListBox.SelectedItem;
            _optionViewModel.RemoveTermCommand.Execute(entry);
        }

        /// <summary>
        /// Event handler for the OkButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        /// <summary>
        /// Event handler for the CancelButton click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Event handler for the RatingTrackBar scroll.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void RatingTrackBar_Scroll(object sender, EventArgs e)
        {
            MorphRuleRatingLabel.Text = string.Format(Properties.Resources.ResourceManager.GetString("OptionForm.MorphRuleRatingLabel")!, (double)RatingTrackBar.Value / RatingTrackBar.Maximum * 100);
            FdictRatingLabel.Text = string.Format(Properties.Resources.ResourceManager.GetString("OptionForm.FdictRatingLabel")!, (double)(RatingTrackBar.Maximum - RatingTrackBar.Value) / RatingTrackBar.Maximum * 100);
        }

        /// <summary>
        /// Saves control and field settings.
        /// </summary>
        private void SaveSettings()
        {
            _optionViewModel.DebugMode = ModeComboBox.SelectedIndex == 0;
            _optionViewModel.FreqRatingRatio = Math.Round((double)(RatingTrackBar.Maximum - RatingTrackBar.Value) / RatingTrackBar.Maximum, 2);

            _optionViewModel.SaveCommand.Execute(null);
        }

        /// <summary>
        /// Event handler for the OptionForm visible changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OptionForm_VisibleChanged(object sender, EventArgs e)
        {
            _optionViewModel.IsActive = Visible;

            if (Visible)
            {
                SetSettings();
                SubscribeEvents();
            }
            else
            {
                UnsubscribeEvents();
            }
        }

        /// <summary>
        /// Sets setting values for controls and fields.
        /// </summary>
        private void SetSettings()
        {
            AvailableListBox.DataSource = null;
            ShownListBox.DataSource = null;

            AvailableListBox.DataSource = _optionViewModel.AvailableTerms;
            ShownListBox.DataSource = _optionViewModel.ShownTerms;

            if (AvailableListBox.Items.Count == 0)
            {
                AddButton.Enabled = false;
            }
            else
            {
                AvailableListBox.SetSelected(0, true);
            }

            if (ShownListBox.Items.Count == 0)
            {
                RemoveButton.Enabled = false;
            }
            else
            {
                ShownListBox.SetSelected(0, true);
            }

            ModeComboBox.SelectedIndex = _optionViewModel.DebugMode ? 0 : 1;

            RatingTrackBar.Value = Convert.ToInt32(RatingTrackBar.Maximum * (1 - _optionViewModel.FreqRatingRatio));
            MorphRuleRatingLabel.Text = string.Format(Properties.Resources.ResourceManager.GetString("OptionForm.MorphRuleRatingLabel")!, (1 - _optionViewModel.FreqRatingRatio) * 100);
            FdictRatingLabel.Text = string.Format(Properties.Resources.ResourceManager.GetString("OptionForm.FdictRatingLabel")!, _optionViewModel.FreqRatingRatio * 100);
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _optionViewModel.AvailableTerms.CollectionChanged += AvailableTerms_CollectionChanged;
            _optionViewModel.ShownTerms.CollectionChanged += ShownTerms_CollectionChanged;
        }

        /// <summary>
        /// Unsubscribes events.
        /// </summary>
        private void UnsubscribeEvents()
        {
            _optionViewModel.AvailableTerms.CollectionChanged -= AvailableTerms_CollectionChanged;
            _optionViewModel.ShownTerms.CollectionChanged -= ShownTerms_CollectionChanged;
        }
    }
}
