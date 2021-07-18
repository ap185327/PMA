// <copyright file="IMorphRuleDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Providers.Base;
using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Providers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphRuleDbProvider"/> interfacing class.
    /// </summary>
    public interface IMorphRuleDbProvider : IDbProvider
    {
        /// <summary>
        /// Gets a collection of morphological combinations.
        /// </summary>
        /// <returns>A collection of combinations.</returns>
        IList<MorphRule> GetValues();

        /// <summary>
        /// Reloads all data.
        /// </summary>
        void Reload();
    }
}
