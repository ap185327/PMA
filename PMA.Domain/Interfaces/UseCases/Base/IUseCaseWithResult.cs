// <copyright file="IUseCaseWithResult.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;

namespace PMA.Domain.Interfaces.UseCases.Base
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IUseCaseWithResult{TResult}"/> interfacing class.
    /// </summary>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface IUseCaseWithResult<TResult>
    {
        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        OperationResult<TResult> Execute();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IUseCaseWithResult{TInput, TResult}"/> interfacing class.
    /// </summary>
    /// <typeparam name="TInput">Type of the input data.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface IUseCaseWithResult<in TInput, TResult>
    {
        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        OperationResult<TResult> Execute(TInput inputData);
    }
}
