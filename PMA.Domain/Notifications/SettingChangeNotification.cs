// <copyright file="SettingChangeNotification.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;

namespace PMA.Domain.Notifications
{
    /// <summary>
    /// The setting change notification class.
    /// </summary>
    public class SettingChangeNotification : INotification
    {
        /// <summary>
        /// Gets or sets the setting name.
        /// </summary>
        public string SettingName { get; init; }
    }
}
