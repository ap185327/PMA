// <copyright file="TermDbProvider.cs" company="Andrey Pospelov">
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
    /// The term database provider class.
    /// </summary>
    public class TermDbProvider : DbProviderBase<TermDbProvider>, ITermDbProvider
    {
        /// <summary>
        /// The collection of terms.
        /// </summary>
        private readonly IList<Term> _terms;

        /// <summary>
        /// Initializes the new instance of <see cref="TermDbProvider"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public TermDbProvider(SqLiteDbContext context, IMapperBase mapper, ILogger<TermDbProvider> logger) : base(context, mapper, logger)
        {
            Logger.LogInit();

            _terms = Context.Terms.ToList().Select(term =>
                new Term
                {
                    Id = term.Id,
                    Entry = term.Entry,
                    AltEntry = term.AltEntry,
                    AltPropertyEntry = term.AltPropEntry
                }).ToList();
        }

        #region Implementation of ITermDbProvider

        /// <summary>
        /// Gets a collection of terms.
        /// </summary>
        /// <returns>A collection of terms.</returns>
        public IList<Term> GetValues()
        {
            return _terms;
        }

        #endregion
    }
}
