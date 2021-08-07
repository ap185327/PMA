// <copyright file="IMorphParserInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

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
        /// <returns>The result of operation.</returns>
        OperationResult ParseMorphEntry(MorphParserInputPort inputData);

        /// <summary>
        /// Removes solutions with excessive depth.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        OperationResult RemoveSolutionsWithExcessiveDepth(MorphParserInputPort inputData);

        /// <summary>
        /// Collapses solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        OperationResult CollapseSolutions(MorphParserInputPort inputData);

        /// <summary>
        /// Removes unsuitable derivative solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        OperationResult RemoveUnsuitableDerivativeSolutions(MorphParserInputPort inputData);

        /// <summary>
        /// Updates solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        OperationResult UpdateSolutions(MorphParserInputPort inputData);

        /// <summary>
        /// Removes duplicates.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        OperationResult RemoveDuplicates(MorphParserInputPort inputData);

        /// <summary>
        /// Removes unsuitable solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        OperationResult RemoveUnsuitableSolutions(MorphParserInputPort inputData);

        /// <summary>
        /// Sort solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        OperationResult SortSolutions(MorphParserInputPort inputData);

        /// <summary>
        /// Validates solutions.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of operation.</returns>
        OperationResult ValidateSolutions(MorphParserInputPort inputData);

        /// <summary>
        /// Clears manager cache.
        /// </summary>
        /// <returns>The result of operation.</returns>
        OperationResult ClearCache();
    }
}
