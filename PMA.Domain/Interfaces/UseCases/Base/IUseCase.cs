// <copyright file="IUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;

namespace PMA.Domain.Interfaces.UseCases.Base
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IUseCase"/> interfacing class.
    /// </summary>
    public interface IUseCase
    {
        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        OperationResult Execute();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IUseCase{TInput}"/> interfacing class.
    /// </summary>
    /// <typeparam name="TInput">Type of the input data.</typeparam>
    public interface IUseCase<in TInput>
    {
        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        OperationResult Execute(TInput inputData);
    }
}
