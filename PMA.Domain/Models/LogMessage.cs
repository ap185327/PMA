// <copyright file="LogMessage.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;

namespace PMA.Domain.Models
{
    /// <summary>
    /// The log message class.
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// Gets or sets message level.
        /// </summary>
        public MessageLevel Level { get; init; }

        /// <summary>
        /// Gets or sets message text.
        /// </summary>
        public string Text { get; init; }
    }
}
