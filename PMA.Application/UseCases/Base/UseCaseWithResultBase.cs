// <copyright file="UseCaseWithResultBase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.UseCases.Base;
using PMA.Utils.Extensions;
using System.Threading;
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
        /// The logger.
        /// </summary>
        protected readonly ILogger<T> Logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UseCaseWithResultBase{T, TResult}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected UseCaseWithResultBase(ILogger<T> logger)
        {
            Logger = logger;

            Logger.LogInit();
        }

        #region Implementation of IUseCaseWithResult

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        public abstract OperationResult<TResult> Execute();

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public abstract Task<OperationResult<TResult>> ExecuteAsync(CancellationToken token = default);

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Logger.LogDispose();
        }

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
        /// The logger.
        /// </summary>
        protected readonly ILogger<T> Logger;

        /// <summary>
        /// Initializes a new instance of <see cref="UseCaseWithResultBase{T, TInput, TResult}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected UseCaseWithResultBase(ILogger<T> logger)
        {
            Logger = logger;

            Logger.LogInit();
        }

        #region Implementation of IUseCaseWithResult<in TInput>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public abstract OperationResult<TResult> Execute(TInput inputPort);

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public abstract Task<OperationResult<TResult>> ExecuteAsync(TInput inputPort, CancellationToken token = default);

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Logger.LogDispose();
        }

        #endregion
    }
}
