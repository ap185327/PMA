// <copyright file="IGetEntryIdInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of delete morphological entry output ports.</returns>
        Task<OperationResult<IList<DeleteMorphEntryOutputPort>>> TryToDeleteMorphEntriesAsync(IList<int> ids, CancellationToken token = default);

        /// <summary>
        /// Extracts a morphological information from the collection of morphological parameters.
        /// </summary>
        /// <param name="inputPort">The extract a morphological information input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        Task<OperationResult<string>> ExtractMorphInfoFromMorphParametersAsync(ExtractMorphInfoInputPort inputPort, CancellationToken token = default);

        /// <summary>
        /// Gets a collection of get entry ID control view models by the morphological entry.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of get entry ID control view models.</returns>
        Task<OperationResult<IList<IGetEntryIdControlViewModel>>> GetGetEntryIdControlViewModelsAsync(MorphEntry morphEntry, CancellationToken token = default);
    }
}
