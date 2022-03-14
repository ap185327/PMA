// <copyright file="SolutionContent.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;

namespace PMA.Domain.Models
{
    /// <summary>
    /// A unique set of morphological parameters. Several solutions can have it.
    /// </summary>
    public class SolutionContent
    {
        /// <summary>
        /// Gets or sets the ID from the morphological dictionary. Returns 0 if solution isn't found in the morphological dictionary.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets or sets morphological parameters.
        /// </summary>
        public byte[] Parameters { get; init; }

        /// <summary>
        /// Gets or sets the morphological base. 
        /// </summary>
        public MorphBase Base { get; init; }

        /// <summary>
        /// Gets or sets whether the solution is virtual (doesn't exist in the live language) or not.
        /// </summary>
        public bool IsVirtual { get; init; }

        /// <summary>
        /// Gets or sets the solution error code.
        /// </summary>
        public SolutionError Error { get; set; }
    }
}
