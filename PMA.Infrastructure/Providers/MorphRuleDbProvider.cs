// <copyright file="MorphRuleDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Models;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Providers.Base;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Infrastructure.Providers
{
    /// <summary>
    /// The morphological rule database provider class.
    /// </summary>
    public class MorphRuleDbProvider : DbProviderBase<MorphRuleDbProvider>, IMorphRuleDbProvider
    {
        /// <summary>
        /// The collection of morphological rules.
        /// </summary>
        private IList<MorphRule> _morphRules;

        /// <summary>
        /// Initializes the new instance of <see cref="DbProviderBase{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public MorphRuleDbProvider(SqLiteDbContext context, IMapperBase mapper, ILogger<MorphRuleDbProvider> logger) : base(context, mapper, logger)
        {
            Logger.LogInit();

            Reload();
        }

        #region Implementation of IMorphRuleDbProvider

        /// <summary>
        /// Gets a collection of morphological combinations.
        /// </summary>
        /// <returns>A collection of combinations.</returns>
        public IList<MorphRule> GetValues()
        {
            return _morphRules;
        }

        /// <summary>
        /// Reloads all data.
        /// </summary>
        public void Reload()
        {
            var sandhiResultEntities = Context.SandhiResults.ToList();
            var sandhiRuleEntities = Context.SandhiRules.ToList();
            var morphRuleEntities = Context.MorphRules.ToList();

            _morphRules = morphRuleEntities.Select(Mapper.Map<MorphRule>).ToList();

            foreach (var morphRule in _morphRules)
            {
                var sandhiRules = sandhiRuleEntities
                    .Where(sandhiRuleEntity => sandhiRuleEntity.GroupId == morphRule.SandhiGroup)
                    .Select(Mapper.Map<SandhiRule>).ToList();

                foreach (var sandhiRule in sandhiRules)
                {
                    sandhiRule.RegexResults = sandhiResultEntities
                        .Where(sandhiResult => sandhiResult.RuleId == sandhiRule.Id)
                        .Select(sandhiResult => sandhiResult.RegexResult).ToList();
                }

                morphRule.SandhiRules = sandhiRules;
            }
        }

        #endregion
    }
}
