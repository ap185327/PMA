// <copyright file="SolutionCollectionExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Application.Extensions
{
    /// <summary>
    /// Defines method extensions for solution collection.
    /// </summary>
    internal static class SolutionCollectionExtension
    {
        /// <summary>
        /// Determines if there is at least one successful solution or not.
        /// </summary>
        /// <param name="solutions">The collection of solutions.</param>
        /// <returns>Is there at least one successful solution or not.</returns>
        public static bool NoSuccess(this IEnumerable<Solution> solutions)
        {
            return solutions.All(x => x.Content.Error != SolutionError.Success);
        }
    }
}
