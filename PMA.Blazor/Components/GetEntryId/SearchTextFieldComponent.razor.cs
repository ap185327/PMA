// <copyright file="SearchTextFieldComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Utils.Extensions;

namespace PMA.Blazor.Components.GetEntryId
{
    public partial class SearchTextFieldComponent
    {
        #region Injections

        [Inject] private IMainViewModel ViewModel { get; set; }

        #endregion

        #region Parameters

        [Parameter]
        public string TextValue
        {
            get => _textValue;
            set
            {
                if (_textValue == value) return;

                _textValue = value;

                if (ViewModel.AutoSymbolReplace)
                {
                    _textValue = value
                        .Replace("aa", "ā")
                        .Replace("ii", "ī")
                        .Replace("uu", "ū")
                        .Replace("~n", "ñ")
                        .Replace(".n", "ṇ")
                        .Replace(".d", "ḍ")
                        .Replace(".l", "ḷ")
                        .Replace(".m", "ṃ")
                        .Replace(".t", "ṭ")
                        .Replace("\"n", "ṅ");

                    if (_textValue != value)
                    {
                        _mudInput.SetText(_textValue);
                    }
                }

                TextValueChanged.InvokeAsync(_textValue);
            }
        }

        [Parameter] public EventCallback<string> TextValueChanged { get; set; }

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
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _mudInput = _mudTextField.GetNotPublicValue<MudInput<string>>("_elementReference");
            }
        }

        #endregion

        #region Private Properties / Fields

        private MudTextField<string> _mudTextField;

        private MudInput<string> _mudInput;

        private string _textValue;

        private string SearchPlaceholder => Resources.Strings.ResourceManager.GetString("SearchTextFieldComponent.Placeholder");

        #endregion
    }
}
