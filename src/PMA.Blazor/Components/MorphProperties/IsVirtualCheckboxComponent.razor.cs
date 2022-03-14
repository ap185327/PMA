// <copyright file="IsVirtualCheckboxComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor;
using PMA.Domain.Interfaces.ViewModels;
using System;
using System.ComponentModel;

namespace PMA.Blazor.Components.MorphProperties
{
    public partial class IsVirtualCheckboxComponent : IDisposable
    {
        #region Injections

        [Inject] private IMorphPropertyViewModel ViewModel { get; set; }

        #endregion

        #region Overrides of ComponentBase

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        protected override void OnInitialized()
        {
            ViewModel.PropertyChanged += OnPropertyChangedHandler;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ViewModel.PropertyChanged -= OnPropertyChangedHandler;
        }

        #endregion

        #region Private Properties / Fields

        private string Label => Resources.Strings.ResourceManager.GetString("IsVirtualCheckboxComponent.Label");

        private Color Color => ViewModel.IsVirtual is null ? Color.Error : Color.Primary;

        #endregion

        #region Private Methods

        private async void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ViewModel.IsVirtual)) return;

            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
