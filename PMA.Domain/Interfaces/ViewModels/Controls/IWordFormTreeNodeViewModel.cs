// <copyright file="IWordFormTreeNodeViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Controls.Base;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.ViewModels.Controls
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IWordFormTreeNodeViewModel"/> interfacing class.
    /// </summary>
    public interface IWordFormTreeNodeViewModel : ITreeNodeViewModel
    {
        /// <summary>
        /// Gets a collection of solution node view models.
        /// </summary>
        IList<ISolutionTreeNodeViewModel> SolutionNodes { get; }
    }
}
