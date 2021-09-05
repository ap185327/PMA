// <copyright file="GetEntryIdForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;

namespace PMA.WinForms.Forms
{
    public partial class GetEntryIdForm : Form
    {
        /// <summary>
        /// The get entry ID view model.
        /// </summary>
        private IGetEntryIdViewModel _getEntryIdViewModel;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEntryIdForm"/> class.
        /// </summary>
        /// <param name="viewModel">The get entry ID view model.</param>
        public GetEntryIdForm(IGetEntryIdViewModel viewModel)
        {
            if (viewModel.MorphEntries.Count == 0)
            {
                Close();
                return;
            }

            _getEntryIdViewModel = viewModel;
            _settingService = Program.Scope.Resolve<ISettingService>();

            InitializeComponent();
            OverrideStrings();
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
            IdColumnHeader.Text = Properties.Resources.ResourceManager.GetString("GetEntryIdForm.IdColumnHeader");
            EntryColumnHeader.Text = Properties.Resources.ResourceManager.GetString("GetEntryIdForm.EntryColumnHeader");
            InfoColumnHeader.Text = Properties.Resources.ResourceManager.GetString("GetEntryIdForm.InfoColumnHeader");
            BaseColumnHeader.Text = Properties.Resources.ResourceManager.GetString("GetEntryIdForm.BaseColumnHeader");
            LeftColumnHeader.Text = Properties.Resources.ResourceManager.GetString("GetEntryIdForm.LeftColumnHeader");
            RightColumnHeader.Text = Properties.Resources.ResourceManager.GetString("GetEntryIdForm.RightColumnHeader");
            IsVirtualColumnHeader.Text = Properties.Resources.ResourceManager.GetString("GetEntryIdForm.IsVirtualColumnHeader");
            DeleteToolStripMenuItem.Text = Properties.Resources.ResourceManager.GetString("GetEntryIdForm.DeleteToolStripMenuItem");
        }

        /// <summary>
        /// Sets default values for control and fields.
        /// </summary>
        private void SetDefaultValues()
        {
            foreach (var item in _getEntryIdViewModel.MorphEntries)
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
                            item.Right,
                            item.IsVirtual.ToString().ToLower()
                        }
                    )
                { Tag = item };

                EntryListView.Items.Add(listViewItem);
            }
        }

        /// <summary>
        /// Sets setting values for controls and fields.
        /// </summary>
        private void SetSettings()
        {
            Text = string.Format(Properties.Resources.ResourceManager.GetString("GetEntryIdForm.Title")!, _getEntryIdViewModel.Entry);
            Height = _settingService.GetValue<int>("WinForms.GetEntryIdForm.Height");
            Width = _settingService.GetValue<int>("WinForms.GetEntryIdForm.Width");
            IdColumnHeader.Width = _settingService.GetValue<int>("WinForms.GetEntryIdForm.IdColumnHeader.Width");
            EntryColumnHeader.Width = _settingService.GetValue<int>("WinForms.GetEntryIdForm.EntryColumnHeader.Width");
            InfoColumnHeader.Width = _settingService.GetValue<int>("WinForms.GetEntryIdForm.InfoColumnHeader.Width");
            BaseColumnHeader.Width = _settingService.GetValue<int>("WinForms.GetEntryIdForm.BaseColumnHeader.Width");
            LeftColumnHeader.Width = _settingService.GetValue<int>("WinForms.GetEntryIdForm.LeftColumnHeader.Width");
            RightColumnHeader.Width = _settingService.GetValue<int>("WinForms.GetEntryIdForm.RightColumnHeader.Width");
            IsVirtualColumnHeader.Width = _settingService.GetValue<int>("WinForms.GetEntryIdForm.IsVirtualColumnHeader.Width");
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _getEntryIdViewModel.MorphEntries.CollectionChanged += MorphEntries_CollectionChanged;
        }

        #endregion Initialization methods

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
                                        item.Right,
                                        item.IsVirtual.ToString().ToLower()
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
                            Close();
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

            Close();
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

            EntryListView.Items.Clear();
            _getEntryIdViewModel = null;
        }

        /// <summary>
        /// Unsubscribes events.
        /// </summary>
        private void UnsubscribeEvents()
        {
            _getEntryIdViewModel.MorphEntries.CollectionChanged -= MorphEntries_CollectionChanged;
        }

        /// <summary>
        /// Saves control and field settings.
        /// </summary>
        private void SaveSettings()
        {
            _settingService.SetValue("WinForms.GetEntryIdForm.Height", Height);
            _settingService.SetValue("WinForms.GetEntryIdForm.Width", Width);
            _settingService.SetValue("WinForms.GetEntryIdForm.IdColumnHeader.Width", IdColumnHeader.Width);
            _settingService.SetValue("WinForms.GetEntryIdForm.EntryColumnHeader.Width", EntryColumnHeader.Width);
            _settingService.SetValue("WinForms.GetEntryIdForm.InfoColumnHeader.Width", InfoColumnHeader.Width);
            _settingService.SetValue("WinForms.GetEntryIdForm.BaseColumnHeader.Width", BaseColumnHeader.Width);
            _settingService.SetValue("WinForms.GetEntryIdForm.LeftColumnHeader.Width", LeftColumnHeader.Width);
            _settingService.SetValue("WinForms.GetEntryIdForm.RightColumnHeader.Width", RightColumnHeader.Width);
            _settingService.SetValue("WinForms.GetEntryIdForm.IsVirtualColumnHeader.Width", IsVirtualColumnHeader.Width);
        }
    }
}
