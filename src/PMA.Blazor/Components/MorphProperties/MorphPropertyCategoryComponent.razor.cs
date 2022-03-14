// <copyright file="MorphPropertyCategoryComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System;
using System.ComponentModel;

namespace PMA.Blazor.Components.MorphProperties
{
    public partial class MorphPropertyCategoryComponent : IDisposable
    {
        #region Parameters

        [CascadingParameter(Name = "Theme")] private MudTheme Theme { get; set; }

        [Parameter]
        public IMorphCategoryControlViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel == value) return;

                if (_viewModel is not null)
                {
                    _viewModel.PropertyChanged -= OnPropertyChangedHandler;
                }

                _viewModel = value;

                if (_viewModel is null) return;

                _viewModel.PropertyChanged += OnPropertyChangedHandler;
            }
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ViewModel = null;
        }

        #endregion

        #region Private Properties / Fields

        private IMorphCategoryControlViewModel _viewModel;

        private string Name => ViewModel.Name;

        private string Terms => ViewModel.Terms;

        private string Style => $"background: {Theme.Palette.Background}";

        #endregion

        #region Private Methods

        private async void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IMorphCategoryControlViewModel.Terms)) return;

            await InvokeAsync(StateHasChanged);
        }

        #endregion
    }
}
