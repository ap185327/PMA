﻿// <copyright file="StartMorphAnalysisButtonComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.ViewModels;
using System;
using System.ComponentModel;

namespace PMA.Blazor.Components.Main
{
    public partial class StartMorphAnalysisButtonComponent : IDisposable
    {
        #region Injections

        [Inject] private IMainViewModel ViewModel { get; set; }

        #endregion

        #region Parameters

        [Parameter] public string Class { get; set; }

        #endregion

        #region Overrides of ComponentBase

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        protected override void OnInitialized()
        {
            ViewModel.PropertyChanged += OnPropertyChangedHandlerAsync;
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ViewModel.PropertyChanged -= OnPropertyChangedHandlerAsync;
        }

        #endregion

        #region Private Properties / Fields

        private string StartButtonText => Resources.Strings.ResourceManager.GetString("StartMorphAnalysisButtonComponent.StartText");
        private string StopButtonText => Resources.Strings.ResourceManager.GetString("StartMorphAnalysisButtonComponent.StopText");

        #endregion

        #region Private Methods

        private async void OnPropertyChangedHandlerAsync(object sender, PropertyChangedEventArgs e)
        {
            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
