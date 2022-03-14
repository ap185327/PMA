// <copyright file="MorphParsingType.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of morphological parsing types.
    /// </summary>
    public enum MorphParsingType
    {
        /// <summary>
        /// All solutions, including unsuccessful ones.
        /// </summary>
        Debug,

        /// <summary>
        /// Only successful solutions sorted by rating.
        /// </summary>
        Release,

        /// <summary>
        /// Only successful solutions without sorting.
        /// </summary>
        Import
    }
}
