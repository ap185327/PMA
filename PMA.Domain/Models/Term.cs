// <copyright file="Term.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Models
{
    /// <summary>
    /// Class for the Term model.
    /// </summary>
    public class Term
    {
        /// <summary>
        /// Gets or sets a Term ID.
        /// </summary>
        public byte Id { get; init; }

        /// <summary>
        /// Gets or sets a term entry.
        /// </summary>
        public string Entry { get; init; }

        /// <summary>
        /// Gets or sets a term alternative entry.
        /// </summary>
        public string AltEntry { get; init; }

        /// <summary>
        /// Gets or sets a term alternative property entry.
        /// </summary>
        public string AltPropertyEntry { get; init; }
    }
}
