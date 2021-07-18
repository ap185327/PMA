// <copyright file="IFreqRatingManager.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Interfaces.Managers
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IFreqRatingManager"/> interfacing class.
    /// </summary>
    public interface IFreqRatingManager
    {
        /// <summary>
        /// Gets a chronological layer.
        /// </summary>
        uint GetLayer();

        /// <summary>
        /// Sets a chronological layer.
        /// </summary>
        /// <param name="layer">The layer number.</param>
        void SetLayer(uint layer);

        /// <summary>
        /// Automatically sets a chronological layer by the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        void SetLayerByEntry(string entry);

        /// <summary>
        /// Gets rating by the entry for the current chronological layer.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>The rating value.</returns>
        double GetRating(string entry);

        /// <summary>
        /// Clears temporary data used by manager.
        /// </summary>
        void Clear();
    }
}
