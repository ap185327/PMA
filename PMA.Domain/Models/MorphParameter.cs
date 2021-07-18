// <copyright file="MorphParameter.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Models
{
    /// <summary>
    /// Class for the MorphParameter model.
    /// </summary>
    public class MorphParameter
    {
        /// <summary>
        /// Gets or sets a parameter ID.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Gets or sets a parameter name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets or sets a parameter category.
        /// </summary>
        public string Category { get; init; }

        /// <summary>
        /// Gets or sets a parameter description.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets or sets whether an alternative property name is used.
        /// </summary>
        public bool UseAltPropertyEntry { get; init; }

        /// <summary>
        /// Gets or sets whether a parameter is visible or not in the Morphological solutions view.
        /// </summary>
        public bool IsVisible { get; set; }
    }
}
