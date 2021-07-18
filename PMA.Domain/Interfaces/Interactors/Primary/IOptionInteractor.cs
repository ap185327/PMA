// <copyright file="IOptionInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;
using PMA.Domain.OutputPorts;

namespace PMA.Domain.Interfaces.Interactors.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IOptionInteractor"/> interfacing class.
    /// </summary>
    public interface IOptionInteractor : IInteractor
    {
        /// <summary>
        /// Gets current option values.
        /// </summary>
        /// <returns>The operation result.</returns>
        OperationResult<OptionValueOutputPort> GetCurrentOptionValues();

        /// <summary>
        /// Saves option values.
        /// </summary>
        /// <param name="inputData">Options values.</param>
        /// <returns>The operation result.</returns>
        OperationResult SaveOptionValues(OptionValueInputPort inputData);
    }
}
