// <copyright file="TranslateHelper.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.WinForms.Helpers
{
    /// <summary>
    /// The translate helper class.
    /// </summary>
    public static class TranslateHelper
    {
        /// <summary>
        /// Converts a property category name.
        /// </summary>
        /// <param name="text">The category name.</param>
        /// <returns>The converted category name.</returns>
        public static string ConvertCategoryName(string text)
        {
            string newText = Properties.Resources.ResourceManager.GetString($"MorphParameters.Category.{text}");

            return newText ?? text;
        }

        /// <summary>
        /// Converts a property name.
        /// </summary>
        /// <param name="text">The property name.</param>
        /// <returns>The converted property name.</returns>
        public static string ConvertPropertyName(string text)
        {
            string newText = Properties.Resources.ResourceManager.GetString($"MorphParameters.Property.{text}");

            return newText ?? text;
        }
    }
}
