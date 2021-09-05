// <copyright file="FreqRatingManager.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Constants;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.Providers;
using PMA.Utils.Exceptions;
using PMA.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace PMA.Application.Managers
{
    /// <summary>
    /// The frequency rating manager class.
    /// </summary>
    public sealed class FreqRatingManager : IFreqRatingManager
    {
        /// <summary>
        /// The frequency rating database provider.
        /// </summary>
        private readonly IFreqRatingDbProvider _freqRatingDbProvider;

        /// <summary>
        /// The collection of cached rating values.
        /// </summary>
        private readonly ConcurrentDictionary<string, double> _tempDictionary = new();

        /// <summary>
        /// The collection of maximal counts for all layers.
        /// </summary>
        private readonly int[] _maxCounts = new int[8];

        /// <summary>
        /// A current chronological layer.
        /// </summary>
        private int _layer = -1;

        /// <summary>
        /// Initializes the new instance of <see cref="FreqRatingManager"/> class.
        /// </summary>
        /// <param name="freqRatingDbProvider">The frequency rating provider.</param>
        public FreqRatingManager(IFreqRatingDbProvider freqRatingDbProvider)
        {
            _freqRatingDbProvider = freqRatingDbProvider;

            for (int i = 0; i < 8; i++)
            {
                _maxCounts[i] = _freqRatingDbProvider.GetSum($"Layer{i + 1}");
            }
        }

        #region Implementation of IFreqRatingManager

        /// <summary>
        /// Gets a chronological layer.
        /// </summary>
        public uint GetLayer()
        {
            if (_layer == -1)
            {
                throw new CustomException(ErrorMessageConstants.LayerIsNotSet);
            }

            return (uint)_layer;
        }

        /// <summary>
        /// Sets a chronological layer.
        /// </summary>
        /// <param name="layer">The layer number.</param>
        public void SetLayer(uint layer)
        {
            _layer = (int)layer;
        }

        /// <summary>
        /// Automatically sets a chronological layer by the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void SetLayerByEntry(string entry)
        {
            int[] sums = _freqRatingDbProvider.GetSums(entry);

            double[] ratings = new double[8];

            for (int i = 0; i < ratings.Length; i++)
            {
                ratings[i] = (double)sums[i] / _maxCounts[i];
            }

            _layer = ratings.Sum() == 0 ? MorphConstants.DefaultLayer : ratings.MaxIndex() + 1;
        }

        /// <summary>
        /// Gets rating by the entry for the current chronological layer.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>The rating value.</returns>
        public double GetRating(string entry)
        {
            if (_layer == -1)
            {
                throw new CustomException(ErrorMessageConstants.LayerIsNotSet);
            }

            if (_tempDictionary.TryGetValue(entry, out double rating))
            {
                return rating;
            }

            int currentCount = _freqRatingDbProvider.GetSum($"Layer{_layer}", entry);

            double pow = 1d / entry.Length;

            int maxCount = _maxCounts[_layer - 1]; // maximum word count for the specific layer.

            rating = Math.Pow((double)currentCount / maxCount, pow);

            _tempDictionary.TryAdd(entry, rating);

            return rating;
        }

        /// <summary>
        /// Clears temporary data used by manager.
        /// </summary>
        public void Clear()
        {
            _tempDictionary.Clear();
        }

        #endregion
    }
}
