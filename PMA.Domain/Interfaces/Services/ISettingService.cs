// <copyright file="ISettingService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services.Base;
using System;

namespace PMA.Domain.Interfaces.Services
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ISettingService"/> interfacing class.
    /// </summary>
    public interface ISettingService : IService
    {
        /// <summary>
        ///  The event that the setting was changed.
        /// </summary>
        event EventHandler<SettingEventArgs> SettingChanged;

        /// <summary>
        /// Gets a setting value by name.
        /// </summary>
        /// <typeparam name="T">A type of value.</typeparam>
        /// <param name="name">A setting name.</param>
        /// <returns>A setting value.</returns>
        T GetValue<T>(string name);

        /// <summary>
        /// Sets a setting value.
        /// </summary>
        /// <param name="name">A setting name.</param>
        /// <param name="value">A setting value.</param>
        void SetValue(string name, object value);

        /// <summary>
        /// Saves setting changes.
        /// </summary>
        void Commit();
    }
}
