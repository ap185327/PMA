// <copyright file="IUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Domain.Interfaces.UseCases.Base
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IUseCase"/> interfacing class.
    /// </summary>
    public interface IUseCase : IDisposable
    {
        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        OperationResult Execute();

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        Task<OperationResult> ExecuteAsync(CancellationToken token = default);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IUseCase{TInput}"/> interfacing class.
    /// </summary>
    /// <typeparam name="TInput">Type of the input data.</typeparam>
    public interface IUseCase<in TInput> : IDisposable
    {
        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        OperationResult Execute(TInput inputPort);

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        Task<OperationResult> ExecuteAsync(TInput inputPort, CancellationToken token = default);
    }
}
