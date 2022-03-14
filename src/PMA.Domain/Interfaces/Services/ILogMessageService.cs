// <copyright file="ILogMessageService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services.Base;
using System;

namespace PMA.Domain.Interfaces.Services
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ILogMessageService"/> interfacing class.
    /// </summary>
    public interface ILogMessageService : IService
    {
        /// <summary>
        /// Event handler when a new message received.
        /// </summary>
        event EventHandler<LogMessageEventArgs> MessageReceived;

        /// <summary>
        /// Raises the message received event.
        /// </summary>
        /// <param name="message">The message type.</param>
        void SendMessage(string message);

        /// <summary>
        /// Raises the message received event.
        /// </summary>
        /// <param name="level">The message level.</param>
        /// <param name="message">The message text.</param>
        void SendMessage(MessageLevel level, string message);
    }
}
