// <copyright file="Size.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of size.
    /// </summary>
    public enum Size
    {
        /// <summary>
        /// Size in bytes.
        /// </summary>
        Bytes = 0,

        /// <summary>
        /// Size in KB.
        /// </summary>
        Kb,

        /// <summary>
        /// Size in MB.
        /// </summary>
        Mb,

        /// <summary>
        /// Size in GB.
        /// </summary>
        Gb
    }
}
