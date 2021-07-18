// <copyright file="GetEntryIdForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.WinForms.Helpers;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace PMA.WinForms.Forms
{
    public partial class GetEntryIdForm : Form
    {
        /// <summary>
        /// The get entry ID view model.
        /// </summary>
        private readonly IGetEntryIdViewModel _getEntryIdViewModel;

        /// <summary>
        /// The translate service.
        /// </summary>
        private readonly ITranslateService _translateService;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEntryIdForm"/> class.
        /// </summary>
        public GetEntryIdForm()
        {
            _getEntryIdViewModel = Program.Scope.Resolve<IGetEntryIdViewModel>();
            _translateService = Program.Scope.Resolve<ITranslateService>();
            _settingService = Program.Scope.Resolve<ISettingService>();

            InitializeComponent();
            OverrideStrings();
            SetDefaultValues();

            SubscribeEvents();

            _getEntryIdViewModel.OnAppearing();

            SetSettings();

        }

        #region Initialization methods

        /// <summary>
        /// Overrides control strings.
        /// </summary>
        private void OverrideStrings()
        {
            IdColumnHeader.Text = _translateService.Translate($"{Name}.IdColumnHeader");
            EntryColumnHeader.Text = _translateService.Translate($"{Name}.EntryColumnHeader");
            InfoColumnHeader.Text = _translateService.Translate($"{Name}.InfoColumnHeader");
            BaseColumnHeader.Text = _translateService.Translate($"{Name}.BaseColumnHeader");
            LeftColumnHeader.Text = _translateService.Translate($"{Name}.LeftColumnHeader");
            RightColumnHeader.Text = _translateService.Translate($"{Name}.RightColumnHeader");
            IsVirtualColumnHeader.Text = _translateService.Translate($"{Name}.IsVirtualColumnHeader");
            DeleteToolStripMenuItem.Text = _translateService.Translate($"{Name}.{DeleteToolStripMenuItem.Name}");
        }

        /// <summary>
        /// Sets default values for control and fields.
        /// </summary>
        private void SetDefaultValues()
        {
            EntryListView.Items.Clear();
        }

        /// <summary>
        /// Sets setting values for controls and fields.
        /// </summary>
        private void SetSettings()
        {
            Height = _settingService.GetValue<int>($"{Name}.Height");
            Width = _settingService.GetValue<int>($"{Name}.Width");
            IdColumnHeader.Width = _settingService.GetValue<int>($"{Name}.IdColumnHeader.Width");
            EntryColumnHeader.Width = _settingService.GetValue<int>($"{Name}.EntryColumnHeader.Width");
            InfoColumnHeader.Width = _settingService.GetValue<int>($"{Name}.InfoColumnHeader.Width");
            BaseColumnHeader.Width = _settingService.GetValue<int>($"{Name}.BaseColumnHeader.Width");
            LeftColumnHeader.Width = _settingService.GetValue<int>($"{Name}.LeftColumnHeader.Width");
            RightColumnHeader.Width = _settingService.GetValue<int>($"{Name}.RightColumnHeader.Width");
            IsVirtualColumnHeader.Width = _settingService.GetValue<int>($"{Name}.IsVirtualColumnHeader.Width");
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _getEntryIdViewModel.ShowModalDialog += GetEntryIdViewModel_ShowModalDialog;
            _getEntryIdViewModel.PropertyChanged += GetEntryIdViewModel_PropertyChanged;
            _getEntryIdViewModel.MorphEntries.CollectionChanged += MorphEntries_CollectionChanged;
        }

        #endregion Initialization methods

        /// <summary>
        /// Event handler for the view model property dialog shown.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void GetEntryIdViewModel_ShowModalDialog(object sender, ModalDialogEventArgs e)
        {
            var buttons = MessageBoxHelper.GetButtons(e.Buttons);
            var icon = MessageBoxHelper.GetIcon(e.Type);

            var result = MessageBox.Show(e.Message, e.Title, buttons, icon);

            int index = MessageBoxHelper.GetButtonIndex(result, buttons);

            _getEntryIdViewModel.PressModalDialogButtonCommand.Execute(index);

            if (EntryListView.Items.Count == 0)
            {
                CustomClose();
            }
        }

        /// <summary>
        /// Event handler for the GetEntryIdViewModel property changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void GetEntryIdViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Entry":
                    {
                        Text = _translateService.Translate($"{Name}", _getEntryIdViewModel.Entry);
                        break;
                    }
            }
        }

        /// <summary>
        /// Event handler for the MorphEntries collection changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        foreach (IGetEntryIdControlViewModel item in e.NewItems)
                        {
                            var listViewItem = new ListViewItem
                                (
                                    new[]
                                    {
                                    item.Id.ToString(),
                                    item.Entry,
                                    item.Parameters,
                                    item.Base,
                                    item.Left,
                                    item.Right, item.IsVirtual.ToString().ToLower()
                                    }
                                )
                            { Tag = item };

                            EntryListView.Items.Add(listViewItem);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        foreach (IGetEntryIdControlViewModel item in e.OldItems)
                        {
                            var listItem = EntryListView.Items.Cast<ListViewItem>().Single(x => x.Text == item.Id.ToString());

                            EntryListView.Items.Remove(listItem);
                        }

                        if (EntryListView.Items.Count == 0)
                        {
                            CustomClose();
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Reset:
                    {
                        EntryListView.Items.Clear();
                        break;
                    }
            }
        }

        /// <summary>
        /// Event handler for the EntryListView item selection changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void EntryListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ((IGetEntryIdControlViewModel)e.Item.Tag).IsSelected = e.IsSelected;
        }

        /// <summary>
        /// Event handler for the EntryListView mouse double click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void EntryListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ((IGetEntryIdControlViewModel)EntryListView.SelectedItems[0].Tag).SelectCommand.Execute(null);
            UnsubscribeEvents();
            SaveSettings();
            _getEntryIdViewModel.OnDisappearing();
            CustomClose();
        }

        /// <summary>
        /// Event handler for the DeleteToolStripMenuItem click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _getEntryIdViewModel.DeleteCommand.Execute(null);
        }

        /// <summary>
        /// Event handler for the GetEntryIdForm closing.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void GetEntryIdForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnsubscribeEvents();
            SaveSettings();
            _getEntryIdViewModel.OnDisappearing();
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        private void CustomClose()
        {
            UnsubscribeEvents();
            SaveSettings();
            _getEntryIdViewModel.OnDisappearing();
            Close();
        }

        /// <summary>
        /// Unsubscribes events.
        /// </summary>
        private void UnsubscribeEvents()
        {
            _getEntryIdViewModel.ShowModalDialog -= GetEntryIdViewModel_ShowModalDialog;
            _getEntryIdViewModel.PropertyChanged -= GetEntryIdViewModel_PropertyChanged;
            _getEntryIdViewModel.MorphEntries.CollectionChanged -= MorphEntries_CollectionChanged;
        }

        /// <summary>
        /// Saves control and field settings.
        /// </summary>
        private void SaveSettings()
        {
            _settingService.SetValue($"{Name}.Height", Height);
            _settingService.SetValue($"{Name}.Width", Width);
            _settingService.SetValue($"{Name}.IdColumnHeader.Width", IdColumnHeader.Width);
            _settingService.SetValue($"{Name}.EntryColumnHeader.Width", EntryColumnHeader.Width);
            _settingService.SetValue($"{Name}.InfoColumnHeader.Width", InfoColumnHeader.Width);
            _settingService.SetValue($"{Name}.BaseColumnHeader.Width", BaseColumnHeader.Width);
            _settingService.SetValue($"{Name}.LeftColumnHeader.Width", LeftColumnHeader.Width);
            _settingService.SetValue($"{Name}.RightColumnHeader.Width", RightColumnHeader.Width);
            _settingService.SetValue($"{Name}.IsVirtualColumnHeader.Width", IsVirtualColumnHeader.Width);
        }
    }
}
