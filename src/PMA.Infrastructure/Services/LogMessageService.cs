// <copyright file="LogMessageService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Domain.Enums;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Models;
using PMA.Infrastructure.Services.Base;
using PMA.Utils.Extensions;
using System;

namespace PMA.Infrastructure.Services
{
    /// <summary>
    /// The log message service class.
    /// </summary>
    public class LogMessageService : ServiceBase<LogMessageService>, ILogMessageService
    {
        /// <summary>
        /// Initializes the new instance of <see cref="LogMessageService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LogMessageService(ILogger<LogMessageService> logger) : base(logger)
        {
            Logger.LogInit();
        }

        #region Implementation of ILogMessageService

        /// <summary>
        /// Event handler when a new message received.
        /// </summary>
        public event EventHandler<LogMessageEventArgs> MessageReceived;

        /// <summary>
        /// Raises the message received event.
        /// </summary>
        /// <param name="message">The message type.</param>
        public void SendMessage(string message)
        {
            SendMessage(MessageLevel.Information, message);
        }

        /// <summary>
        /// Raises the message received event.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="message">The message text.</param>
        public void SendMessage(MessageLevel level, string message)
        {
            MessageReceived?.Invoke(this, new LogMessageEventArgs
            {
                Message = new LogMessage
                {
                    Level = level,
                    Text = message
                }
            });
        }

        #endregion
    }
}
