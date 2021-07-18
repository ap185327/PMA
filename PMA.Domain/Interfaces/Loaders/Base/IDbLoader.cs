// <copyright file="IDbLoader.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Interfaces.Loaders.Base
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IDbLoader"/> interfacing class.
    /// </summary>
    public interface IDbLoader : ILoader
    {
        /// <summary>
        /// Loads mapping data to database.
        /// </summary>
        /// <returns>True if the raw data has been loaded; otherwise - False.</returns>
        bool LoadData();
    }
}
