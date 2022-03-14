// <copyright file="StatusStripComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.ViewModels;
using System;
using System.ComponentModel;

namespace PMA.Blazor.Components.Main
{
    public partial class StatusStripComponent : IDisposable
    {
        #region Injections

        [Inject] private IMainViewModel ViewModel { get; set; }

        #endregion

        #region Overrides of ComponentBase

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// Override this method if you will perform an asynchronous operation and
        /// want the component to refresh when that operation is completed.
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing any asynchronous operation.</returns>
        protected override void OnInitialized()
        {
            ViewModel.PropertyChanged += OnPropertyChangedHandler;
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ViewModel.PropertyChanged -= OnPropertyChangedHandler;
        }

        #endregion

        #region Private Methods

        private async void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
