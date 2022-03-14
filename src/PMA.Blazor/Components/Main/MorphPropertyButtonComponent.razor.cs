// <copyright file="MorphPropertyButtonComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.Services;

namespace PMA.Blazor.Components.Main
{
    public partial class MorphPropertyButtonComponent
    {
        #region Injections

        [Inject] private ISettingService SettingService { get; set; }

        #endregion

        #region Private Properties / Fields

        private string TooltipText => Resources.Strings.ResourceManager.GetString("MorphPropertyButtonComponent.TooltipText");

        #endregion

        #region Private Methods

        private void OnButtonClick()
        {
            bool isVisible = SettingService.GetValue<bool>("Blazor.Desktop.MorphPropertyContainerComponent.IsVisible");

            SettingService.SetValue("Blazor.Desktop.MorphPropertyContainerComponent.IsVisible", !isVisible);
        }

        #endregion
    }
}
