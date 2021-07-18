// <copyright file="MorphRuleManager.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Application.Managers
{
    /// <summary>
    /// The morphological rule manager.
    /// </summary>
    public sealed class MorphRuleManager : IMorphRuleManager
    {
        /// <summary>
        /// The morphological rule database provider.
        /// </summary>
        private readonly IMorphRuleDbProvider _morphRuleDbProvider;

        /// <summary>
        /// The collection of cached morphological rule collection values.
        /// </summary>
        private readonly ConcurrentDictionary<string, IList<MorphRule>> _morphRuleTempDictionary = new();

        /// <summary>
        /// The collection of cached sandhi match collection values.
        /// </summary>
        private readonly ConcurrentDictionary<string, IList<SandhiMatch>> _sandhiMatchTempDictionary = new();

        /// <summary>
        /// Initializes the new instance of <see cref="MorphRuleManager"/> class.
        /// </summary>
        /// <param name="morphRuleDbProvider">The morphological rule database provider.</param>
        public MorphRuleManager(IMorphRuleDbProvider morphRuleDbProvider)
        {
            _morphRuleDbProvider = morphRuleDbProvider;
        }

        #region Implementation of IMorphRuleService

        /// <summary>
        /// Gets and caches a collection of morphological rules by the label and morphological parameters.
        /// </summary>
        /// <param name="label">A rule label.</param>
        /// <param name="parameters">Morphological parameters</param>
        /// <returns>A collection of morphological rules</returns>
        public IList<MorphRule> GetAndCacheRules(string label, byte[] parameters)
        {
            string key = label + string.Join(",", parameters);

            if (_morphRuleTempDictionary.TryGetValue(key, out var rules)) return rules;

            rules = _morphRuleDbProvider.GetValues().Where(x => x.Label == label).ToList();

            if (rules.Count > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    byte parameter = parameters[i];

                    if (parameter == 0) continue;

                    rules = rules.Where(x => x.Parameters[i] == 0 || x.Parameters[i] == parameter).ToList();

                    if (rules.Count == 0)
                    {
                        break;
                    }
                }
            }

            _morphRuleTempDictionary.TryAdd(key, rules);

            return rules;
        }

        /// <summary>
        /// Gets and caches a collection of sandhi matches by the entry and the morphological rule.
        /// </summary>
        /// <param name="entry">An entry.</param>
        /// <param name="morphRule">A morphological rule.</param>
        /// <returns>A collection of sandhi matches.</returns>
        public IList<SandhiMatch> GetAndCacheSandhiMatches(string entry, MorphRule morphRule)
        {
            string key = morphRule.SandhiGroup + entry;

            if (_sandhiMatchTempDictionary.TryGetValue(key, out var sandhiMatches))
            {
                return sandhiMatches;
            }

            sandhiMatches = new List<SandhiMatch>();

            if (morphRule.SandhiGroup < (int)SandhiGroup.Other)
            {
                sandhiMatches.Add(new SandhiMatch
                {
                    SandhiExpression = entry
                });

                return !_sandhiMatchTempDictionary.TryAdd(key, sandhiMatches) ? _sandhiMatchTempDictionary[key] : sandhiMatches;
            }

            for (int i = 0; i < morphRule.SandhiRules.Count; i++)
            {
                var sandhiRule = morphRule.SandhiRules[i];

                var matches = sandhiRule.Regex.Matches(entry);

                if (matches.Count <= 0) continue;

                for (int j = 0; j < matches.Count; j++)
                {
                    var match = matches[j];

                    for (int k = 0; k < sandhiRule.RegexResults.Count; k++)
                    {
                        sandhiMatches.Add(new SandhiMatch
                        {
                            SandhiExpression = sandhiRule.Regex.Replace(entry, sandhiRule.RegexResults[k], 1, match.Index),
                            Rules = new List<SandhiRule> { sandhiRule }
                        });
                    }
                }
            }

            sandhiMatches = sandhiMatches.Count switch
            {
                > 1 => sandhiMatches.GroupBy(x => x.SandhiExpression,
                        (e, rule) =>
                            new SandhiMatch { SandhiExpression = e, Rules = rule.Select(x => x.Rules[0]).ToList() })
                    .ToList(),
                0 => null,
                _ => sandhiMatches
            };

            _sandhiMatchTempDictionary.TryAdd(key, sandhiMatches);

            return sandhiMatches;
        }

        /// <summary>
        /// Clears temporary data used by the service.
        /// </summary>
        public void Clear()
        {
            _morphRuleTempDictionary.Clear();
            _sandhiMatchTempDictionary.Clear();
        }

        #endregion
    }
}
