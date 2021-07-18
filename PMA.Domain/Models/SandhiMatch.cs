// <copyright file="SandhiMatch.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace PMA.Domain.Models
{
    /// <summary>
    /// Class for the SandhiMatch model.
    /// </summary>
    public class SandhiMatch
    {
        /// <summary>
        /// Gets or sets a sandhi expression.
        /// </summary>
        public string SandhiExpression { get; init; }

        /// <summary>
        /// Gets or sets a collection of sandhi rules used to express the sandhi.
        /// </summary>
        public IList<SandhiRule> Rules { get; init; }
    }
}
