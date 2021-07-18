// <copyright file="UpdateDbNotification.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using PMA.Domain.Enums;

namespace PMA.Domain.Notifications
{
    /// <summary>
    /// The update database notification class.
    /// </summary>
    public class UpdateDbNotification : INotification
    {
        /// <summary>
        /// Gets or sets an updating state.
        /// </summary>
        public ProcessState State { get; init; }
    }
}
