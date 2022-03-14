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
        /// The None term ID.
        /// </summary>
        public const byte NoneTermId = 1;

        /// <summary>
        /// The alternative unknown term ID.
        /// </summary>
        public const byte AlternativeUnknownTermId = 255;

        /// <summary>
        /// The auto chronological layer.
        /// </summary>
        public const uint AutoLayer = 0;

        /// <summary>
        /// The default chronological layer.
        /// </summary>
        public const int DefaultLayer = 6;
    }
}
