// <copyright file="GetEntryIdInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.Interactors.Primary
{
    /// <summary>
    /// The get entry ID interactor class.
    /// </summary>
    public sealed class GetEntryIdInteractor : InteractorBase<GetEntryIdInteractor>, IGetEntryIdInteractor
    {
        /// <summary>
        /// The try to delete morphological entries use case.
        /// </summary>
        private readonly ITryToDeleteMorphEntriesUseCase _tryToDeleteMorphEntriesUseCase;

        /// <summary>
        /// The extract morphological information from morphological parameters use case.
        /// </summary>
        private readonly IExtractMorphInfoFromMorphParametersUseCase _extractMorphInfoFromMorphParametersUseCase;

        /// <summary>
        /// The get entry ID control view models use case.
        /// </summary>
        private readonly IGetEntryIdControlViewModelsUseCase _getEntryIdControlViewModelsUseCase;

        /// <summary>
        /// Initializes a new instance of <see cref="GetEntryIdInteractor"/> class.
        /// </summary>
        /// <param name="tryToDeleteMorphEntriesUseCase">The try to delete morphological entries use case.</param>
        /// <param name="extractMorphInfoFromMorphParametersUseCase">The extract morphological information from morphological parameters use case.</param>
        /// <param name="getEntryIdControlViewModelsUseCase">The get entry ID control view models use case.</param>
        /// <param name="logger">The logger.</param>
        public GetEntryIdInteractor(ITryToDeleteMorphEntriesUseCase tryToDeleteMorphEntriesUseCase,
            IExtractMorphInfoFromMorphParametersUseCase extractMorphInfoFromMorphParametersUseCase,
            IGetEntryIdControlViewModelsUseCase getEntryIdControlViewModelsUseCase,
            ILogger<GetEntryIdInteractor> logger) : base(logger)
        {
            _tryToDeleteMorphEntriesUseCase = tryToDeleteMorphEntriesUseCase;
            _extractMorphInfoFromMorphParametersUseCase = extractMorphInfoFromMorphParametersUseCase;
            _getEntryIdControlViewModelsUseCase = getEntryIdControlViewModelsUseCase;
        }

        #region Implementation of IGetEntryIdInteractor

        /// <summary>
        /// Tries to delete a collection of morphological entries from the database.
        /// </summary>
        /// <param name="ids">The collection of morphological entry IDs.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of delete morphological entry output ports.</returns>
        public async Task<OperationResult<IList<DeleteMorphEntryOutputPort>>> TryToDeleteMorphEntriesAsync(IList<int> ids, CancellationToken token = default)
        {
            return await _tryToDeleteMorphEntriesUseCase.ExecuteAsync(ids, token);
        }

        /// <summary>
        /// Extracts a morphological information from the collection of morphological parameters.
        /// </summary>
        /// <param name="inputPort">The extract a morphological information input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        public async Task<OperationResult<string>> ExtractMorphInfoFromMorphParametersAsync(ExtractMorphInfoInputPort inputPort, CancellationToken token = default)
        {
            return await _extractMorphInfoFromMorphParametersUseCase.ExecuteAsync(inputPort, token);
        }

        /// <summary>
        /// Gets a collection of get entry ID control view models by the morphological entry.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of get entry ID control view models.</returns>
        public async Task<OperationResult<IList<IGetEntryIdControlViewModel>>> GetGetEntryIdControlViewModelsAsync(MorphEntry morphEntry, CancellationToken token = default)
        {
            return await _getEntryIdControlViewModelsUseCase.ExecuteAsync(morphEntry, token);
        }

        #endregion
    }
}
