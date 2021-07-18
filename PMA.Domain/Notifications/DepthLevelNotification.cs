// <copyright file="DepthLevelNotification.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;

namespace PMA.Domain.Notifications
{
    /// <summary>
    /// The depth level notification class.
    /// </summary>
    public class DepthLevelNotification : INotification
    {
        /// <summary>
        /// Gets or sets a current depth level.
        /// </summary>
        public int CurrentDepthLevel { get; init; }
    }
}
