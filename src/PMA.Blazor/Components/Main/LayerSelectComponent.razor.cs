// <copyright file="LayerSelectComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.ViewModels;

namespace PMA.Blazor.Components.Main
{
    public partial class LayerSelectComponent
    {
        #region Injections

        [Inject] private IMainViewModel ViewModel { get; set; }

        #endregion

        #region Private Properties / Fields

        private string TooltipText => Resources.Strings.ResourceManager.GetString("LayerSelectComponent.TooltipText");

        private string Label => Resources.Strings.ResourceManager.GetString("LayerSelectComponent.Label");

        #endregion

        #region Private Methods

        private string GetLayerValue(int layer)
        {
            return Resources.Strings.ResourceManager.GetString($"LayerSelectComponent.Layer.{layer}");
        }

        #endregion
    }
}
