// <copyright file="MorphCombinationManager.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Application.Extensions;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.Providers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Application.Managers
{
    /// <summary>
    /// The morphological combination manager.
    /// </summary>
    public sealed class MorphCombinationManager : IMorphCombinationManager
    {
        /// <summary>
        /// The morphological combination database provider.
        /// </summary>
        private readonly IMorphCombinationDbProvider _morphCombinationDbProvider;

        /// <summary>
        /// The collection of cached collection of parameters.
        /// </summary>
        private readonly ConcurrentDictionary<string, byte[]> _parameterTempDictionary = new();

        /// <summary>
        /// The collection of cached boolean values.
        /// </summary>
        private readonly ConcurrentDictionary<string, bool> _boolTempDictionary = new();

        /// <summary>
        /// Initializes the new instance of <see cref="MorphCombinationManager"/> class.
        /// </summary>
        /// <param name="morphCombinationDbProvider">The morphological combination database provider.</param>
        public MorphCombinationManager(IMorphCombinationDbProvider morphCombinationDbProvider)
        {
            _morphCombinationDbProvider = morphCombinationDbProvider;
        }

        #region Implementation of IMorphCombinationService

        /// <summary>
        /// Checks morphological parameters are valid or not.
        /// </summary>
        /// <param name="parameters">Morphological parameters.</param>
        /// <returns>A morphological parameters are valid or not.</returns>
        public bool Check(byte[] parameters)
        {
            var rows = _morphCombinationDbProvider.GetValues();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == 0) continue;

                rows = rows.Where(x => x[i] == parameters[i]).ToList();

                if (!rows.Any()) return false;
            }

            return true;
        }

        /// <summary>
        /// Checks and caches morphological parameters are valid or not.
        /// </summary>
        /// <param name="parameters">Morphological parameters.</param>
        /// <returns>Morphological parameters are valid or not.</returns>
        public bool CheckAndCache(byte[] parameters)
        {
            string key = string.Join(",", parameters);

            if (_boolTempDictionary.TryGetValue(key, out bool value)) return value;

            var rows = _morphCombinationDbProvider.GetValues();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == 0) continue;

                rows = rows.Where(x => x[i] == parameters[i]).ToList();

                if (rows.Count != 0) continue;

                _boolTempDictionary.TryAdd(key, false);

                return false;
            }

            _boolTempDictionary.TryAdd(key, true);

            return true;
        }

        /// <summary>
        /// Checks and caches morphological parameters are valid or not.
        /// </summary>
        /// <param name="parameters">Morphological parameters.</param>
        /// <param name="collectiveParameters">Collective morphological parameters of matching combinations.</param>
        /// <returns>Morphological parameters are valid or not.</returns>
        public bool CheckAndCache(byte[] parameters, out byte[] collectiveParameters)
        {
            string key = string.Join(",", parameters);

            if (_parameterTempDictionary.TryGetValue(key, out collectiveParameters)) return collectiveParameters != null;

            var rows = _morphCombinationDbProvider.GetValues();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == 0) continue;

                rows = rows.Where(x => x[i] == parameters[i]).ToList();

                if (rows.Count != 0) continue;

                _parameterTempDictionary.TryAdd(key, null);

                return false;
            }

            collectiveParameters = rows.ToList().GetCollectiveParameters();

            _parameterTempDictionary.TryAdd(key, collectiveParameters);

            return true;
        }

        /// <summary>
        /// Gets a collection of valid morphological combinations.
        /// </summary>
        /// <param name="parameters">Morphological parameters.</param>
        /// <returns>A collection of valid morphological combinations.</returns>
        public IList<byte[]> GetValidParameters(byte[] parameters)
        {
            var rows = _morphCombinationDbProvider.GetValues();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] != 0) rows = rows.Where(x => x[i] == parameters[i]).ToList();
            }

            return rows.ToList();
        }

        /// <summary>
        /// Clears temporary data used by the service.
        /// </summary>
        public void Clear()
        {
            _parameterTempDictionary.Clear();
            _boolTempDictionary.Clear();
        }

        #endregion
    }
}
