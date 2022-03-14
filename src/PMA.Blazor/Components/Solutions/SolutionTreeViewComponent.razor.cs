// <copyright file="SolutionTreeViewComponent.razor" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Interfaces.ViewModels.Controls.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMA.Blazor.Components.Solutions
{
    public partial class SolutionTreeViewComponent
    {
        #region Parameters

        [Parameter]
        public ITreeNodeViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel == value) return;

                _viewModel = value;

                TreeItems = new HashSet<ITreeNodeViewModel> { _viewModel };
            }
        }

        [Parameter]
        public string SandhiLine
        {
            get => _sandhiLine;
            set
            {
                if (_sandhiLine == value) return;

                _sandhiLine = value;

                SandhiLineChanged.InvokeAsync(_sandhiLine);
            }
        }

        [Parameter] public EventCallback<string> SandhiLineChanged { get; set; }

        #endregion

        #region Private Properties / Fields

        private string _sandhiLine = string.Empty;

        private ITreeNodeViewModel _selectedValue;

        private ITreeNodeViewModel SelectedValue
        {
            get => _selectedValue;
            set
            {
                if (_selectedValue == value) return;

                _selectedValue = value;

                SandhiLine = _selectedValue is null
                    ? string.Empty
                    : _selectedValue.SandhiLine;
            }
        }

        private ITreeNodeViewModel _viewModel;

        private HashSet<ITreeNodeViewModel> TreeItems { get; set; }

        #endregion

        #region Private Methods

        private Task<HashSet<ITreeNodeViewModel>> LoadServerData(ITreeNodeViewModel parentNode)
        {
            var treeItems = new HashSet<ITreeNodeViewModel>();

            switch (parentNode)
            {
                case IWordFormTreeNodeViewModel wordFormViewModel:
                    foreach (var solutionNode in wordFormViewModel.SolutionNodes)
                    {
                        treeItems.Add(solutionNode);
                    }

                    break;
                case ISolutionTreeNodeViewModel solutionViewModel:
                    if (solutionViewModel.LeftNode is not null)
                    {
                        treeItems.Add(solutionViewModel.LeftNode);
                    }

                    if (solutionViewModel.RightNode is not null)
                    {
                        treeItems.Add(solutionViewModel.RightNode);
                    }

                    break;
            }

            return Task.FromResult(treeItems);
        }

        #endregion
    }
}
