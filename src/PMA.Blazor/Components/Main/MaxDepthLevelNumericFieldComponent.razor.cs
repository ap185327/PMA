// <copyright file="MaxDepthLevelNumericFieldComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.ViewModels;

namespace PMA.Blazor.Components.Main
{
    public partial class MaxDepthLevelNumericFieldComponent
    {
        #region Injections

        [Inject] private IMainViewModel ViewModel { get; set; }

        #endregion

        #region Parameters

        /// <summary>
        /// User class names, separated by space.
        /// </summary>
        [Parameter] public string Class { get; set; }

        #endregion

        #region Private Properties / Fields

        private string TooltipText => Resources.Strings.ResourceManager.GetString("MaxDepthLevelNumericFieldComponent.TooltipText");

        private string Label => Resources.Strings.ResourceManager.GetString("MaxDepthLevelNumericFieldComponent.Label");

        #endregion
    }
}
