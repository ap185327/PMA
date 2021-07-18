// <copyright file="RawLogMessage.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;

namespace PMA.Infrastructure.Models
{
    /// <summary>
    /// The raw log message class.
    /// </summary>
    internal class RawLogMessage
    {
        /// <summary>
        /// Gets or sets message type.
        /// </summary>
        public LogMessageType Type { get; set; }

        /// <summary>
        /// Gets or sets message parameters.
        /// </summary>
        public object[] Parameters { get; set; }
    }
}
