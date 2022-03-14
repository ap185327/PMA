// <copyright file="SaveButtonComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.ViewModels;
using System;
using System.ComponentModel;

namespace PMA.Blazor.Components.MorphProperties
{
    public partial class SaveButtonComponent : IDisposable
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

        private string Text => Resources.Strings.ResourceManager.GetString("SaveButtonComponent.Text");

        private bool Disabled =>
            string.IsNullOrEmpty(ViewModel.Entry) ||
            ViewModel.IsVirtual is null ||
            ViewModel.Base != MorphBase.None && (ViewModel.IsLeftChecked && ViewModel.LeftId == 0 ||
                                                 ViewModel.IsRightChecked && ViewModel.RightId == 0);

        #endregion

        #region Private Methods

        private async void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
