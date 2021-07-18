// <copyright file="IMainInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;

namespace PMA.Domain.Interfaces.Interactors.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMainInteractor"/> interfacing class.
    /// </summary>
    public interface IMainInteractor : IInteractor
    {
        /// <summary>
        /// Starts the morphological parsing.
        /// </summary>
        /// <param name="inputData">The morphological parser input data.</param>
        /// <returns>The operation result.</returns>
        OperationResult StartAnalysis(MorphParserInputPort inputData);

        /// <summary>
        /// Stops the morphological parsing.
        /// </summary>
        /// <returns>The operation result.</returns>
        OperationResult StopAnalysis();
    }
}
