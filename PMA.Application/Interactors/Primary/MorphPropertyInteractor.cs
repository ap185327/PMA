// <copyright file="MorphPropertyInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.Interactors.Primary
{
    /// <summary>
    /// The morphological property interactor class.
    /// </summary>
    public sealed class MorphPropertyInteractor : InteractorBase<MorphPropertyInteractor>, IMorphPropertyInteractor
    {
        /// <summary>
        /// The start morphological analysis use case.
        /// </summary>
        private readonly IStartMorphAnalysisUseCase _startMorphAnalysisUseCase;

        /// <summary>
        /// The update all morphological property use case.
        /// </summary>
        private readonly IUpdateAllMorphPropertyUseCase _updateAllMorphPropertyUseCase;

        /// <summary>
        /// The get morphological parameter by ID use case.
        /// </summary>
        private readonly IGetMorphParameterByIdUseCase _getMorphParameterByIdUseCase;

        /// <summary>
        /// The get term entries by IDs use case.
        /// </summary>
        private readonly IGetTermEntriesByIdsUseCase _getTermEntriesByIdsUseCase;

        /// <summary>
        /// The try to add morphological entry use case.
        /// </summary>
        private readonly ITryToAddMorphEntryUseCase _tryToAddMorphEntryUseCase;

        /// <summary>
        /// The get morphological control view models use case.
        /// </summary>
        private readonly IGetMorphPropertyControlViewModelsUseCase _getMorphPropertyControlViewModelsUseCase;

        /// <summary>
        /// The get morphological category view models use case.
        /// </summary>
        private readonly IGetMorphCategoryControlViewModelsUseCase _getMorphCategoryControlViewModelsUseCase;

        /// <summary>
        /// The try to update morphological entry use case.
        /// </summary>
        private readonly ITryToUpdateMorphEntryUseCase _tryToUpdateMorphEntryUseCase;

        /// <summary>
        /// The try to delete morphological entry use case.
        /// </summary>
        private readonly ITryToDeleteMorphEntryUseCase _tryToDeleteMorphEntryUseCase;

        /// <summary>
        /// The get entry ID view model use case.
        /// </summary>
        private readonly IGetEntryIdViewModelUseCase _getEntryIdViewModelUseCase;

        /// <summary>
        /// The extract morphological information from morphological parameters use case.
        /// </summary>
        private readonly IExtractMorphInfoFromMorphParametersUseCase _extractMorphInfoFromMorphParametersUseCase;

        /// <summary>
        /// The get similar morphological entry use case.
        /// </summary>
        private readonly IGetSimilarMorphEntryIdsUseCase _getSimilarMorphEntryIdsUseCase;

        /// <summary>
        /// Initializes a new instance of <see cref="MorphPropertyInteractor"/> class.
        /// </summary>
        /// <param name="startMorphAnalysisUseCase">The start morphological analysis use case.</param>
        /// <param name="updateAllMorphPropertyUseCase">The update all morphological property use case.</param>
        /// <param name="getMorphParameterByIdUseCase">The get morphological parameter by ID use case.</param>
        /// /// <param name="getTermEntriesByIdsUseCase">The get term entries by IDs use case.</param>
        /// <param name="tryToAddMorphEntryUseCase">The try to add morphological entry use case.</param>
        /// <param name="getMorphPropertyControlViewModelsUseCase">The get morphological control view models use case.</param>
        /// <param name="getMorphCategoryControlViewModelsUseCase">The get morphological category view models use case.</param>
        /// <param name="tryToUpdateMorphEntryUseCase">The try to update morphological entry use case.</param>
        /// <param name="tryToDeleteMorphEntryUseCase">The try to delete morphological entry use case.</param>
        /// <param name="getEntryIdViewModelUseCase">The get entry ID view model use case.</param>
        /// <param name="extractMorphInfoFromMorphParametersUseCase">The extract morphological information from morphological parameters use case.</param>
        /// <param name="getSimilarMorphEntryIdsUseCase">The get similar morphological entry use case.</param>
        /// <param name="logger">The logger.</param>
        public MorphPropertyInteractor(IStartMorphAnalysisUseCase startMorphAnalysisUseCase,
            IUpdateAllMorphPropertyUseCase updateAllMorphPropertyUseCase,
            IGetMorphParameterByIdUseCase getMorphParameterByIdUseCase,
            IGetTermEntriesByIdsUseCase getTermEntriesByIdsUseCase,
            ITryToAddMorphEntryUseCase tryToAddMorphEntryUseCase,
            IGetMorphPropertyControlViewModelsUseCase getMorphPropertyControlViewModelsUseCase,
            IGetMorphCategoryControlViewModelsUseCase getMorphCategoryControlViewModelsUseCase,
            ITryToUpdateMorphEntryUseCase tryToUpdateMorphEntryUseCase,
            ITryToDeleteMorphEntryUseCase tryToDeleteMorphEntryUseCase,
            IGetEntryIdViewModelUseCase getEntryIdViewModelUseCase,
            IExtractMorphInfoFromMorphParametersUseCase extractMorphInfoFromMorphParametersUseCase,
            IGetSimilarMorphEntryIdsUseCase getSimilarMorphEntryIdsUseCase,
            ILogger<MorphPropertyInteractor> logger) : base(logger)
        {
            _startMorphAnalysisUseCase = startMorphAnalysisUseCase;
            _updateAllMorphPropertyUseCase = updateAllMorphPropertyUseCase;
            _getMorphParameterByIdUseCase = getMorphParameterByIdUseCase;
            _getTermEntriesByIdsUseCase = getTermEntriesByIdsUseCase;
            _tryToAddMorphEntryUseCase = tryToAddMorphEntryUseCase;
            _getMorphPropertyControlViewModelsUseCase = getMorphPropertyControlViewModelsUseCase;
            _getMorphCategoryControlViewModelsUseCase = getMorphCategoryControlViewModelsUseCase;
            _tryToUpdateMorphEntryUseCase = tryToUpdateMorphEntryUseCase;
            _tryToDeleteMorphEntryUseCase = tryToDeleteMorphEntryUseCase;
            _getEntryIdViewModelUseCase = getEntryIdViewModelUseCase;
            _extractMorphInfoFromMorphParametersUseCase = extractMorphInfoFromMorphParametersUseCase;
            _getSimilarMorphEntryIdsUseCase = getSimilarMorphEntryIdsUseCase;
        }

        #region Implementation of IMorphPropertyInteractor

        /// <summary>
        /// Starts the morphological parsing.
        /// </summary>
        /// <param name="inputData">The morphological parser input port.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        public async Task<OperationResult> StartAnalysisAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _startMorphAnalysisUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Tries to add a morphological entry to database.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        public async Task<OperationResult<string>> TryToAddMorphEntryAsync(MorphEntry morphEntry, CancellationToken token = default)
        {
            return await _tryToAddMorphEntryUseCase.ExecuteAsync(morphEntry, token);
        }

        /// <summary>
        /// Tries to update the morphological entry in the database.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The update morphological entry output port.</returns>
        public async Task<OperationResult<UpdateMorphEntryOutputPort>> TryToUpdateMorphEntryAsync(MorphEntry morphEntry, CancellationToken token = default)
        {
            return await _tryToUpdateMorphEntryUseCase.ExecuteAsync(morphEntry, token);
        }

        /// <summary>
        /// Tries to delete the morphological entry from the database.
        /// </summary>
        /// <param name="id">The morphological entry ID.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The delete morphological entry output port.</returns>
        public async Task<OperationResult<DeleteMorphEntryOutputPort>> TryToDeleteMorphEntryAsync(int id, CancellationToken token = default)
        {
            return await _tryToDeleteMorphEntryUseCase.ExecuteAsync(id, token);
        }

        /// <summary>
        /// Gets a collection of morphological property view model controls.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of morphological property view model controls.</returns>
        public async Task<OperationResult<IList<IMorphPropertyControlViewModel>>> GetMorphPropertyControlViewModelsAsync(CancellationToken token = default)
        {
            return await _getMorphPropertyControlViewModelsUseCase.ExecuteAsync(token);
        }

        /// <summary>
        /// Gets a collection of morphological category view model controls.
        /// </summary>
        /// <param name="properties">The collection of morphological property view model controls.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of morphological category view model controls.</returns>
        public async Task<OperationResult<IList<IMorphCategoryControlViewModel>>> GetMorphCategoryControlViewModelsAsync(IList<IMorphPropertyControlViewModel> properties, CancellationToken token = default)
        {
            return await _getMorphCategoryControlViewModelsUseCase.ExecuteAsync(properties, token);
        }

        /// <summary>
        /// Updates all morphological properties starting at the start property index.
        /// </summary>
        /// <param name="inputData">The update morphological property input port.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        public async Task<OperationResult> UpdateAllMorphPropertiesAsync(UpdateMorphPropertyInputPort inputData, CancellationToken token = default)
        {
            return await _updateAllMorphPropertyUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Gets a morphological parameter by ID.
        /// </summary>
        /// <param name="id">The morphological parameter ID.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The morphological parameter.</returns>
        public async Task<OperationResult<MorphParameter>> GetMorphParameterAsync(int id, CancellationToken token = default)
        {
            return await _getMorphParameterByIdUseCase.ExecuteAsync(id, token);
        }

        /// <summary>
        /// Gets a collection of term entries by IDs.
        /// </summary>
        /// <param name="inputPort">The get term entries by IDs input port.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        public async Task<OperationResult<IList<string>>> GetTermEntriesByIdsAsync(GetTermEntriesByIdsInputPort inputPort, CancellationToken token = default)
        {
            return await _getTermEntriesByIdsUseCase.ExecuteAsync(inputPort, token);
        }

        /// <summary>
        /// Gets a get entry ID view model.
        /// </summary>
        /// <param name="entry">The morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The get entry ID view model.</returns>
        public async Task<OperationResult<IGetEntryIdViewModel>> GetGetEntryIdViewModelAsync(MorphEntry entry, CancellationToken token = default)
        {
            return await _getEntryIdViewModelUseCase.ExecuteAsync(entry, token);
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
        /// Gets a collection of similar morphological entries.
        /// </summary>
        /// <param name="entry">The original morphological entry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of similar morphological entries.</returns>
        public async Task<OperationResult<IList<int>>> GetSimilarMorphEntryIdsAsync(MorphEntry entry, CancellationToken token = default)
        {
            return await _getSimilarMorphEntryIdsUseCase.ExecuteAsync(entry, token);
        }

        #endregion
    }
}
