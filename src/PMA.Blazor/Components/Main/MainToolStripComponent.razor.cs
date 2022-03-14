// <copyright file="MainToolStripComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;

namespace PMA.Blazor.Components.Main
{
    public partial class MainToolStripComponent
    {
        #region Parameters

        [Parameter]
        public bool DarkMode
        {
            get => _darkMode;
            set
            {
                if (_darkMode == value) return;

                _darkMode = value;

                DarkModeChanged.InvokeAsync(_darkMode);
            }
        }

        [Parameter] public EventCallback<bool> DarkModeChanged { get; set; }

        #endregion

        #region Private Properties / Fields

        private bool _darkMode;

        #endregion
    }
}
