// <copyright file="MorphPropertyContainerComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PMA.Blazor.Components.MorphProperties
{
    public partial class MorphPropertyContainerComponent : IDisposable
    {
        #region Injections

        [Inject] private IResizeListenerService ResizeListener { get; set; }

        [Inject] private IMorphPropertyViewModel ViewModel { get; set; }

        [Inject] private ISettingService SettingService { get; set; }

        #endregion

        #region Overrides of ComponentBase

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        protected override void OnInitialized()
        {
            _isVisible = SettingService.GetValue<bool>("Blazor.Desktop.MorphPropertyContainerComponent.IsVisible");

            ViewModel.PropertyChanged += OnPropertyChangedHandlerAsync;
            SettingService.SettingChanged += OnSettingChangedHandlerAsync;
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

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            ResizeListener.OnResized -= OnResizedHandler;
            ViewModel.PropertyChanged -= OnPropertyChangedHandlerAsync;
            SettingService.SettingChanged -= OnSettingChangedHandlerAsync;
        }

        #endregion

        #region Private Properties / Fields

        private string _containerStyle = string.Empty;

        private bool _isVisible;

        #endregion

        #region Private Methods

        private void OnResizedHandler(object sender, BrowserWindowSize e)
        {
            UpdateStyles(e);
        }

        private async void OnPropertyChangedHandlerAsync(object sender, PropertyChangedEventArgs e)
        {
            await InvokeAsync(StateHasChanged);
        }

        private async void OnSettingChangedHandlerAsync(object sender, SettingEventArgs e)
        {
            switch (e.SettingName)
            {
                case "Blazor.Desktop.MorphPropertyContainerComponent.IsVisible":
                    _isVisible = SettingService.GetValue<bool>(e.SettingName);
                    await InvokeAsync(StateHasChanged);
                    break;
            }
        }

        private void UpdateStyles(BrowserWindowSize size)
        {
            _containerStyle = $"height: {size.Height - 112}px";

            StateHasChanged();
        }

        #endregion
    }
}
