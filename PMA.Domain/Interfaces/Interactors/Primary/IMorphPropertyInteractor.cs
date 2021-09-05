// <copyright file="IMorphPropertyInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        Task<OperationResult> StartAnalysisAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Tries to add a morphological entry to database.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        Task<OperationResult<string>> TryToAddMorphEntryAsync(MorphEntry morphEntry, CancellationToken token = default);

        /// <summary>
        /// Tries to update the morphological entry in the database.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The update morphological entry output port.</returns>
        Task<OperationResult<UpdateMorphEntryOutputPort>> TryToUpdateMorphEntryAsync(MorphEntry morphEntry, CancellationToken token = default);

        /// <summary>
        /// Tries to delete the morphological entry from the database.
        /// </summary>
        /// <param name="id">The morphological entry ID.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The delete morphological entry output port.</returns>
        Task<OperationResult<DeleteMorphEntryOutputPort>> TryToDeleteMorphEntryAsync(int id, CancellationToken token = default);

        /// <summary>
        /// Gets a collection of morphological property view model controls.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of morphological property view model controls.</returns>
        Task<OperationResult<IList<IMorphPropertyControlViewModel>>> GetMorphPropertyControlViewModelsAsync(CancellationToken token = default);

        /// <summary>
        /// Gets a collection of morphological category view model controls.
        /// </summary>
        /// <param name="properties">The collection of morphological property view model controls.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of morphological category view model controls.</returns>
        Task<OperationResult<IList<IMorphCategoryControlViewModel>>> GetMorphCategoryControlViewModelsAsync(IList<IMorphPropertyControlViewModel> properties, CancellationToken token = default);

        /// <summary>
        /// Updates all morphological properties starting at the start property index.
        /// </summary>
        /// <param name="inputData">The update morphological property input port.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        Task<OperationResult> UpdateAllMorphPropertiesAsync(UpdateMorphPropertyInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Gets a morphological parameter by ID.
        /// </summary>
        /// <param name="id">The morphological parameter ID.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The morphological parameter.</returns>
        Task<OperationResult<MorphParameter>> GetMorphParameterAsync(int id, CancellationToken token = default);

        /// <summary>
        /// Gets a collection of term entries by IDs.
        /// </summary>
        /// <param name="inputPort">The get term entries by IDs input port.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        Task<OperationResult<IList<string>>> GetTermEntriesByIdsAsync(GetTermEntriesByIdsInputPort inputPort, CancellationToken token = default);

        /// <summary>
        /// Gets a collection of similar morphological entries.
        /// </summary>
        /// <param name="entry">The original morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of similar morphological entries.</returns>
        Task<OperationResult<IList<int>>> GetSimilarMorphEntryIdsAsync(MorphEntry entry, CancellationToken token = default);

        /// <summary>
        /// Gets a get entry ID view model.
        /// </summary>
        /// <param name="entry">The morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The get entry ID view model.</returns>
        Task<OperationResult<IGetEntryIdViewModel>> GetGetEntryIdViewModelAsync(MorphEntry entry, CancellationToken token = default);

        /// <summary>
        /// Extracts a morphological information from the collection of morphological parameters.
        /// </summary>
        /// <param name="inputPort">The extract a morphological information input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        Task<OperationResult<string>> ExtractMorphInfoFromMorphParametersAsync(ExtractMorphInfoInputPort inputPort, CancellationToken token = default);
    }
}
