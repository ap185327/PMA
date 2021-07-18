// <copyright file="LoggerExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PMA.Utils.Extensions
{
    /// <summary>
    /// Defines method extensions for logger.
    /// </summary>
    public static class LoggerExtension
    {
        /// <summary>
        /// Writes a instance initialization log message.
        /// </summary>
        /// <typeparam name="TCategoryName">The type who's name is used for the logger category name.</typeparam>
        /// <param name="logger">The <see cref="ILogger{TCategoryName}"/> to write to.</param>
        public static void LogInit<TCategoryName>(this ILogger<TCategoryName> logger)
        {
            logger.LogDebug($"Initialize a new instance of the class");
        }

        /// <summary>
        /// Writes a entry log message.
        /// </summary>
        /// <typeparam name="TCategoryName">The type who's name is used for the logger category name.</typeparam>
        /// <param name="logger">The <see cref="ILogger{TCategoryName}"/> to write to.</param>
        /// <param name="name">The method or property name of the caller to the method.</param>
        public static void LogEntry<TCategoryName>(this ILogger<TCategoryName> logger, [CallerMemberName] string name = null)
        {
            logger.LogDebug($"+{name}");
        }

        /// <summary>
        /// Writes a exit log message.
        /// </summary>
        /// <typeparam name="TCategoryName">The type who's name is used for the logger category name.</typeparam>
        /// <param name="logger">The <see cref="ILogger{TCategoryName}"/> to write to.</param>
        /// <param name="name">The method or property name of the caller to the method.</param>
        public static void LogExit<TCategoryName>(this ILogger<TCategoryName> logger, [CallerMemberName] string name = null)
        {
            logger.LogDebug($"-{name}");
        }

        /// <summary>
        /// Writes a collection of warning log messages.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="messages">The collection of warning log messages.</param>
        public static void LogWarnings(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                logger.LogWarning(message);
            }
        }

        /// <summary>
        /// Writes a collection of critical log messages.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="messages">The collection of critical log messages.</param>
        public static void LogCriticals(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                logger.LogCritical(message);
            }
        }

        /// <summary>
        /// Writes a collection of debug log messages.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="messages">The collection of debug log messages.</param>
        public static void LogDebugs(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                logger.LogDebug(message);
            }
        }

        /// <summary>
        /// Writes a collection of error log messages.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="messages">The collection of error log messages.</param>
        public static void LogErrors(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                logger.LogError(message);
            }
        }

        /// <summary>
        /// Writes a collection of information log messages.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="messages">The collection of information log messages.</param>
        public static void LogInformations(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                logger.LogInformation(message);
            }
        }

        /// <summary>
        /// Writes a collection of trace log messages.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="messages">The collection of trace log messages.</param>
        public static void LogTraces(this ILogger logger, IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                logger.LogTrace(message);
            }
        }
    }
}
