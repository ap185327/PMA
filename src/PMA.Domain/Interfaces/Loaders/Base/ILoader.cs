// <copyright file="ILoader.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace PMA.Domain.Interfaces.Loaders.Base
{
    /// <summary>
    /// Initializes a new instance of the ILoader interfacing class.
    /// </summary>
    public interface ILoader
    {
        /// <summary>
        /// Reads raw data.
        /// </summary>
        /// <returns>True if the raw data has been read; otherwise - False.</returns>
        bool ReadRawData();

        /// <summary>
        /// Validates and formats raw data.
        /// </summary>
        /// <returns>True if the raw data has been validated; otherwise - False.</returns>
        bool ValidateAndFormatRawData();

        /// <summary>
        /// Validates and formats raw data.
        /// </summary>
        /// <returns>True if the raw data has been validated; otherwise - False.</returns>
        Task<bool> ValidateAndFormatRawDataAsync(CancellationToken token = default);

        /// <summary>
        /// Clears temp data.
        /// </summary>
        void Clear();
    }
}
