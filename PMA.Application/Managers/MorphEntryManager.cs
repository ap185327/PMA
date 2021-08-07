// <copyright file="MorphEntryManager.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PMA.Application.Managers
{
    /// <summary>
    /// The morphological entry manager class.
    /// </summary>
    public sealed class MorphEntryManager : IMorphEntryManager
    {
        /// <summary>
        /// The morphological entry database provider.
        /// </summary>
        private readonly IMorphEntryDbProvider _morphEntryDbProvider;

        /// <summary>
        /// The collection of cached values.
        /// </summary>
        private readonly ConcurrentDictionary<string, IList<MorphEntry>> _tempDictionary = new();

        /// <summary>
        /// Initializes the new instance of <see cref="MorphEntryManager"/> class.
        /// </summary>
        /// <param name="morphEntryDbProvider">The morphological entry database provider.</param>
        public MorphEntryManager(IMorphEntryDbProvider morphEntryDbProvider)
        {
            _morphEntryDbProvider = morphEntryDbProvider;
        }

        #region Implementation of IMorphEntryService

        /// <summary>
        /// Get morphological entry by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The morphological entry.</returns>
        public MorphEntry GetValue(int id)
        {
            return _morphEntryDbProvider.GetValues().Single(x => x.Id == id);
        }

        /// <summary>
        /// Gets a collection of morphological entries by parameters.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">The morphological base.</param>
        /// <param name="isVirtual">The solution is virtual (doesn't exist in the live language) or not.</param>
        /// <param name="left">A left morphological entry.</param>
        /// <param name="right">A right morphological entry.</param>
        /// <returns>The collection of morphological entries.</returns>
        public IList<MorphEntry> GetValues(string entry, byte[] parameters, MorphBase morphBase, bool isVirtual, MorphEntry left = null, MorphEntry right = null)
        {
            List<MorphEntry> values;

            if (entry.Contains("*"))
            {
                var regex = new Regex($"\\A{entry.Replace("*", ".*")}\\z");
                values = _morphEntryDbProvider.GetValues().Where(x => regex.IsMatch(x.Entry)).ToList();
            }
            else
            {
                values = _morphEntryDbProvider.GetValues().Where(x => x.Entry == entry).ToList();
            }

            if (left != null)
            {
                values = values.Where(x => x.Left != null).ToList();

                if (left.Id > 0)
                {
                    values = values.Where(x => x.Left.Id == left.Id).ToList();
                }

                if (!string.IsNullOrEmpty(left.Entry))
                {
                    values = values.Where(x => x.Left.Entry == left.Entry).ToList();
                }
            }

            if (right != null)
            {
                values = values.Where(x => x.Right != null).ToList();

                if (right.Id > 0)
                {
                    values = values.Where(x => x.Right.Id == right.Id).ToList();
                }

                if (!string.IsNullOrEmpty(right.Entry))
                {
                    values = values.Where(x => x.Right.Entry == right.Entry).ToList();
                }
            }

            if (values.Count == 0)
            {
                return values;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                byte parameter = parameters[i];

                if (parameter == 0) continue;

                values = values.Where(x => x.Parameters[i] == 0 || x.Parameters[i] == parameter).ToList();

                if (values.Count == 0)
                {
                    return values;
                }
            }

            if (morphBase != MorphBase.Unknown)
            {
                values = values.Where(x => x.Base == MorphBase.Unknown || x.Base == morphBase).ToList();
            }

            return values.Count == 0 ? values : values.Where(x => x.IsVirtual == isVirtual).ToList();
        }

        /// <summary>
        /// Gets and caches a collection of morphological entries by parameters.
        /// </summary>
        /// <param name="entry">An entry.</param>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="morphBase">A morphological base.</param>
        /// <param name="isVirtual">Whether a MorphEntry is virtual (doesn't exist in the live language) or not.</param>
        /// <returns>A collection of morphological entries.</returns>
        public IList<MorphEntry> GetValuesAndCache(string entry, byte[] parameters, MorphBase morphBase, bool isVirtual)
        {
            string key = entry + string.Join(",", parameters) + "," + morphBase + "," + isVirtual;

            if (_tempDictionary.TryGetValue(key, out var values))
            {
                return values;
            }

            values = GetValues(entry, parameters, morphBase, isVirtual);

            _tempDictionary.TryAdd(key, values);

            return values;
        }

        /// <summary>
        /// Clears temporary data used by the service.
        /// </summary>
        public void Clear()
        {
            _tempDictionary.Clear();
        }

        #endregion
    }
}
