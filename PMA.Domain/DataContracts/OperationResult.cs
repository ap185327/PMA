// <copyright file="OperationResult.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace PMA.Domain.DataContracts
{
    /// <summary>
    /// The wrapper class for any action.
    /// </summary>
    /// <typeparam name="TResult">Type of result value.</typeparam>
    public sealed class OperationResult<TResult>
    {
        /// <summary>
        /// Gets whether the operation result is success.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets whether the operation result has exception.
        /// </summary>
        public bool IsException => Exception != null;

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the collection of error messages.
        /// </summary>
        public IList<string> Messages { get; }

        /// <summary>
        /// Gets a result value.
        /// </summary>
        public TResult Result { get; }

        /// <summary>
        /// Initializes the new instance of <see cref="OperationResult{T}"/> class.
        /// </summary>
        /// <param name="result">The result value.</param>
        private OperationResult(TResult result)
        {
            Success = true;
            Result = result;
        }

        /// <summary>
        /// Initializes the new instance of <see cref="OperationResult{T}"/> class.
        /// </summary>
        /// <param name="messages">The collection of error messages.</param>
        private OperationResult(IList<string> messages)
        {
            Success = false;
            Messages = messages;
        }

        /// <summary>
        /// Initializes the new instance of <see cref="OperationResult{T}"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private OperationResult(Exception exception)
        {
            Success = false;
            Exception = exception;
            Messages = new List<string>
            {
                exception.Message
            };
        }

        /// <summary>
        /// Creates a new instance of <see cref="OperationResult{T}"/> class with success result.
        /// </summary>
        /// <param name="result">The result value</param>
        /// <returns>A new instance of <see cref="OperationResult{T}"/> class</returns>
        public static OperationResult<TResult> SuccessResult(TResult result) => new(result);

        /// <summary>
        /// Creates a new instance of <see cref="OperationResult{T}"/> class with failure result.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="parameters">Message parameters.</param>
        /// <returns>A new instance of <see cref="OperationResult{T}"/> class</returns>
        public static OperationResult<TResult> FailureResult(string message, params object[] parameters)
        {
            if (parameters is not null)
            {
                message = string.Format(message, parameters);
            }

            return new OperationResult<TResult>(new List<string> { message });
        }

        /// <summary>
        /// Creates a new instance of <see cref="OperationResult{T}"/> class with failure result.
        /// </summary>
        /// <param name="messages">The collection of error messages.</param>
        /// <returns>A new instance of <see cref="OperationResult{T}"/> class</returns>
        public static OperationResult<TResult> FailureResult(IList<string> messages) => new(messages);

        /// <summary>
        /// Creates a new instance of <see cref="OperationResult{T}"/> class with exception result.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>A new instance of <see cref="OperationResult{T}"/> class</returns>
        public static OperationResult<TResult> ExceptionResult(Exception exception) => new(exception);
    }

    /// <summary>
    /// The wrapper class for any action.
    /// </summary>
    public sealed class OperationResult
    {
        /// <summary>
        /// Gets whether the operation result is success.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets whether the operation result has exception.
        /// </summary>
        public bool IsException => Exception != null;

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the collection of error messages.
        /// </summary>
        public IList<string> Messages { get; }

        /// <summary>
        /// Initializes the new instance of <see cref="OperationResult"/> class.
        /// </summary>
        private OperationResult()
        {
            Success = true;
        }

        /// <summary>
        /// Initializes the new instance of <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="messages">The collection of error messages.</param>
        private OperationResult(IList<string> messages)
        {
            Success = false;
            Messages = messages;
        }

        /// <summary>
        /// Initializes the new instance of <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private OperationResult(Exception exception)
        {
            Success = false;
            Exception = exception;
            Messages = new List<string>
            {
                exception.Message
            };
        }

        /// <summary>
        /// Creates a new instance of <see cref="OperationResult"/> class with success result.
        /// </summary>
        /// <returns>A new instance of <see cref="OperationResult"/> class</returns>
        public static OperationResult SuccessResult() => new();

        /// <summary>
        /// Creates a new instance of <see cref="OperationResult"/> class with failure result.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="parameters">Message parameters.</param>
        /// <returns>A new instance of <see cref="OperationResult"/> class</returns>
        public static OperationResult FailureResult(string message, params object[] parameters)
        {
            if (parameters is not null)
            {
                message = string.Format(message, parameters);
            }

            return new OperationResult(new List<string> { message });
        }

        /// <summary>
        /// Creates a new instance of <see cref="OperationResult"/> class with failure result.
        /// </summary>
        /// <param name="messages">The collection of error messages.</param>
        /// <returns>A new instance of <see cref="OperationResult"/> class</returns>
        public static OperationResult FailureResult(IList<string> messages) => new(messages);

        /// <summary>
        /// Creates a new instance of <see cref="OperationResult"/> class with exception result.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>A new instance of <see cref="OperationResult"/> class</returns>
        public static OperationResult ExceptionResult(Exception exception) => new(exception);
    }
}
