// <copyright file="InputTextFieldComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.ViewModels;

namespace PMA.Blazor.Components.Main
{
    public partial class InputTextFieldComponent
    {
        #region Injections

        [Inject] private IMainViewModel ViewModel { get; set; }

        #endregion

        #region Private Properties / Fields

        private string Label => Resources.Strings.ResourceManager.GetString("InputTextFieldComponent.Label");

        #endregion
    }
}
