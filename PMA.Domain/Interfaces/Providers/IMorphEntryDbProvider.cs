// <copyright file="IMorphEntryDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Providers.Base;
using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Providers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphParameterDbProvider"/> interfacing class.
    /// </summary>
    public interface IMorphEntryDbProvider : IDbProvider
    {
        /// <summary>
        /// Gets a collection of morphological entries.
        /// </summary>
        /// <returns>A collection of morphological entries.</returns>
        IList<MorphEntry> GetValues();

        /// <summary>
        /// Updates a morphological entry in the database.
        /// </summary>
        /// <param name="morphEntry">A morphological entry.</param>
        /// <param name="commit">Whether to save changes to the database after the operation.</param>
        void Update(MorphEntry morphEntry, bool commit = true);

        /// <summary>
        /// Inserts a morphological entry to the database.
        /// </summary>
        /// <param name="morphEntry">A morphological entry.</param>
        /// <param name="commit">Whether to save changes to the database after the operation.</param>
        void Insert(MorphEntry morphEntry, bool commit = true);

        /// <summary>
        /// Deletes a morphological entry from the database.
        /// </summary>
        /// <param name="morphEntry">A morphological entry.</param>
        /// <param name="commit">Whether to save changes to the database after the operation.</param>
        void Delete(MorphEntry morphEntry, bool commit = true);

        /// <summary>
        /// Saves database changes.
        /// </summary>
        void Commit();
    }
}
