// <copyright file="MorphEntrySource.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of MorphEntrySource.
    /// </summary>
    public enum MorphEntrySource
    {
        /// <summary>
        /// Morphological entry is created by the user.
        /// </summary>
        User = 0,

        /// <summary>
        /// Morphological entry is created by the import process without analysis.
        /// </summary>
        ImportWithoutAnalysis = 1,

        /// <summary>
        /// Morphological entry is created by the import process with analysis.
        /// </summary>
        ImportWithAnalysis = 2
    }
}
