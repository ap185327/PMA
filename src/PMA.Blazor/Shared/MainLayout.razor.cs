// <copyright file="MainLayout.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Services;
using PMA.Blazor.Constants;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.ViewModels;
using System;
using System.Threading.Tasks;

namespace PMA.Blazor.Shared
{
    public partial class MainLayout : IDisposable
    {
        #region Injections

        [Inject] private IDialogService DialogService { get; set; }

        [Inject] private IResizeListenerService ResizeListener { get; set; }

        [Inject] private IModalDialogService ModalDialogService { get; set; }

        [Inject] private IMainViewModel MainViewModel { get; set; }

        [Inject] private IMorphSolutionViewModel MorphSolutionViewModel { get; set; }

        [Inject] private IMorphPropertyViewModel MorphPropertyViewModel { get; set; }

        [Inject] private IMorphRuleInfoViewModel MorphRuleInfoViewModel { get; set; }

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
            _currentTheme = _defaultTheme;

            MainViewModel.IsActive = true;
            MorphSolutionViewModel.IsActive = true;
            MorphPropertyViewModel.IsActive = true;
            MorphRuleInfoViewModel.IsActive = true;

            ModalDialogService.ModalDialogShown += OnModalDialogShownEventHandler;
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
            ModalDialogService.ModalDialogShown -= OnModalDialogShownEventHandler;
        }

        #endregion

        #region Private Properties / Fields

        private readonly MudTheme _defaultTheme = new()
        {
            Palette = PaletteConstants.Default,
            Typography = TypographyConstants.Default
        };

        private readonly MudTheme _darkTheme = new()
        {
            Palette = PaletteConstants.Dark,
            Typography = TypographyConstants.Default
        };

        private MudTheme _currentTheme;

        private bool _darkMode;

        private bool DarkMode
        {
            get => _darkMode;
            set
            {
                if (_darkMode == value) return;

                _darkMode = value;

                _currentTheme = _darkMode ? _darkTheme : _defaultTheme;

                StateHasChanged();
            }
        }

        private string _pageStyle = string.Empty;

        #endregion

        #region Private Methods

        private void OnResizedHandler(object sender, BrowserWindowSize e)
        {
            UpdateStyles(e);
        }

        private async void OnModalDialogShownEventHandler(object sender, ModalDialogEventArgs e)
        {
            string title = e.Title;
            string message = e.Message;

            var buttons = e.Buttons;

            string firstButtonText = Resources.Strings.ResourceManager.GetString($"ModalDialog.Buttons.{buttons[0]}");
            string secondButtonText = e.Buttons.Length > 1 ? Resources.Strings.ResourceManager.GetString($"ModalDialog.Buttons.{buttons[1]}") : null;
            string thirdButtonText = e.Buttons.Length > 2 ? Resources.Strings.ResourceManager.GetString($"ModalDialog.Buttons.{buttons[2]}") : null;

            bool? result = await DialogService.ShowMessageBox(title, message, firstButtonText, secondButtonText, thirdButtonText);

            if (!result.HasValue)
            {
                ModalDialogService.PressButton(buttons[2]);
            }
            else if (!result.Value)
            {
                ModalDialogService.PressButton(buttons[1]);
            }
            else
            {
                ModalDialogService.PressButton(buttons[0]);
            }
        }

        private void UpdateStyles(BrowserWindowSize size)
        {
            _pageStyle = $"height: {size.Height - 48}px";

            StateHasChanged();
        }

        #endregion
    }
}
