// <copyright file="LogMessageEventArgs.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Models;
using System;

namespace PMA.Domain.EventArguments
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogMessageEventArgs"/> class.
    /// </summary>
    public class LogMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a log message.
        /// </summary>
        public LogMessage Message { get; init; }
    }
}
