// <copyright file="IGetEntryIdInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Interactors.Base;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Interactors.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IGetEntryIdInteractor"/> interfacing class.
    /// </summary>
    public interface IGetEntryIdInteractor : IInteractor
    {
        /// <summary>
        /// Tries to delete a collection of morphological entries from the database.
        /// </summary>
        /// <param name="ids">The collection of morphological entry IDs.</param>
        /// <returns>The operation result.</returns>
        OperationResult<IList<DeleteMorphEntryOutputPort>> TryToDeleteMorphEntries(IList<int> ids);

        /// <summary>
        /// Extracts a morphological information from the collection of morphological parameters.
        /// </summary>
        /// <param name="parameters">The collection of morphological parameters.</param>
        /// <returns>The operation result.</returns>
        OperationResult<string> ExtractMorphInfoFromMorphParameters(byte[] parameters);

        /// <summary>
        /// Gets a collection of morphological entries by the morphological entry.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <returns>The operation result.</returns>
        OperationResult<IList<MorphEntry>> GetMorphEntriesByMorphEntry(MorphEntry morphEntry);
    }
}
