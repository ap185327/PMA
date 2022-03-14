// <copyright file="SandhiDirection.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Infrastructure.Enums
{
    /// <summary>
    /// Enumerates different types of sandhi directions.
    /// </summary>
    public enum SandhiDirection
    {
        /// <summary>
        /// Forward direction: AB + CD = ABCD.
        /// </summary>
        Forward,

        /// <summary>
        /// Reverse direction: ABCD = AB + CD.
        /// </summary>
        Reverse
    }
}
