// <copyright file="MorphRuleInfoForm.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Autofac;
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
        /// The translate service.
        /// </summary>
        private readonly ITranslateService _translateService;

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
            _translateService = Program.Scope.Resolve<ITranslateService>();
            _settingService = Program.Scope.Resolve<ISettingService>();

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
            Text = _translateService.Translate(Name);
            IdColumnHeader.Text = _translateService.Translate($"{Name}.IdColumnHeader");
            DescriptionColumnHeader.Text = _translateService.Translate($"{Name}.DescriptionColumnHeader");
            RuleListView.Groups[0].Header = _translateService.Translate($"{Name}.MorphRuleListViewGroup");
            RuleListView.Groups[1].Header = _translateService.Translate($"{Name}.SandhiRuleListViewGroup");
        }

        /// <summary>
        /// Sets setting values for controls and fields.
        /// </summary>
        private void SetSettings()
        {
            Height = _settingService.GetValue<int>($"{Name}.Height");
            Width = _settingService.GetValue<int>($"{Name}.Width");
            IdColumnHeader.Width = _settingService.GetValue<int>($"{Name}.IdColumnHeader.Width");
            DescriptionColumnHeader.Width = _settingService.GetValue<int>($"{Name}.DescriptionColumnHeader.Width");
        }

        /// <summary>
        /// Subscribes events.
        /// </summary>
        private void SubscribeEvents()
        {
            _morphRuleInfoViewModel.MorphRules.CollectionChanged += MorphRules_CollectionChanged;
            _morphRuleInfoViewModel.SandhiRules.CollectionChanged += SandhiRules_CollectionChanged;
        }

        #endregion Initialization methods

        /// <summary>
        /// Event handler for the MorphRules collection changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphRules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                                Group = RuleListView.Groups[0]
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
        /// Event handler for the SandhiRules collection changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void SandhiRules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                                Group = RuleListView.Groups[1]
                            };
                            listViewItem.SubItems.Add(item.Description);
                            RuleListView.Items.Add(listViewItem);
                        }
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
            _settingService.SetValue($"{Name}.IdColumnHeader.Width", IdColumnHeader.Width);
            _settingService.SetValue($"{Name}.DescriptionColumnHeader.Width", DescriptionColumnHeader.Width);
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

        /// <summary>
        /// Event handler for the MorphRuleInfoForm visible changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MorphRuleInfoForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                _morphRuleInfoViewModel.OnAppearing();
            }
            else
            {
                _morphRuleInfoViewModel.OnDisappearing();
            }
        }
    }
}
