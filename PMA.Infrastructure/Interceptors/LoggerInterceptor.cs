// <copyright file="LoggerInterceptor.cs" company="NCR">
//     Copyright 2021 NCR Corporation. All rights reserved.
// </copyright>

using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Interceptors;
using System;
using System.Diagnostics;
using System.Linq;

namespace PMA.Infrastructure.Interceptors
{
    /// <summary>
    /// The logger interceptor class.
    /// </summary>
    public class LoggerInterceptor : IInterceptor, ILoggerInterceptor
    {
        /// <summary>
        /// The execution time limit for warnings.
        /// </summary>
        private const long WarningExecutionTime = 500;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<LoggerInterceptor> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerInterceptor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
        {
            _logger = logger;
        }

        #region Implementation of IInterceptor

        /// <summary>
        /// Wraps a proxied method execution.
        /// </summary>
        /// <param name="invocation">The invocation of a proxied method.</param>
        public void Intercept(IInvocation invocation)
        {
            string name = $"{invocation.Method.DeclaringType}.{invocation.Method.Name}";
            string args = string.Join(", ", invocation.Arguments.Select(a => (a ?? string.Empty).ToString()));

            _logger.LogTrace(string.IsNullOrEmpty(args)
                ? $"+{name}"
                : $"+{name} => args: {args}");

            var watch = Stopwatch.StartNew();

            try
            {
                invocation.Proceed();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                throw;
            }

            watch.Stop();

            long executionTime = watch.ElapsedMilliseconds;

            if (executionTime > WarningExecutionTime)
            {
                _logger.LogWarning($"{name} => {executionTime} ms");
            }

            _logger.LogTrace(invocation.ReturnValue is null
                ? $"-{name} ({executionTime} ms)"
                : $"-{name} => return: {invocation.ReturnValue} ({executionTime} ms)");
        }

        #endregion
    }
}
