// <copyright file="WordFormFactory.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Application.Factories
{
    /// <summary>
    /// The wordform factory class.
    /// </summary>
    public static class WordFormFactory
    {
        /// <summary>
        /// Create a new instance of the <see cref="WordForm"/> class.
        /// </summary>
        /// <param name="entry">The wordform entry.</param>
        /// <returns>A new instance of the <see cref="WordForm"/> class.</returns>
        public static WordForm Create(string entry)
        {
            return new()
            {
                Entry = entry,
                Solutions = new List<Solution>()
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="WordForm"/> class.
        /// </summary>
        /// <param name="entry">The wordform entry.</param>
        /// <param name="solution">The solution.</param>
        /// <returns>A new instance of the <see cref="WordForm"/> class.</returns>
        public static WordForm Create(string entry, Solution solution)
        {
            return new()
            {
                Entry = entry,
                Solutions = new List<Solution> { solution }
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="WordForm"/> class.
        /// </summary>
        /// <param name="entry">The wordform entry.</param>
        /// <param name="solutions">The collection of solutions.</param>
        /// <returns>A new instance of the <see cref="WordForm"/> class.</returns>
        public static WordForm Create(string entry, List<Solution> solutions)
        {
            return new()
            {
                Entry = entry,
                Solutions = solutions
            };
        }

        /// <summary>
        /// Clones the <see cref="WordForm"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A new instance of the <see cref="WordForm"/> class.</returns>
        public static WordForm Clone(WordForm source)
        {
            return new()
            {
                Entry = source.Entry,
                Solutions = source.Solutions.Select(SolutionFactory.Clone).ToList()
            };
        }

        /// <summary>
        /// Сlones the <see cref="WordForm"/> class and replace solutions.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="newSolutions">The collection of new solutions.</param>
        /// <returns>A new instance of the <see cref="WordForm"/> class.</returns>
        public static WordForm Clone(WordForm source, List<Solution> newSolutions)
        {
            return new()
            {
                Entry = source.Entry,
                Solutions = newSolutions
            };
        }
    }
}
