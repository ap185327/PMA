// <copyright file="MorphPropertyComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor;
using PMA.Domain.Constants;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Utils.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace PMA.Blazor.Components.MorphProperties
{
    public partial class MorphPropertyComponent : IDisposable
    {
        #region Parameters

        [CascadingParameter(Name = "Theme")]
        private MudTheme Theme
        {
            get => _theme;
            set
            {
                if (_theme == value) return;

                _theme = value;

                UpdateMudInputStyle();
            }

        }

        [Parameter] public IMorphPropertyControlViewModel ViewModel { get; set; }

        #endregion

        #region Overrides of ComponentBase

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        protected override void OnInitialized()
        {
            _disabled = ViewModel.TermIds.Count == 1;

            ViewModel.PropertyChanged += OnPropertyChangedHandler;
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">
        /// Set to <c>true</c> if this is the first time <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> has been invoked
        /// on this component instance; otherwise <c>false</c>.
        /// </param>
        /// <remarks>
        /// The <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> and <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRenderAsync(System.Boolean)" /> lifecycle methods
        /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
        /// Use the <paramref name="firstRender" /> parameter to ensure that initialization work is only performed
        /// once.
        /// </remarks>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                UpdateMudInputStyle();
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)_mudSelect)?.Dispose();

            ViewModel.PropertyChanged -= OnPropertyChangedHandler;
        }

        #endregion

        #region Private Properties / Fields

        private MudTheme _theme;

        private MudSelect<string> _mudSelect;

        private bool _disabled;

        private string PropertyName => ViewModel.Name;

        #endregion

        #region Private Methods

        private async void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IMorphPropertyControlViewModel.SelectedTerm):
                    UpdateMudInputStyle();
                    await InvokeAsync(StateHasChanged);
                    break;
                case nameof(IMorphPropertyControlViewModel.TermEntries):
                    _disabled = ViewModel.TermIds.Count == 1;
                    await InvokeAsync(StateHasChanged);
                    break;
            }
        }

        [SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "<Pending>")]
        private void UpdateMudInputStyle()
        {
            if (_mudSelect is null) return;

            _mudSelect.GetNotPublicValue<MudInput<string>>("_elementReference").Style = ViewModel.TermIds[ViewModel.TermEntries.IndexOf(ViewModel.SelectedTerm)] == MorphConstants.UnknownTermId
                 ? $"color: {Theme.Palette.Warning};"
                 : string.Empty;
        }

        private string GetPropertyValueStyle(string termEntry)
        {
            return ViewModel.TermIds[ViewModel.TermEntries.IndexOf(termEntry)] == MorphConstants.UnknownTermId
                ? $"Color: {Theme.Palette.Warning}"
                : string.Empty;
        }

        #endregion
    }
}
