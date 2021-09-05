// <copyright file="TreeNodeInteractor.cs" company="Andrey Pospelov">
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.Interactors.Primary
{
    /// <summary>
    /// The tree node interactor class.
    /// </summary>
    public sealed class TreeNodeInteractor : InteractorBase<TreeNodeInteractor>, ITreeNodeInteractor
    {
        /// <summary>
        /// The get wordform tree node use case.
        /// </summary>
        private readonly IGetWordFormTreeNodeUseCase _getWordFormTreeNodeUseCase;

        /// <summary>
        /// The extract morphological information from morphological parameters use case.
        /// </summary>
        private readonly IExtractMorphInfoFromMorphParametersUseCase _extractMorphInfoFromMorphParametersUseCase;

        /// <summary>
        /// The get solution tree nodes use case.
        /// </summary>
        private readonly IGetSolutionTreeNodesUseCase _getSolutionTreeNodesUseCase;

        /// <summary>
        /// The get layer for the first node use case.
        /// </summary>
        private readonly IGetLayerForFirstNodeUseCase _getLayerForFirstNodeUseCase;

        /// <summary>
        /// Initializes a new instance of <see cref="TreeNodeInteractor"/> class.
        /// </summary>
        /// <param name="getWordFormTreeNodeUseCase">The get wordform tree node use case.</param>
        /// <param name="getSolutionTreeNodesUseCase">The get solution tree nodes use case.</param>
        /// <param name="getLayerForFirstNodeUseCase">The get layer for the first node use case.</param>
        /// <param name="extractMorphInfoFromMorphParametersUseCase">The extract morphological information from morphological parameters use case.</param>
        /// <param name="logger">The logger.</param>
        public TreeNodeInteractor(IGetWordFormTreeNodeUseCase getWordFormTreeNodeUseCase,
            IGetSolutionTreeNodesUseCase getSolutionTreeNodesUseCase,
            IGetLayerForFirstNodeUseCase getLayerForFirstNodeUseCase,
            IExtractMorphInfoFromMorphParametersUseCase extractMorphInfoFromMorphParametersUseCase,
            ILogger<TreeNodeInteractor> logger) : base(logger)
        {
            _getWordFormTreeNodeUseCase = getWordFormTreeNodeUseCase;
            _getSolutionTreeNodesUseCase = getSolutionTreeNodesUseCase;
            _getLayerForFirstNodeUseCase = getLayerForFirstNodeUseCase;
            _extractMorphInfoFromMorphParametersUseCase = extractMorphInfoFromMorphParametersUseCase;
        }

        #region Implementation of ITreeNodeInteractor

        /// <summary>
        /// Gets a wordform tree node view model.
        /// </summary>
        /// <param name="inputPort">The get wordform tree node input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The wordform tree node view model.</returns>
        public async Task<OperationResult<IWordFormTreeNodeViewModel>> GetWordFormTreeNodeAsync(GetWordFormTreeNodeInputPort inputPort, CancellationToken token = default)
        {
            return await _getWordFormTreeNodeUseCase.ExecuteAsync(inputPort, token);
        }

        /// <summary>
        /// Gets a collection of solution tree node view models.
        /// </summary>
        /// <param name="groups">The solution tree node group input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The a collection of solution tree node view models.</returns>
        public async Task<OperationResult<IList<ISolutionTreeNodeViewModel>>> GetSolutionTreeNodesAsync(IList<SolutionTreeNodeViewModelGroup> groups, CancellationToken token = default)
        {
            return await _getSolutionTreeNodesUseCase.ExecuteAsync(groups, token);
        }

        /// <summary>
        /// Gets a chronological layer for the first tree node.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The chronological layer for the first tree node.</returns>
        public async Task<OperationResult<uint>> GetLayerForFirstNodeAsync(CancellationToken token = default)
        {
            return await _getLayerForFirstNodeUseCase.ExecuteAsync(token);
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

        #endregion
    }
}
