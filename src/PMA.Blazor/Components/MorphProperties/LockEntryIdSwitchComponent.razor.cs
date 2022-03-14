// <copyright file="LockEntryIdSwitchComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.ViewModels;

namespace PMA.Blazor.Components.MorphProperties
{
    public partial class LockEntryIdSwitchComponent
    {
        #region Injections

        [Inject] private IMorphPropertyViewModel ViewModel { get; set; }

        #endregion

        #region Private Properties / Fields

        private string Label => Resources.Strings.ResourceManager.GetString("LockEntryIdSwitchComponent.Label");

        #endregion
    }
}
