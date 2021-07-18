// <copyright file="MorphParserNotification.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using PMA.Domain.Enums;
using PMA.Domain.Models;

namespace PMA.Domain.Notifications
{
    /// <summary>
    /// The morphological parser notification class.
    /// </summary>
    public class MorphParserNotification : INotification
    {
        /// <summary>
        /// Gets or sets an analysis state.
        /// </summary>
        public ProcessState State { get; init; }

        /// <summary>
        /// Gets or sets a result of analysis.
        /// </summary>
        public WordForm Result { get; init; }
    }
}
