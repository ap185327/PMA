// <copyright file="GetEntryIdContainerComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.ViewModels;

namespace PMA.Blazor.Components.GetEntryId
{
    public partial class GetEntryIdContainerComponent
    {
        #region Parameters

        [Parameter]
        public IGetEntryIdViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel == value) return;

                _viewModel = value;

                if (_viewModel is not null && _viewModel.MorphEntries.Count == 0) return;

                _isVisible = _viewModel is not null;
            }
        }

        #endregion

        #region Private Properties / Fields

        private IGetEntryIdViewModel _viewModel;

        private bool _isVisible;

        private string Title => string.Format(Resources.Strings.ResourceManager.GetString("GetEntryIdContainerComponent.Title")!, ViewModel.Entry);

        #endregion

        #region Private Methods

        private void OnCloseButtonCLick()
        {
            ViewModel.IsActive = false;
        }

        #endregion
    }
}
