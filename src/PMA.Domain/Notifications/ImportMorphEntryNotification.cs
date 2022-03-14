// <copyright file="ImportMorphEntryNotification.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using PMA.Domain.Enums;

namespace PMA.Domain.Notifications
{
    /// <summary>
    /// The import morphological entry notification class.
    /// </summary>
    public class ImportMorphEntryNotification : INotification
    {
        /// <summary>
        /// Gets or sets an updating state.
        /// </summary>
        public ProcessState State { get; init; }

        /// <summary>
        /// Gets an analyze progress bar value.
        /// </summary>
        public int AnalyzeProgressBarValue { get; init; }
    }
}
