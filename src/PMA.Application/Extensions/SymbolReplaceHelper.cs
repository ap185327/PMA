// <copyright file="SymbolReplaceHelper.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Application.Extensions
{
    /// <summary>
    /// Defines method extensions for symbol replace option.
    /// </summary>
    internal static class SymbolReplaceHelper
    {
        /// <summary>
        /// Replaces latin symbols to pali: aa -> ā, ii -> ī, etc.
        /// </summary>
        /// <param name="thisText">This text.</param>
        /// <returns>The text with pali symbols.</returns>
        public static string LatinToPali(this string thisText)
        {
            return string.IsNullOrEmpty(thisText)
                ? thisText
                : thisText
                    .Replace("aa", "ā")
                    .Replace("ii", "ī")
                    .Replace("uu", "ū")
                    .Replace("~n", "ñ")
                    .Replace(".n", "ṇ")
                    .Replace(".d", "ḍ")
                    .Replace(".l", "ḷ")
                    .Replace(".m", "ṃ")
                    .Replace(".t", "ṭ")
                    .Replace("\"n", "ṅ")
                    .Replace("ṁ", "ṃ")
                    .Replace("ŋ", "ṃ");
        }
    }
}
