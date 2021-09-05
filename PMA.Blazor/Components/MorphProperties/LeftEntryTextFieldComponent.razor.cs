// <copyright file="LeftEntryTextFieldComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Utils.Extensions;
using System;
using System.ComponentModel;

namespace PMA.Blazor.Components.MorphProperties
{
    public partial class LeftEntryTextFieldComponent : IDisposable
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
                _mudInput = _mudTextField.GetNotPublicValue<MudInput<string>>("_elementReference");
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)_mudTextField)?.Dispose();
            ((IDisposable)_mudInput)?.Dispose();

            ViewModel.PropertyChanged -= OnPropertyChangedHandler;
        }

        #endregion

        #region Private Properties / Fields

        private MudTextField<string> _mudTextField;

        private MudInput<string> _mudInput;

        private string _textValue;

        private string TextValue
        {
            get => _textValue;
            set
            {
                if (_textValue == value) return;

                _textValue = value;

                _mudInput.SetText(_textValue);

                ViewModel.LeftEntry = _textValue;
            }
        }

        private string Label => Resources.Strings.ResourceManager.GetString("LeftEntryTextFieldComponent.Label");

        private string HelperText => string.Format(Resources.Strings.ResourceManager.GetString("EntryTextFieldComponent.HelperText")!, ViewModel.LeftId);

        private bool Disabled => !ViewModel.IsLeftChecked || ViewModel.Base == MorphBase.None;

        private bool Error => !Disabled && ViewModel.LeftId == 0;

        #endregion

        #region Private Methods

        private async void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.LeftEntry))
            {
                TextValue = ViewModel.LeftEntry;
            }

            await InvokeAsync(StateHasChanged);
        }

        private void OnAdornmentClick()
        {
            ViewModel.GetLeftIdCommand.Execute(null);
        }

        #endregion
    }
}
