// <copyright file="ITermDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Providers.Base;
using PMA.Domain.Models;
using System.Collections.Generic;

namespace PMA.Domain.Interfaces.Providers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ITermDbProvider"/> interfacing class.
    /// </summary>
    public interface ITermDbProvider : IDbProvider
    {
        /// <summary>
        /// Gets a collection of terms.
        /// </summary>
        /// <returns>A collection of terms.</returns>
        IList<Term> GetValues();
    }
}
