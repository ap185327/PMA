// <copyright file="MorphParameterDbProvider.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Models;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Providers.Base;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PMA.Infrastructure.Providers
{
    /// <summary>
    /// The morphological parameter database provider class.
    /// </summary>
    public class MorphParameterDbProvider : DbProviderBase<MorphParameterDbProvider>, IMorphParameterDbProvider
    {
        /// <summary>
        /// The collection of morphological parameters.
        /// </summary>
        private readonly IList<MorphParameter> _morphParameters;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphParameterDbProvider"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public MorphParameterDbProvider(SqLiteDbContext context, IMapperBase mapper, ILogger<MorphParameterDbProvider> logger) : base(context, mapper, logger)
        {
            Logger.LogInit();

            var settings = context.Settings.ToList();
            var strings = context.Strings.AsNoTracking().ToList();
            var morphParameterEntities = context.MorphParameters.AsNoTracking().ToList();

            _morphParameters = morphParameterEntities.Select(morphParameter =>
                new MorphParameter
                {
                    Id = morphParameter.Id,
                    Name = strings.Single(x => x.Name == $"MorphParameters[{morphParameter.Id}].Name").Value,
                    Category = strings.Single(x => x.Name == $"MorphParameters.Category[{morphParameter.CategoryId}]").Value,
                    Description = strings.Single(x => x.Name == $"MorphParameters[{morphParameter.Id}].Description").Value,
                    UseAltPropertyEntry = Convert.ToBoolean(morphParameter.UseAltPropertyEntry),
                    IsVisible = Convert.ToBoolean(settings.Single(x => x.Name == $"MorphParameters[{morphParameter.Id}].IsVisible").Value)
                }).ToList();
        }

        #region Implementation of IMorphParameterDbProvider

        /// <summary>
        /// Gets a collection of morphological parameters.
        /// </summary>
        /// <returns>A collection of morphological parameters.</returns>
        public IList<MorphParameter> GetValues()
        {
            return _morphParameters;
        }

        /// <summary>
        /// Updates morphological parameter visibility.
        /// </summary>
        public void UpdateVisibility()
        {
            var settings = Context.Settings.ToList();

            foreach (var morphParameter in _morphParameters)
            {
                morphParameter.IsVisible = Convert.ToBoolean(settings.Single(x => x.Name == $"MorphParameters[{morphParameter.Id}].IsVisible").Value);
            }
        }

        #endregion
    }
}
