// <copyright file="ITreeNodeInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Domain.Interfaces.Interactors.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ITreeNodeInteractor"/> interfacing class.
    /// </summary>
    public interface ITreeNodeInteractor : IInteractor
    {
        /// <summary>
        /// Gets a wordform tree node view model.
        /// </summary>
        /// <param name="inputPort">The get wordform tree node input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The wordform tree node view model.</returns>
        Task<OperationResult<IWordFormTreeNodeViewModel>> GetWordFormTreeNodeAsync(GetWordFormTreeNodeInputPort inputPort, CancellationToken token = default);

        /// <summary>
        /// Gets a collection of solution tree node view models.
        /// </summary>
        /// <param name="groups">The solution tree node group input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The a collection of solution tree node view models.</returns>
        Task<OperationResult<IList<ISolutionTreeNodeViewModel>>> GetSolutionTreeNodesAsync(IList<SolutionTreeNodeViewModelGroup> groups, CancellationToken token = default);

        /// <summary>
        /// Gets a chronological layer for the first tree node.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The chronological layer for the first tree node.</returns>
        Task<OperationResult<uint>> GetLayerForFirstNodeAsync(CancellationToken token = default);

        /// <summary>
        /// Extracts a morphological information from the collection of morphological parameters.
        /// </summary>
        /// <param name="inputPort">The extract a morphological information input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        Task<OperationResult<string>> ExtractMorphInfoFromMorphParametersAsync(ExtractMorphInfoInputPort inputPort, CancellationToken token = default);
    }
}
