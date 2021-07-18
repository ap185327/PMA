// <copyright file="ISolutionTreeNodeViewModel.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Interfaces.ViewModels.Controls.Base;

namespace PMA.Domain.Interfaces.ViewModels.Controls
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ISolutionTreeNodeViewModel"/> interfacing class.
    /// </summary>
    public interface ISolutionTreeNodeViewModel : ITreeNodeViewModel
    {
        /// <summary>
        /// Gets whether the solution is taken from dictionary or not.
        /// </summary>
        bool IsFromDict { get; }

        /// <summary>
        /// Gets the solution relative rating: 0 - minimum, 1 - maximum.
        /// </summary>
        double RelativeRating { get; }

        /// <summary>
        /// Gets whether the solution is virtual (doesn't exist in the live language) or not.
        /// </summary>
        bool IsVirtual { get; }

        /// <summary>
        /// Gets a solution error.
        /// </summary>
        SolutionError Error { get; }

        /// <summary>
        /// Gets the left wordform node view model in the solution.
        /// </summary>
        IWordFormTreeNodeViewModel LeftNode { get; }

        /// <summary>
        /// Gets the right wordform node view model in the solution.
        /// </summary>
        IWordFormTreeNodeViewModel RightNode { get; }
    }
}
