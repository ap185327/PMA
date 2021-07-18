// <copyright file="IMemoryLoader.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Interfaces.Loaders.Base
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMemoryLoader{TResult}"/> interfacing class.
    /// </summary>
    /// <typeparam name="TResult">Type of the result data.</typeparam>
    public interface IMemoryLoader<out TResult> : ILoader
    {
        /// <summary>
        /// Gets mapping data.
        /// </summary>
        /// <returns>The result data.</returns>
        TResult GetData();
    }
}
