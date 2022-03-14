// <copyright file="IMorphSolutionViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Base;
using PMA.Domain.Interfaces.ViewModels.Controls;

namespace PMA.Domain.Interfaces.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphSolutionViewModel"/> interfacing class.
    /// </summary>
    public interface IMorphSolutionViewModel : IViewModel
    {
        /// <summary>
        /// Gets a main wordform node view model.
        /// </summary>
        IWordFormTreeNodeViewModel MainTreeNode { get; }
    }
}
