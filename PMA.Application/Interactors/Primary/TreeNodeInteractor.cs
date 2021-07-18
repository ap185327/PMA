// <copyright file="TreeNodeInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Utils.Extensions;

namespace PMA.Application.Interactors.Primary
{
    /// <summary>
    /// The tree node interactor class.
    /// </summary>
    public sealed class TreeNodeInteractor : InteractorBase<TreeNodeInteractor>, ITreeNodeInteractor
    {
        /// <summary>
        /// The get layer for the first node use case.
        /// </summary>
        private readonly IGetLayerForFirstNodeUseCase _getLayerForFirstNodeUseCase;

        /// <summary>
        /// The extract morphological information from morphological parameters use case.
        /// </summary>
        private readonly IExtractMorphInfoFromMorphParametersUseCase _extractMorphInfoFromMorphParametersUseCase;

        /// <summary>
        /// Initializes a new instance of <see cref="TreeNodeInteractor"/> class.
        /// </summary>
        /// <param name="getLayerForFirstNodeUseCase">The get layer for the first node use case.</param>
        /// <param name="extractMorphInfoFromMorphParametersUseCase">The extract morphological information from morphological parameters use case.</param>
        /// <param name="logger">The logger.</param>
        public TreeNodeInteractor(IGetLayerForFirstNodeUseCase getLayerForFirstNodeUseCase,
            IExtractMorphInfoFromMorphParametersUseCase extractMorphInfoFromMorphParametersUseCase,
            ILogger<TreeNodeInteractor> logger) : base(logger)
        {
            _getLayerForFirstNodeUseCase = getLayerForFirstNodeUseCase;
            _extractMorphInfoFromMorphParametersUseCase = extractMorphInfoFromMorphParametersUseCase;

            Logger.LogInit();
        }

        #region Implementation of ITreeNodeViewModelInteractor

        /// <summary>
        /// Gets a chronological layer for the first tree node.
        /// </summary>
        /// <returns>The operation result.</returns>
        public OperationResult<uint> GetLayerForFirstNode()
        {
            var result = _getLayerForFirstNodeUseCase.Execute();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Extracts a morphological information from the collection of morphological parameters.
        /// </summary>
        /// <param name="parameters">The collection of morphological parameters.</param>
        /// <returns>The operation result.</returns>
        public OperationResult<string> ExtractMorphInfoFromMorphParameters(byte[] parameters)
        {
            var result = _extractMorphInfoFromMorphParametersUseCase.Execute(parameters);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        #endregion
    }
}
