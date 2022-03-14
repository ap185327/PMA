// <copyright file="ColorExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Drawing;

namespace PMA.Utils.Extensions
{
    /// <summary>
    /// Defines method extensions for color.
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// Converts the color value to hexadecimal format.
        /// </summary>
        /// <param name="color">This color.</param>
        /// <returns>The formatted hexadecimal value.</returns>
        public static string ToHex(this Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        /// Converts the color value to RGBA format.
        /// </summary>
        /// <param name="color">This color.</param>
        /// <param name="opacity">The opacity.</param>
        /// <returns></returns>
        public static string ToRgba(this Color color, double opacity = 1)
        {
            return $"rgba({color.R},{color.G},{color.B}, {Math.Round(opacity, 2)})";
        }
    }
}
