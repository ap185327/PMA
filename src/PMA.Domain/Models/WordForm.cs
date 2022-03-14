// <copyright file="Term.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace PMA.Domain.Models
{
    /// <summary>
    /// The morphological wordform model.
    /// </summary>
    public class WordForm
    {
        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        public string Entry { get; init; }

        /// <summary>
        /// Gets or sets the collection of solutions.
        /// </summary>
        public List<Solution> Solutions { get; set; }
    }
}
