// <copyright file="IFreqRatingDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Interfaces.Providers.Base;

namespace PMA.Domain.Interfaces.Providers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IFreqRatingDbProvider"/> interfacing class.
    /// </summary>
    public interface IFreqRatingDbProvider : IDbProvider
    {
        /// <summary>
        /// Gets a sum of values in specific column by the entry.
        /// </summary>
        /// <param name="column">A column name.</param>
        /// <param name="entry">An entry.</param>
        /// <returns>A sum of values.</returns>
        int GetSum(string column, string entry = null);

        /// <summary>
        /// Gets a collection of sum of values in all columns by the entry.
        /// </summary>
        /// <param name="entry">An entry.</param>
        /// <returns>A collection of sum of values.</returns>
        int[] GetSums(string entry);
    }
}
