// <copyright file="IMorphRuleManager.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Managers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphRuleManager"/> interfacing class.
    /// </summary>
    public interface IMorphRuleManager
    {
        /// <summary>
        /// Gets and caches a collection of morphological rules by the label and morphological parameters.
        /// </summary>
        /// <param name="label">A rule label.</param>
        /// <param name="parameters">Morphological parameters</param>
        /// <returns>A collection of morphological rules</returns>
        IList<MorphRule> GetAndCacheRules(string label, byte[] parameters);

        /// <summary>
        /// Gets and caches a collection of sandhi matches by the entry and the morphological rule.
        /// </summary>
        /// <param name="entry">An entry.</param>
        /// <param name="morphRule">A morphological rule.</param>
        /// <returns>A collection of sandhi matches.</returns>
        IList<SandhiMatch> GetAndCacheSandhiMatches(string entry, MorphRule morphRule);

        /// <summary>
        /// Clears temporary data used by the service.
        /// </summary>
        void Clear();
    }
}
