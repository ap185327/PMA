// <copyright file="IUseCaseWithResult.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Domain.Interfaces.UseCases.Base
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IUseCaseWithResult{TResult}"/> interfacing class.
    /// </summary>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface IUseCaseWithResult<TResult> : IDisposable
    {
        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        OperationResult<TResult> Execute();

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        Task<OperationResult<TResult>> ExecuteAsync(CancellationToken token = default);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IUseCaseWithResult{TInput, TResult}"/> interfacing class.
    /// </summary>
    /// <typeparam name="TInput">Type of the input data.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface IUseCaseWithResult<in TInput, TResult> : IDisposable
    {
        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        OperationResult<TResult> Execute(TInput inputPort);

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        Task<OperationResult<TResult>> ExecuteAsync(TInput inputPort, CancellationToken token = default);
    }
}
