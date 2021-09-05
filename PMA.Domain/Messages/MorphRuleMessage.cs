// <copyright file="MorphRuleMessage.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Messages
{
    /// <summary>
    /// The morphological rule message class.
    /// </summary>
    public class MorphRuleMessage
    {
        /// <summary>
        /// Gets or sets a collection of morphological rules.
        /// </summary>
        public IList<MorphRule> MorphRules { get; init; }

        /// <summary>
        /// Gets or sets a collection of sandhi matches.
        /// </summary>
        public IList<SandhiMatch> SandhiMatches { get; init; }
    }
}
