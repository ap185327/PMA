// <copyright file="MorphConstants.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Constants
{
    /// <summary>
    /// The morphological constant class.
    /// </summary>
    public static class MorphConstants
    {
        /// <summary>
        /// The number of morphological parameters.
        /// </summary>
        public const int ParameterCount = 21;

        /// <summary>
        /// The unknown term ID.
        /// </summary>
        public const byte UnknownTermId = 0;

        /// <summary>
        /// The alternative unknown term ID.
        /// </summary>
        public const byte AlternativeUnknownTermId = 255;
    }
}
