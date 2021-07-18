// <copyright file="IMorphPropertyInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Interactors.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphPropertyInteractor"/> interfacing class.
    /// </summary>
    public interface IMorphPropertyInteractor : IInteractor
    {
        /// <summary>
        /// Starts the morphological parsing.
        /// </summary>
        /// <param name="inputData">The morphological parser input port.</param>
        /// <returns>The operation result.</returns>
        OperationResult StartAnalysis(MorphParserInputPort inputData);

        /// <summary>
        /// Stops the morphological parsing.
        /// </summary>
        /// <returns>The operation result.</returns>
        OperationResult StopAnalysis();

        /// <summary>
        /// Tries to add a morphological entry to database.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <returns>The operation result.</returns>
        OperationResult<string> TryToAddMorphEntry(MorphEntry morphEntry);

        /// <summary>
        /// Tries to update the morphological entry in the database.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <returns>The operation result.</returns>
        OperationResult<UpdateMorphEntryOutputPort> TryToUpdateMorphEntry(MorphEntry morphEntry);

        /// <summary>
        /// Tries to delete the morphological entry from the database.
        /// </summary>
        /// <param name="id">The morphological entry ID.</param>
        /// <returns>The operation result.</returns>
        OperationResult<DeleteMorphEntryOutputPort> TryToDeleteMorphEntry(int id);

        /// <summary>
        /// Updates all morphological properties starting at the start property index.
        /// </summary>
        /// <param name="inputData">The update morphological property input port.</param>
        /// <returns>The operation result.</returns>
        OperationResult UpdateAllMorphProperties(UpdateMorphPropertyInputPort inputData);

        /// <summary>
        /// Gets a morphological parameter by ID.
        /// </summary>
        /// <param name="id">The morphological parameter ID.</param>
        /// <returns>The operation result.</returns>
        OperationResult<MorphParameter> GetMorphParameterById(int id);

        /// <summary>
        /// Gets a collection of term entries by IDs.
        /// </summary>
        /// <param name="ids">Term IDs.</param>
        /// <param name="useAltPropertyEntry">Use alternative property entry or not.</param>
        /// <returns>The operation result.</returns>
        OperationResult<IList<string>> GetTermEntriesByIds(IList<byte> ids, bool useAltPropertyEntry = false);

        /// <summary>
        /// Gets a collection of similar.
        /// </summary>
        /// <param name="entry">The original morphological entry.</param>
        /// <returns>The operation result.</returns>
        OperationResult<IList<int>> GetSimilarMorphEntryIds(MorphEntry entry);
    }
}
