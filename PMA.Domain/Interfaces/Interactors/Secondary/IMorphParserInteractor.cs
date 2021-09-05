// <copyright file="IMorphParserInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;

namespace PMA.Domain.Interfaces.Interactors.Secondary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphParserInteractor"/> interfacing class.
    /// </summary>
    public interface IMorphParserInteractor : IInteractor
    {
        /// <summary>
        /// Parses morphological entry.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> ParseMorphEntryAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Removes solutions with excessive depth.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> RemoveSolutionsWithExcessiveDepthAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Collapses solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> CollapseSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Removes unsuitable derivative solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> RemoveUnsuitableDerivativeSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Updates solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> UpdateSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Removes duplicates.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> RemoveDuplicatesAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Removes unsuitable solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> RemoveUnsuitableSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Sort solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> SortSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Validates solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> ValidateSolutionsAsync(MorphParserInputPort inputData, CancellationToken token = default);

        /// <summary>
        /// Clears manager cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of operation.</returns>
        Task<OperationResult> ClearCacheAsync(CancellationToken token = default);
    }
}
