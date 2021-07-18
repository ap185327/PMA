// <copyright file="MorphBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of MorphBase.
    /// </summary>
    public enum MorphBase
    {
        /// <summary>
        /// Basis unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// No basis.
        /// </summary>
        None = 1,

        /// <summary>
        /// The basis is the left part.
        /// </summary>
        Left = 2,

        /// <summary>
        /// The basis is the right part.
        /// </summary>
        Right = 3,

        /// <summary>
        /// The basics are both left and right parts.
        /// </summary>
        Both = 4
    }
}
