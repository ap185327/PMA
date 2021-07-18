// <copyright file="IMorphCombinationDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Providers.Base;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Providers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphCombinationDbProvider"/> interfacing class.
    /// </summary>
    public interface IMorphCombinationDbProvider : IDbProvider
    {
        /// <summary>
        /// Gets a collection of morphological combinations.
        /// </summary>
        /// <returns>A collection of morphological combinations.</returns>
        IList<byte[]> GetValues();

        /// <summary>
        /// Reloads all data.
        /// </summary>
        void Reload();
    }
}
