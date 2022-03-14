// <copyright file="IMorphCombinationManager.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Managers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphCombinationManager"/> interfacing class.
    /// </summary>
    public interface IMorphCombinationManager
    {
        /// <summary>
        /// Checks morphological parameters are valid or not.
        /// </summary>
        /// <param name="parameters">Morphological parameters.</param>
        /// <returns>A morphological parameters are valid or not.</returns>
        bool Check(byte[] parameters);

        /// <summary>
        /// Checks and caches morphological parameters are valid or not.
        /// </summary>
        /// <param name="parameters">Morphological parameters.</param>
        /// <returns>Morphological parameters are valid or not.</returns>
        bool CheckAndCache(byte[] parameters);

        /// <summary>
        /// Checks and caches morphological parameters are valid or not.
        /// </summary>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="collectiveParameters">Collective morphological parameters of matching combinations.</param>
        /// <returns>Morphological parameters are valid or not.</returns>
        bool CheckAndCache(byte[] parameters, out byte[] collectiveParameters);

        /// <summary>
        /// Gets a collection of valid morphological combinations.
        /// </summary>
        /// <param name="parameters">Morphological parameters.</param>
        /// <returns>A collection of valid morphological combinations.</returns>
        IList<byte[]> GetValidParameters(byte[] parameters);

        /// <summary>
        /// Clears temporary data used by the service.
        /// </summary>
        void Clear();
    }
}
