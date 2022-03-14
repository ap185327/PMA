// <copyright file="FreqRatingDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Providers;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Providers.Base;
using PMA.Utils.Extensions;
using System.Linq;

namespace PMA.Infrastructure.Providers
{
    public class FreqRatingDbProvider : DbProviderBase<FreqRatingDbProvider>, IFreqRatingDbProvider
    {
        /// <summary>
        /// Initializes the new instance of <see cref="FreqRatingDbProvider"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public FreqRatingDbProvider(SqLiteDbContext context, IMapperBase mapper, ILogger<FreqRatingDbProvider> logger) : base(context, mapper, logger)
        {
            Logger.LogInit();
        }

        #region Overrides of DbProviderBase<FreqRatingDbProvider>

        #endregion

        #region Implementation of IFreqRatingDbProvider

        /// <summary>
        /// Gets a sum of values in specific column by the entry.
        /// </summary>
        /// <param name="column">A column name.</param>
        /// <param name="entry">An entry.</param>
        /// <returns>A sum of values.</returns>
        public int GetSum(string column, string entry = null)
        {
            string sqlQuery = string.IsNullOrEmpty(entry)
                ? $"SELECT SUM({column}) AS Sum FROM FreqGram1"
                : $"SELECT COALESCE(SUM({column}), 0) AS Sum FROM FreqGram1 WHERE Entry LIKE '%{entry.Replace("(", "").Replace(")", "")}%'";

            return Context.FreqGram1Entities.FromSqlRaw(sqlQuery).Single().Sum;
        }

        /// <summary>
        /// Gets a collection of sum of values in all columns by the entry.
        /// </summary>
        /// <param name="entry">An entry.</param>
        /// <returns>A collection of sum of values.</returns>
        public int[] GetSums(string entry)
        {
            int[] sums = new int[8];

            for (int i = 0; i < sums.Length; i++)
            {
                sums[i] = GetSum($"Layer{i + 1}", entry);
            }

            return sums;
        }

        #endregion
    }
}
