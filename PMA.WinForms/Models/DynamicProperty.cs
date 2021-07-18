// <copyright file="DynamicProperty.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;

namespace PMA.WinForms.Models
{
    /// <summary>
    /// Initializes a new instance of the DynamicProperty model.
    /// </summary>
    internal class DynamicProperty
    {
        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is read-only.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the property category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the property description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of property values.
        /// </summary>
        public string[] Values { get; set; }
    }
}
