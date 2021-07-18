// <copyright file="SolutionGroup.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Models
{
    /// <summary>
    /// Class for the solution group model.
    /// </summary>
    public class SolutionGroup
    {
        /// <summary>
        /// Gets or sets a solution parent.
        /// </summary>
        public WordForm Parent { get; init; }

        /// <summary>
        /// Gets or sets a solution.
        /// </summary>
        public Solution Solution { get; init; }
    }
}
