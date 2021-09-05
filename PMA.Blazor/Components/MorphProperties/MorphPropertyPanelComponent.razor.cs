// <copyright file="MorphPropertyPanelComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;
using PMA.Domain.Interfaces.ViewModels;
using System;
using System.Threading.Tasks;

namespace PMA.Blazor.Components.MorphProperties
{
    public partial class MorphPropertyPanelComponent : IDisposable
    {
        #region Injections

        [Inject] private IResizeListenerService ResizeListener { get; set; }

        [Inject] private IMorphPropertyViewModel ViewModel { get; set; }

        #endregion

        #region Parameters

        [CascadingParameter(Name = "Theme")]
        private MudTheme Theme
        {
            get => _theme;
            set
            {
                if (_theme == value) return;

                _theme = value;

                _backgroundColor = $"background: {Theme.Palette.Background}";

                _panelStyle = $"{_height} padding: 12px; overflow-y: auto; {_backgroundColor}";
            }

        }

        #endregion

        #region Overrides of ComponentBase

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
        }

        #endregion

        #region Private Properties / Fields

        private MudTheme _theme;

        private string _backgroundColor;

        private string _height;

        private string _panelStyle;

        private string Title => Resources.Strings.ResourceManager.GetString("MorphPropertyPanelComponent.Title");

        #endregion

        #region Private Methods

        private void OnResizedHandler(object sender, BrowserWindowSize e)
        {
            UpdateStyles(e);
        }

        private void UpdateStyles(BrowserWindowSize size)
        {
            _height = $"height: {size.Height - 465}px;";

            _panelStyle = $"{_height} padding: 12px; overflow-y: auto; {_backgroundColor}";

            StateHasChanged();
        }

        #endregion
    }
}
