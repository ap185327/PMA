// <copyright file="GcHelper.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using System;
using System.Globalization;

namespace PMA.Application.Helpers
{
    /// <summary>
    /// The helper for the GC.
    /// </summary>
    internal static class GcHelper
    {
        /// <summary>
        /// Retrieves the number of bytes currently thought to be allocated. A parameter indicates whether this method can wait a short interval before returning, to allow the system to collect garbage and finalize objects.
        /// </summary>
        /// <param name="forceFullCollection">true to indicate that this method can wait for garbage collection to occur before returning; otherwise, false.</param>
        /// <returns>A number that is the best available approximation of the number of bytes currently allocated in managed memory.</returns>
        public static (string sizeValue, Size sizeType) GetTotalMemory(bool forceFullCollection = false)
        {
            long memory = GC.GetTotalMemory(forceFullCollection);

            return memory switch
            {
                > 1073741824 => (Math.Round((double)memory / 1048576 / 1024, 1).ToString(CultureInfo.CurrentCulture),
                    Size.Gb),
                > 1048576 => (Math.Round((double)memory / 1024 / 1024, 1).ToString(CultureInfo.CurrentCulture),
                    Size.Mb),
                _ => memory > 1024
                    ? (Math.Round((double)memory / 1024, 1).ToString(CultureInfo.CurrentCulture), Size.Kb)
                    : (memory.ToString(CultureInfo.CurrentCulture), Size.Bytes)
            };
        }
    }
}
