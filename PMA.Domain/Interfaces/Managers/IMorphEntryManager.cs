// <copyright file="IMorphEntryManager.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Managers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphEntryManager"/> interfacing class.
    /// </summary>
    public interface IMorphEntryManager
    {
        /// <summary>
        /// Get morphological entry by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The morphological entry.</returns>
        MorphEntry GetValue(int id);

        /// <summary>
        /// Gets a collection of morphological entries by parameters.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">The morphological base.</param>
        /// <param name="isVirtual">The solution is virtual (doesn't exist in the live language) or not.</param>
        /// <param name="left">A left morphological entry.</param>
        /// <param name="right">A right morphological entry.</param>
        /// <returns>The collection of morphological entries.</returns>
        IList<MorphEntry> GetValues(string entry, byte[] parameters, MorphBase morphBase, bool? isVirtual, MorphEntry left = null, MorphEntry right = null);

        /// <summary>
        /// Gets and caches a collection of morphological entries by parameters.
        /// </summary>
        /// <param name="entry">An entry.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">A morphological base.</param>
        /// <param name="isVirtual">Whether a MorphEntry is virtual (doesn't exist in the live language) or not.</param>
        /// <returns>A collection of morphological entries.</returns>
        IList<MorphEntry> GetValuesAndCache(string entry, byte[] parameters, MorphBase morphBase, bool isVirtual);

        /// <summary>
        /// Clears temporary data used by the service.
        /// </summary>
        void Clear();
    }
}
