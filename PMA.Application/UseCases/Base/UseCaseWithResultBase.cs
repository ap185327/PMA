// <copyright file="UseCaseWithResultBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.UseCases.Base;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Base
{
    /// <summary>
    /// The base use case with result class.
    /// </summary>
    /// <typeparam name="T">Type of the implementation class.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public abstract class UseCaseWithResultBase<T, TResult> : IUseCaseWithResult<TResult> where T : class
    {
        /// <summary>
        /// The mediator.
        /// </summary>
        protected readonly IMediator Mediator;

        /// <summary>
        /// Options that configure the operation of methods on the <see cref="Parallel"/> class.
        /// </summary>
        protected readonly ParallelOptions ParallelOptions;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<T> Logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UseCaseWithResultBase{T, TResult}"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        protected UseCaseWithResultBase(IMediator mediator, ParallelOptions parallelOptions, ILogger<T> logger)
        {
            Mediator = mediator;
            ParallelOptions = parallelOptions;
            Logger = logger;
        }

        #region Implementation of IUseCaseWithResult

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        public abstract OperationResult<TResult> Execute();

        #endregion
    }

    /// <summary>
    /// The base use case with result class.
    /// </summary>
    /// <typeparam name="T">Type of the implementation class.</typeparam>
    /// <typeparam name="TInput">Type of the input data.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public abstract class UseCaseWithResultBase<T, TInput, TResult> : IUseCaseWithResult<TInput, TResult> where T : class
    {
        /// <summary>
        /// The mediator.
        /// </summary>
        protected readonly IMediator Mediator;

        /// <summary>
        /// Options that configure the operation of methods on the <see cref="Parallel"/> class.
        /// </summary>
        protected readonly ParallelOptions ParallelOptions;

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<T> Logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UseCaseWithResultBase{T, TInput, TResult}"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        protected UseCaseWithResultBase(IMediator mediator, ParallelOptions parallelOptions, ILogger<T> logger)
        {
            Mediator = mediator;
            ParallelOptions = parallelOptions;
            Logger = logger;
        }

        #region Implementation of IUseCaseWithResult<in TInput>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public abstract OperationResult<TResult> Execute(TInput inputData);

        #endregion
    }
}
