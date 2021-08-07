// <copyright file="MorphParserInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Secondary;
using PMA.Domain.Interfaces.UseCases.Secondary;

namespace PMA.Application.Interactors.Secondary
{
    /// <summary>
    /// The morphological parser interactor class.
    /// </summary>
    public sealed class MorphParserInteractor : IMorphParserInteractor
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
        public MorphParserInteractor(ICollapseSolutionUseCase collapseSolutionUseCase,
            IParseMorphEntryUseCase parseMorphEntryUseCase,
            IRemoveDuplicateUseCase removeDuplicateUseCase,
            IRemoveSolutionWithExcessiveDepthUseCase removeSolutionWithExcessiveDepthUseCase,
            IRemoveUnsuitableDerivativeSolutionUseCase removeUnsuitableDerivativeSolutionUseCase,
            IRemoveUnsuitableSolutionUseCase removeUnsuitableSolutionUseCase,
            ISortSolutionUseCase sortSolutionUseCase,
            IUpdateSolutionUseCase updateSolutionUseCase,
            IValidateSolutionUseCase validateSolutionUseCase,
            IClearCacheUseCase clearCacheUseCase)
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
        /// <returns>The result of operation.</returns>
        public OperationResult ParseMorphEntry(MorphParserInputPort inputData)
        {
            return _parseMorphEntryUseCase.Execute(inputData);
        }

        /// <summary>
        /// Removes solutions with excessive depth.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        public OperationResult RemoveSolutionsWithExcessiveDepth(MorphParserInputPort inputData)
        {
            return _removeSolutionWithExcessiveDepthUseCase.Execute(inputData);
        }

        /// <summary>
        /// Collapses solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        public OperationResult CollapseSolutions(MorphParserInputPort inputData)
        {
            return _collapseSolutionUseCase.Execute(inputData);
        }

        /// <summary>
        /// Removes unsuitable derivative solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        public OperationResult RemoveUnsuitableDerivativeSolutions(MorphParserInputPort inputData)
        {
            return _removeUnsuitableDerivativeSolutionUseCase.Execute(inputData);
        }

        /// <summary>
        /// Updates solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        public OperationResult UpdateSolutions(MorphParserInputPort inputData)
        {
            return _updateSolutionUseCase.Execute(inputData);
        }

        /// <summary>
        /// Removes duplicates.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        public OperationResult RemoveDuplicates(MorphParserInputPort inputData)
        {
            return _removeDuplicateUseCase.Execute(inputData);
        }

        /// <summary>
        /// Removes unsuitable solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        public OperationResult RemoveUnsuitableSolutions(MorphParserInputPort inputData)
        {
            return _removeUnsuitableSolutionUseCase.Execute(inputData);
        }

        /// <summary>
        /// Sort solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        public OperationResult SortSolutions(MorphParserInputPort inputData)
        {
            return _sortSolutionUseCase.Execute(inputData);
        }

        /// <summary>
        /// Validates solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        public OperationResult ValidateSolutions(MorphParserInputPort inputData)
        {
            return _validateSolutionUseCase.Execute(inputData);
        }

        /// <summary>
        /// Clears manager cache.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public OperationResult ClearCache()
        {
            return _clearCacheUseCase.Execute();
        }

        #endregion
    }
}
