// <copyright file="IMorphParameterDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Providers.Base;
using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Providers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphParameterDbProvider"/> interfacing class.
    /// </summary>
    public interface IMorphParameterDbProvider : IDbProvider
    {
        /// <summary>
        /// Gets a collection of morphological parameters.
        /// </summary>
        /// <returns>A collection of morphological parameters.</returns>
        IList<MorphParameter> GetValues();

        /// <summary>
        /// Updates morphological parameter visibility.
        /// </summary>
        void UpdateVisibility();
    }
}
