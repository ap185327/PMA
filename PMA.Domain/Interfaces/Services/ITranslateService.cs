// <copyright file="ITranslateService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Services.Base;
using System;

namespace PMA.Domain.Interfaces.Services
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ITranslateService"/> interfacing class.
    /// </summary>
    public interface ITranslateService : IService
    {
        /// <summary>
        /// Gets string value by name.
        /// </summary>
        /// <param name="name">The string name.</param>
        /// <param name="parameters">String parameters.</param>
        /// <returns>A string value.</returns>
        string Translate(string name, params object[] parameters);

        /// <summary>
        /// Gets a string value by the enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="parameters">String parameters.</param>
        /// <returns>A string value.</returns>
        string Translate(Enum enumValue, params object[] parameters);
    }
}
