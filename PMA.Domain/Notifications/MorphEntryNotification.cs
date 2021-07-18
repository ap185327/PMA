// <copyright file="MorphEntryNotification.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using PMA.Domain.Models;

namespace PMA.Domain.Notifications
{
    /// <summary>
    /// The morphological entry notification class.
    /// </summary>
    public class MorphEntryNotification : INotification
    {
        /// <summary>
        /// Gets or sets a morphological entry.
        /// </summary>
        public MorphEntry MorphEntry { get; init; }
    }
}
