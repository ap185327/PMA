// <copyright file="SolutionTreeNodeViewModelGroup.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.ViewModels.Controls;

namespace PMA.Domain.Models
{
    /// <summary>
    /// Class for the solution tree node view model group.
    /// </summary>
    public class SolutionTreeNodeViewModelGroup
    {
        /// <summary>
        /// Gets or initializes a parent tree node.
        /// </summary>
        public IWordFormTreeNodeViewModel Parent { get; init; }

        /// <summary>
        /// Gets or initializes a solution.
        /// </summary>
        public Solution Solution { get; init; }
    }
}
