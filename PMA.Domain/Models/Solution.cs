// <copyright file="Solution.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace PMA.Domain.Models
{
    /// <summary>
    /// The morphological solution model.
    /// </summary>
    public class Solution
    {
        /// <summary>
        /// Gets or sets the unique set of morphological parameters. Several Solutions can have it.
        /// </summary>
        public SolutionContent Content { get; set; }

        /// <summary>
        /// Gets or sets the left part of the wordform in the solution.
        /// </summary>
        public WordForm Left { get; set; }

        /// <summary>
        /// Gets or sets the right part of the wordform in the solution.
        /// </summary>
        public WordForm Right { get; set; }

        /// <summary>
        /// Gets or sets the original solution from the morphological dictionary if any.
        /// </summary>
        public Solution Original { get; set; }

        /// <summary>
        /// Gets or sets the collection of morphological rules.
        /// </summary>
        public List<MorphRule> Rules { get; set; }

        /// <summary>
        /// Gets or sets the collection of sandhi matches.
        /// </summary>
        public List<SandhiMatch> Sandhi { get; set; }

        /// <summary>
        /// Gets or sets the collapse solution rating.
        /// </summary>
        /// <remarks>Before rating calculation, this property accumulates the rating of collapsing solutions.</remarks>
        public double CollapseRating { get; set; } = 1;

        /// <summary>
        /// Gets or sets the solution rating: 0 - minimum, 2 - maximum.
        /// </summary>
        public double Rating { get; set; } = -1;
    }
}
