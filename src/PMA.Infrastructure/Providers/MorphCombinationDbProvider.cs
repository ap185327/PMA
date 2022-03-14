// <copyright file="MorphCombinationDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Providers;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Providers.Base;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Infrastructure.Providers
{
    /// <summary>
    /// The morphological combination database provider class.
    /// </summary>
    public class MorphCombinationDbProvider : DbProviderBase<MorphCombinationDbProvider>, IMorphCombinationDbProvider
    {
        /// <summary>
        /// The collection of morphological combinations.
        /// </summary>
        private IList<byte[]> _morphCombinations;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphCombinationDbProvider"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public MorphCombinationDbProvider(SqLiteDbContext context, IMapperBase mapper, ILogger<MorphCombinationDbProvider> logger) : base(context, mapper, logger)
        {
            Logger.LogInit();

            Reload();
        }

        #region Implementation of IMorphCombinationDbProvider

        /// <summary>
        /// Gets a collection of morphological combinations.
        /// </summary>
        /// <returns>A collection of combinations.</returns>
        public IList<byte[]> GetValues()
        {
            return _morphCombinations;
        }

        /// <summary>
        /// Reloads all data.
        /// </summary>
        public void Reload()
        {
            _morphCombinations = Context.MorphCombinations.ToList().Select(Mapper.Map<byte[]>).ToList();
        }

        #endregion
    }
}
