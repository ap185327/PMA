// <copyright file="SolutionTreeNodeComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using MudBlazor;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Interfaces.ViewModels.Controls.Base;
using PMA.Utils.Extensions;

namespace PMA.Blazor.Components.Solutions
{
    public partial class SolutionTreeNodeComponent
    {
        #region Parameters

        [CascadingParameter(Name = "Theme")] private MudTheme Theme { get; set; }

        [Parameter] public ITreeNodeViewModel ViewModel { get; set; }

        #endregion

        #region Private Properties / Fields

        private MudTreeViewItem<ITreeNodeViewModel> _mudTreeViewItem;

        private bool IsFromDict => ViewModel is ISolutionTreeNodeViewModel { IsFromDict: true };

        private string ExpandedIcon => ViewModel.HasChildren
                ? Icons.Material.Filled.ChevronRight
                : string.Empty;

        private string Style
        {
            get
            {
                if (ViewModel is not ISolutionTreeNodeViewModel solutionViewModel)
                {
                    return $"color:{Theme.Palette.TextPrimary}";
                }

                // ReSharper disable once PossibleNullReferenceException
                if (solutionViewModel.Error != SolutionError.Success)
                {
                    return $"color:{Theme.Palette.TextDisabled}";
                }

                int red = (int)((1 - solutionViewModel.RelativeRating) * 244);
                int green = (int)(solutionViewModel.RelativeRating * 133 + 67);
                int blue = (int)(29 * solutionViewModel.RelativeRating + 54);
                string opacity = solutionViewModel.IsVirtual ? "0.5" : "1";

                return $"color:rgba({red},{green},{blue}, {opacity})";
            }
        }

        #endregion

        #region Private Methods

        private void OnItemExpanded(bool expanded)
        {
            _mudTreeViewItem.ExecuteNotPublicMethod("OnItemExpanded", expanded);
        }

        #endregion
    }
}
