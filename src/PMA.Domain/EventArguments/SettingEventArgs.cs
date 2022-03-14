// <copyright file="SettingEventArgs.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.EventArguments
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingEventArgs"/> class.
    /// </summary>
    public class SettingEventArgs
    {
        /// <summary>
        /// Gets or sets the setting name.
        /// </summary>
        public string SettingName { get; init; }
    }
}
