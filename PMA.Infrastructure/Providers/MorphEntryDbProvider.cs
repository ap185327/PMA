// <copyright file="MorphEntryDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Models;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Entities;
using PMA.Infrastructure.Extensions;
using PMA.Infrastructure.Providers.Base;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Infrastructure.Providers
{
    /// <summary>
    /// The morphological entry database provider class.
    /// </summary>
    public class MorphEntryDbProvider : DbProviderBase<MorphEntryDbProvider>, IMorphEntryDbProvider
    {
        /// <summary>
        /// The collection of morphological entries.
        /// </summary>
        private readonly IList<MorphEntry> _morphEntries;

        /// <summary>
        /// Initializes the new instance of <see cref="DbProviderBase{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public MorphEntryDbProvider(SqLiteDbContext context, IMapperBase mapper, ILogger<MorphEntryDbProvider> logger) : base(context, mapper, logger)
        {
            Logger.LogInit();

            var entities = context.MorphEntries.ToList();

            _morphEntries = entities.Select(mapper.Map<MorphEntry>).ToList();

            var children = entities.Where(x => x.LeftEntryId > 0 || x.RightEntryId > 0).ToList();
            foreach (var item in children)
            {
                var morphEntry = _morphEntries.Single(x => x.Id == item.Id);

                if (item.LeftEntryId > 0) morphEntry.Left = _morphEntries.Single(x => x.Id == item.LeftEntryId);
                if (item.RightEntryId > 0) morphEntry.Right = _morphEntries.Single(x => x.Id == item.RightEntryId);
            }
        }

        #region Implementation of IMorphEntryDbProvider

        /// <summary>
        /// Gets a collection of morphological entries.
        /// </summary>
        /// <returns>A collection of morphological entries.</returns>
        public IList<MorphEntry> GetValues()
        {
            return _morphEntries;
        }

        /// <summary>
        /// Updates a morphological entry in the database.
        /// </summary>
        /// <param name="morphEntry">A morphological entry.</param>
        /// <param name="commit">Whether to save changes to the database after the operation.</param>
        public void Update(MorphEntry morphEntry, bool commit = true)
        {
            var entity = Context.MorphEntries.Find(morphEntry.Id);
            entity.OverrideBy(morphEntry);

            if (commit)
            {
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Inserts a morphological entry to the database.
        /// </summary>
        /// <param name="morphEntry">A morphological entry.</param>
        /// <param name="commit">Whether to save changes to the database after the operation.</param>
        public void Insert(MorphEntry morphEntry, bool commit = true)
        {
            morphEntry.Id = _morphEntries.Max(x => x.Id) + 1;

            _morphEntries.Add(morphEntry);

            var morphEntryEntity = Mapper.Map<MorphEntryEntity>(morphEntry);

            Context.MorphEntries.Add(morphEntryEntity);

            if (commit)
            {
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a morphological entry from the database.
        /// </summary>
        /// <param name="morphEntry">A morphological entry.</param>
        /// <param name="commit">Whether to save changes to the database after the operation.</param>
        public void Delete(MorphEntry morphEntry, bool commit = true)
        {
            _morphEntries.Remove(morphEntry);

            var entity = Context.MorphEntries.Find(morphEntry.Id);

            Context.MorphEntries.Remove(entity);

            if (commit)
            {
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Saves database changes.
        /// </summary>
        public void Commit()
        {
            Context.SaveChanges();
        }

        #endregion
    }
}
