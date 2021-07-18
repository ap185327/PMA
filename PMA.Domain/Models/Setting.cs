// <copyright file="Setting.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Models
{
    /// <summary>
    /// The setting model.
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Gets or set a setting Name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets or set a setting value.
        /// </summary>
        public string Value { get; init; }
    }
}
