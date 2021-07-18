// <copyright file="MorphPropertyInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Models;
using PMA.Domain.OutputPorts;
using PMA.Utils.Extensions;
using System.Collections.Generic;

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
        /// The stop morphological analysis use case.
        /// </summary>
        private readonly IStopMorphAnalysisUseCase _stopMorphAnalysisUseCase;

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
        /// The try to update morphological entry use case.
        /// </summary>
        private readonly ITryToUpdateMorphEntryUseCase _tryToUpdateMorphEntryUseCase;

        /// <summary>
        /// The try to delete morphological entry use case.
        /// </summary>
        private readonly ITryToDeleteMorphEntryUseCase _tryToDeleteMorphEntryUseCase;

        /// <summary>
        /// The get similar morphological entry use case.
        /// </summary>
        private readonly IGetSimilarMorphEntryIdsUseCase _getSimilarMorphEntryIdsUseCase;

        /// <summary>
        /// Initializes a new instance of <see cref="InteractorBase{T}"/> class.
        /// </summary>
        /// <param name="startMorphAnalysisUseCase">The start morphological analysis use case.</param>
        /// <param name="stopMorphAnalysisUseCase">The stop morphological analysis use case.</param>
        /// <param name="updateAllMorphPropertyUseCase">The update all morphological property use case.</param>
        /// <param name="getMorphParameterByIdUseCase">The get morphological parameter by ID use case.</param>
        /// <param name="getTermEntriesByIdsUseCase">The get term entries by IDs use case.</param>
        /// <param name="tryToAddMorphEntryUseCase">The try to add morphological entry use case.</param>
        /// <param name="tryToUpdateMorphEntryUseCase">The try to update morphological entry use case.</param>
        /// <param name="tryToDeleteMorphEntryUseCase">The try to delete morphological entry use case.</param>
        /// <param name="getSimilarMorphEntryIdsUseCase">The get similar morphological entry use case.</param>
        /// <param name="logger">The logger.</param>
        public MorphPropertyInteractor(IStartMorphAnalysisUseCase startMorphAnalysisUseCase,
            IStopMorphAnalysisUseCase stopMorphAnalysisUseCase,
            IUpdateAllMorphPropertyUseCase updateAllMorphPropertyUseCase,
            IGetMorphParameterByIdUseCase getMorphParameterByIdUseCase,
            IGetTermEntriesByIdsUseCase getTermEntriesByIdsUseCase,
            ITryToAddMorphEntryUseCase tryToAddMorphEntryUseCase,
            ITryToUpdateMorphEntryUseCase tryToUpdateMorphEntryUseCase,
            ITryToDeleteMorphEntryUseCase tryToDeleteMorphEntryUseCase,
            IGetSimilarMorphEntryIdsUseCase getSimilarMorphEntryIdsUseCase,
            ILogger<MorphPropertyInteractor> logger) : base(logger)
        {
            _startMorphAnalysisUseCase = startMorphAnalysisUseCase;
            _stopMorphAnalysisUseCase = stopMorphAnalysisUseCase;
            _updateAllMorphPropertyUseCase = updateAllMorphPropertyUseCase;
            _getMorphParameterByIdUseCase = getMorphParameterByIdUseCase;
            _getTermEntriesByIdsUseCase = getTermEntriesByIdsUseCase;
            _tryToAddMorphEntryUseCase = tryToAddMorphEntryUseCase;
            _tryToUpdateMorphEntryUseCase = tryToUpdateMorphEntryUseCase;
            _tryToDeleteMorphEntryUseCase = tryToDeleteMorphEntryUseCase;
            _getSimilarMorphEntryIdsUseCase = getSimilarMorphEntryIdsUseCase;

            Logger.LogInit();
        }

        #region Implementation of IMorphPropertyViewModelInteractor

        /// <summary>
        /// Starts the morphological parsing.
        /// </summary>
        /// <param name="inputData">The morphological parser input data.</param>
        /// <returns>The operation result.</returns>
        public OperationResult StartAnalysis(MorphParserInputPort inputData)
        {
            var result = _startMorphAnalysisUseCase.Execute(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Stops the morphological parsing.
        /// </summary>
        /// <returns>The operation result.</returns>
        public OperationResult StopAnalysis()
        {
            var result = _stopMorphAnalysisUseCase.Execute();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Tries to add a morphological entry to database.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<string> TryToAddMorphEntry(MorphEntry morphEntry)
        {
            var result = _tryToAddMorphEntryUseCase.Execute(morphEntry);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Tries to update the morphological entry in the database.
        /// </summary>
        /// <param name="morphEntry">The morphological entry.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<UpdateMorphEntryOutputPort> TryToUpdateMorphEntry(MorphEntry morphEntry)
        {
            var result = _tryToUpdateMorphEntryUseCase.Execute(morphEntry);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Tries to delete the morphological entry from the database.
        /// </summary>
        /// <param name="id">The morphological entry ID.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<DeleteMorphEntryOutputPort> TryToDeleteMorphEntry(int id)
        {
            var result = _tryToDeleteMorphEntryUseCase.Execute(id);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Updates all morphological properties starting at the start property index.
        /// </summary>
        /// <param name="inputData">The update morphological property input data.</param>
        /// <returns>The operation result.</returns>
        public OperationResult UpdateAllMorphProperties(UpdateMorphPropertyInputPort inputData)
        {
            var result = _updateAllMorphPropertyUseCase.Execute(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Gets a morphological parameter by ID.
        /// </summary>
        /// <param name="id">The morphological parameter ID.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<MorphParameter> GetMorphParameterById(int id)
        {
            var result = _getMorphParameterByIdUseCase.Execute(id);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Gets a collection of term entries by IDs.
        /// </summary>
        /// <param name="ids">Term IDs.</param>
        /// <param name="useAltPropertyEntry">Use alternative property entry or not.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<IList<string>> GetTermEntriesByIds(IList<byte> ids, bool useAltPropertyEntry = false)
        {
            var inputPort = new GetTermEntriesByIdsInputPort
            {
                TermIds = ids,
                UseAltPropertyEntry = useAltPropertyEntry
            };

            var result = _getTermEntriesByIdsUseCase.Execute(inputPort);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Gets a collection of similar.
        /// </summary>
        /// <param name="entry">The original morphological entry.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<IList<int>> GetSimilarMorphEntryIds(MorphEntry entry)
        {
            var result = _getSimilarMorphEntryIdsUseCase.Execute(entry);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        #endregion
    }
}
