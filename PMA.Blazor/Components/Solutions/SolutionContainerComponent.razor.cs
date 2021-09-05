// <copyright file="SolutionContainerComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using PMA.Domain.Interfaces.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PMA.Blazor.Components.Solutions
{
    public partial class SolutionContainerComponent : IDisposable
    {
        #region Injections

        [Inject] private IResizeListenerService ResizeListener { get; set; }

        [Inject] private IMorphSolutionViewModel ViewModel { get; set; }

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

        /// <summary>
        /// Method invoked after each time the component has been rendered. Note that the component does
        /// not automatically re-render after the completion of any returned <see cref="T:System.Threading.Tasks.Task" />, because
        /// that would cause an infinite render loop.
        /// </summary>
        /// <param name="firstRender">
        /// Set to <c>true</c> if this is the first time <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> has been invoked
        /// on this component instance; otherwise <c>false</c>.
        /// </param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing any asynchronous operation.</returns>
        /// <remarks>
        /// The <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> and <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRenderAsync(System.Boolean)" /> lifecycle methods
        /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
        /// Use the <paramref name="firstRender" /> parameter to ensure that initialization work is only performed
        /// once.
        /// </remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var size = await ResizeListener.GetBrowserWindowSize();

                UpdateStyles(size);

                ResizeListener.OnResized += OnResizedHandler;
            }
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ResizeListener.OnResized -= OnResizedHandler;
            ViewModel.PropertyChanged -= OnPropertyChangedHandler;
        }

        #endregion

        #region Private Properties / Fields

        private string _sandhiLine;

        private string _containerStyle;

        #endregion

        #region Private Methods

        private void OnResizedHandler(object sender, BrowserWindowSize e)
        {
            UpdateStyles(e);
        }

        private async void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IMorphSolutionViewModel.IsBusy) && ViewModel.IsBusy)
            {
                _sandhiLine = string.Empty;
            }

            await InvokeAsync(StateHasChanged);
        }

        private void UpdateStyles(BrowserWindowSize size)
        {
            _containerStyle = $"height: {size.Height - 150}px; overflow: auto";

            StateHasChanged();
        }

        #endregion
    }
}
