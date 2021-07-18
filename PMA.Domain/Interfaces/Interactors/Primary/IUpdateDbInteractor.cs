// <copyright file="IUpdateDbInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;

namespace PMA.Domain.Interfaces.Interactors.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IUpdateDbInteractor"/> interfacing class.
    /// </summary>
    public interface IUpdateDbInteractor : IInteractor
    {
        /// <summary>
        /// Starts the update database process.
        /// </summary>
        /// <param name="inputData">The update database input port.</param>
        OperationResult StartDbUpdating(UpdateDbInputPort inputData);

        /// <summary>
        /// Stops the update database process.
        /// </summary>
        OperationResult StopDbUpdating();
    }
}
