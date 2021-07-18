// <copyright file="CancellationTokenResourceNotification.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using System.Threading;

namespace PMA.Domain.Notifications
{
    /// <summary>
    /// The cancellation token source notification class.
    /// </summary>
    public class CancellationTokenResourceNotification : INotification
    {
        /// <summary>
        /// Gets or sets a cancellation token source. 
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; init; }
    }
}
