// <copyright file="MorphRuleInfoForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.WinForms.Extensions;
using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace PMA.WinForms.Forms
{
    public partial class MorphRuleInfoForm : Form
    {
        /// <summary>
        /// The morphological rule information view model.
        /// </summary>
        private readonly IMorphRuleInfoViewModel _morphRuleInfoViewModel;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MorphRuleInfoForm"/> class.
        /// </summary>
        public MorphRuleInfoForm()
        {
            _morphRuleInfoViewModel = Program.Scope.Resolve<IMorphRuleInfoViewModel>();
            _settingService = Program.Scope.Resolve<ISettingService>();

            _morphRuleInfoViewModel.IsActive = true;

            InitializeComponent();
            OverrideStrings();
            SetSettings();
            SubscribeEvents();
        }

        #region Initialization methods

        /// <summary>
        /// Overrides control strings.
        /// </summary>
        private void OverrideStrings()
        {
            Text = Properties.Resources.ResourceManager.GetString("MorphRuleInfoForm.Title");
            IdColumnHeader.Text = Properties.Resources.ResourceManager.GetString("MorphRuleInfoForm.IdColumnHeader");
            DescriptionColumnHeader.Text = Properties.Resources.ResourceManager.GetString("MorphRuleInfoForm.DescriptionColumnHeader");
            RuleListView.Groups[0].Header = Properties.Resources.ResourceManager.GetString("MorphRuleInfoForm.MorphRuleListViewGroup")!;
            RuleListView.Groups[1].Header = Properties.Resources.ResourceManager.GetString("MorphRuleInfoForm.SandhiRuleListViewGroup")!;
        }

        /// <summary>
        /// Sets setting values for controls and fields.
        /// </summary>
        private void SetSettings()
        {
            Height = _settingService.GetValue<int>("WinForms.MorphRuleInfoForm.Height");
            Width = _settingService.GetValue<int>("WinForms.MorphRuleInfoForm.Width");
            IdColumnHeader.Width = _settingService.GetValue<int>("WinForms.MorphRuleInfoForm.IdColumnHeader.Width");
            DescriptionColumnHeader.Width = _settingService.GetValue<int>("WinForms.MorphRuleInfoForm.DescriptionColumnHeader.Width");
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _morphRuleInfoViewModel.Rules.CollectionChanged += OnCollectionChangedEventHandler;
        }

        #endregion Initialization methods

        /// <summary>
        /// Event handler for the rule collection changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        foreach (IRuleInfoItemViewModel item in e.NewItems)
                        {
                            var listViewItem = new ListViewItem
                            {
                                Text = item.Id.ToString(),
                                Group = item.Type == RuleType.Morphological
                                    ? RuleListView.Groups[0]
                                    : RuleListView.Groups[1]
                            };
                            listViewItem.SubItems.Add(item.Description);
                            RuleListView.Items.Add(listViewItem);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Reset:
                    {
                        RuleListView.Items.Clear();
                        break;
                    }
            }
        }

        /// <summary>
        /// Event handler for the RuleListView column width changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void RuleListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            _settingService.SetValue("WinForms.MorphRuleInfoForm.IdColumnHeader.Width", IdColumnHeader.Width);
            _settingService.SetValue("WinForms.MorphRuleInfoForm.DescriptionColumnHeader.Width", DescriptionColumnHeader.Width);
        }

        /// <summary>
        /// Event handler for the MorphRuleInfoForm location changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphRuleInfoForm_LocationChanged(object sender, EventArgs e)
        {
            this.Sticking();
        }
    }
}
