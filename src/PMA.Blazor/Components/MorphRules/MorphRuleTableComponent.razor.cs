// <copyright file="MorphRuleTableComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System;
using System.Collections.Specialized;
using PMA.Domain.Enums;

namespace PMA.Blazor.Components.MorphRules
{
    public partial class MorphRuleTableComponent : IDisposable
    {
        #region Injections

        [Inject] private IMorphRuleInfoViewModel ViewModel { get; set; }

        #endregion

        #region Overrides of ComponentBase

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        protected override void OnInitialized()
        {
            ViewModel.Rules.CollectionChanged += OnCollectionChangedEventHandlerAsync;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ViewModel.Rules.CollectionChanged -= OnCollectionChangedEventHandlerAsync;
        }

        #endregion

        #region Private Properties / Fields

        private string IdColumnHeader => Resources.Strings.ResourceManager.GetString("MorphRuleTableComponent.IdColumnHeader");

        private string DescriptionColumnHeader => Resources.Strings.ResourceManager.GetString("MorphRuleTableComponent.DescriptionColumnHeader");

        private TableGroupDefinition<IRuleInfoItemViewModel> _groupDefinition = new()
        {
            Indentation = false,
            Expandable = true,
            IsInitiallyExpanded = true,
            Selector = e => e.Type
        };

        #endregion

        #region Private Methods

        private async void OnCollectionChangedEventHandlerAsync(object sender, NotifyCollectionChangedEventArgs e)
        {
            await InvokeAsync(StateHasChanged);
        }

        private string GetRuleTypeValue(object type)
        {
            return Resources.Strings.ResourceManager.GetString($"RuleType.{type}");
        }

        #endregion
    }
}
