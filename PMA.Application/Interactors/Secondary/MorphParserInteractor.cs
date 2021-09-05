// <copyright file="MorphParserInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Secondary;
using PMA.Domain.Interfaces.UseCases.Secondary;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.Interactors.Secondary
{
    /// <summary>
    /// The morphological parser interactor class.
    /// </summary>
    public sealed class MorphParserInteractor : InteractorBase<MorphParserInteractor>, IMorphParserInteractor
    {
        /// <summary>
        /// The collapse solution use case.
        /// </summary>
        private readonly ICollapseSolutionUseCase _collapseSolutionUseCase;

        /// <summary>
        /// The parse morphological entry use case.
        /// </summary>
        private readonly IParseMorphEntryUseCase _parseMorphEntryUseCase;

        /// <summary>
        /// The remove duplicate use case.
        /// </summary>
        private readonly IRemoveDuplicateUseCase _removeDuplicateUseCase;

        /// <summary>
        /// The remove solution with excessive depth use case.
        /// </summary>
        private readonly IRemoveSolutionWithExcessiveDepthUseCase _removeSolutionWithExcessiveDepthUseCase;

        /// <summary>
        /// The remove unsuitable derivative solution use case.
        /// </summary>
        private readonly IRemoveUnsuitableDerivativeSolutionUseCase _removeUnsuitableDerivativeSolutionUseCase;

        /// <summary>
        /// The remove unsuitable solution use case.
        /// </summary>
        private readonly IRemoveUnsuitableSolutionUseCase _removeUnsuitableSolutionUseCase;

        /// <summary>
        /// The sort solution use case.
        /// </summary>
        private readonly ISortSolutionUseCase _sortSolutionUseCase;

        /// <summary>
        /// The update solution use case.
        /// </summary>
        private readonly IUpdateSolutionUseCase _updateSolutionUseCase;

        /// <summary>
        /// The validate solution use case.
        /// </summary>
        private readonly IValidateSolutionUseCase _validateSolutionUseCase;

        /// <summary>
        /// The clear cache use case.
        /// </summary>
        private readonly IClearCacheUseCase _clearCacheUseCase;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphParserInteractor"/> class.
        /// </summary>
        /// <param name="collapseSolutionUseCase">The collapse solution use case.</param>
        /// <param name="parseMorphEntryUseCase">The parse morphological entry use case.</param>
        /// <param name="removeDuplicateUseCase">The remove duplicate use case.</param>
        /// <param name="removeSolutionWithExcessiveDepthUseCase">The remove solution with excessive depth use case.</param>
        /// <param name="removeUnsuitableDerivativeSolutionUseCase">The remove unsuitable derivative solution use case.</param>
        /// <param name="removeUnsuitableSolutionUseCase">The remove unsuitable solution use case.</param>
        /// <param name="sortSolutionUseCase">The sort solution use case.</param>
        /// <param name="updateSolutionUseCase">The update solution use case.</param>
        /// <param name="validateSolutionUseCase">The validate solution use case.</param>
        /// <param name="clearCacheUseCase">The clear cache use case.</param>
        /// <param name="logger">The logger.</param>
        public MorphParserInteractor(ICollapseSolutionUseCase collapseSolutionUseCase,
            IParseMorphEntryUseCase parseMorphEntryUseCase,
            IRemoveDuplicateUseCase removeDuplicateUseCase,
            IRemoveSolutionWithExcessiveDepthUseCase removeSolutionWithExcessiveDepthUseCase,
            IRemoveUnsuitableDerivativeSolutionUseCase removeUnsuitableDerivativeSolutionUseCase,
            IRemoveUnsuitableSolutionUseCase removeUnsuitableSolutionUseCase,
            ISortSolutionUseCase sortSolutionUseCase,
            IUpdateSolutionUseCase updateSolutionUseCase,
            IValidateSolutionUseCase validateSolutionUseCase,
            IClearCacheUseCase clearCacheUseCase,
            ILogger<MorphParserInteractor> logger) : base(logger)
        {
            _collapseSolutionUseCase = collapseSolutionUseCase;
            _parseMorphEntryUseCase = parseMorphEntryUseCase;
            _removeDuplicateUseCase = removeDuplicateUseCase;
            _removeSolutionWithExcessiveDepthUseCase = removeSolutionWithExcessiveDepthUseCase;
            _removeUnsuitableDerivativeSolutionUseCase = removeUnsuitableDerivativeSolutionUseCase;
            _removeUnsuitableSolutionUseCase = removeUnsuitableSolutionUseCase;
            _sortSolutionUseCase = sortSolutionUseCase;
            _updateSolutionUseCase = updateSolutionUseCase;
            _validateSolutionUseCase = validateSolutionUseCase;
            _clearCacheUseCase = clearCacheUseCase;
        }

        #region Implementation of IMorphParserInteractor

        /// <summary>
        /// Parses morphological entry.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> ParseMorphEntryAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _parseMorphEntryUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Removes solutions with excessive depth.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> RemoveSolutionsWithExcessiveDepthAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _removeSolutionWithExcessiveDepthUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Collapses solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> CollapseSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _collapseSolutionUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Removes unsuitable derivative solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> RemoveUnsuitableDerivativeSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _removeUnsuitableDerivativeSolutionUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Updates solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> UpdateSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _updateSolutionUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Removes duplicates.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> RemoveDuplicatesAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _removeDuplicateUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Removes unsuitable solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> RemoveUnsuitableSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _removeUnsuitableSolutionUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Sort solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> SortSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _sortSolutionUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Validates solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> ValidateSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _validateSolutionUseCase.ExecuteAsync(inputData, token);
        }

        /// <summary>
        /// Clears manager cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        public async Task<OperationResult> ClearCacheAsync(CancellationToken token = default)
        {
            return await _clearCacheUseCase.ExecuteAsync(token);
        }

        #endregion
    }
}
