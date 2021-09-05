// <copyright file="IOptionInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Base;
using PMA.Domain.OutputPorts;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="token">The cancellation token.</param>
        /// <returns>The option value output port.</returns>
        Task<OperationResult<OptionValueOutputPort>> GetCurrentOptionValuesAsync(CancellationToken token = default);

        /// <summary>
        /// Saves option values.
        /// </summary>
        /// <param name="inputData">Options values.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        Task<OperationResult> SaveOptionValuesAsync(OptionValueInputPort inputData, CancellationToken token = default);
    }
}
