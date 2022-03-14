// <copyright file="GetEntryIdTableComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;

namespace PMA.Blazor.Components.GetEntryId
{
    public partial class GetEntryIdTableComponent
    {
        #region Parameters

        [Parameter] public IGetEntryIdViewModel ViewModel { get; set; }

        #endregion

        #region Private Properties / Fields

        private string _searchString;

        private int[] _pageSizeOptions = { 10, 20, 50, 100 };

        private string RowsPerPageString => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.RowsPerPageString");

        private string InfoFormat => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.InfoFormat");

        private string SortLabel => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.SortLabel");

        private string IdColumnHeader => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.IdColumnHeader");

        private string EntryColumnHeader => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.EntryColumnHeader");

        private string ParameterColumnHeader => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.ParameterColumnHeader");

        private string BaseColumnHeader => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.BaseColumnHeader");

        private string LeftColumnHeader => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.LeftColumnHeader");

        private string RightColumnHeader => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.RightColumnHeader");

        private string IsVirtualColumnHeader => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.IsVirtualColumnHeader");

        private string NoRecords => Resources.Strings.ResourceManager.GetString("GetEntryIdTableComponent.NoRecords");

        private IGetEntryIdControlViewModel _selectedItem;

        private IGetEntryIdControlViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;

                _selectedItem = value;

                _selectedItem.SelectCommand.Execute(null);
            }
        }

        #endregion

        #region Private Methods

        private bool FilterBySearchString(IGetEntryIdControlViewModel controlViewModel)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
            {
                return true;
            }

            if (controlViewModel.Id.ToString().Contains(_searchString))
            {
                return true;
            }

            if (controlViewModel.Entry.Contains(_searchString))
            {
                return true;
            }

            if (controlViewModel.Parameters.Contains(_searchString))
            {
                return true;
            }

            if (controlViewModel.Base.Contains(_searchString))
            {
                return true;
            }

            return controlViewModel.Left.Contains(_searchString) || controlViewModel.Right.Contains(_searchString);
        }

        #endregion
    }
}
