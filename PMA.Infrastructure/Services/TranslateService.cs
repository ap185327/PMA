// <copyright file="TranslateService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Domain.Interfaces.Services;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Services.Base;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Infrastructure.Services
{
    /// <summary>
    /// The translate service class.
    /// </summary>
    public class TranslateService : ServiceBase<TranslateService>, ITranslateService
    {
        /// <summary>
        /// Gets a collection of string name/value pairs. 
        /// </summary>
        private readonly IDictionary<string, string> _dictionary;

        /// <summary>
        /// Initializes the new instance of <see cref="TranslateService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger.</param>
        public TranslateService(SqLiteDbContext context, ILogger<TranslateService> logger) : base(logger)
        {
            Logger.LogInit();

            _dictionary = context.Strings.Select(x => new
            {
                Key = x.Name,
                x.Value,
            }).ToDictionary(y => y.Key, y => y.Value);
        }

        #region Implementation of ITranslateService

        /// <summary>
        /// Gets string value by name.
        /// </summary>
        /// <param name="name">The string name.</param>
        /// <param name="parameters">String parameters.</param>
        /// <returns>A string value.</returns>
        public string Translate(string name, params object[] parameters)
        {
            if (_dictionary.TryGetValue(name, out string value))
            {
                value = value.Replace("\\t", "\t").Replace("\\n", "\n");

                return parameters is null || parameters.Length == 0 ? value : string.Format(value, parameters);
            }

            if (parameters is null || parameters.Length == 0) return $"[{name}]";

            string parameterValues = "{" + string.Join("},{", parameters) + "}";

            return $"[{name} {parameterValues}]";
        }

        /// <summary>
        /// Gets a string value by the enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="parameters">String parameters.</param>
        /// <returns>A string value.</returns>
        public string Translate(Enum enumValue, params object[] parameters)
        {
            return Translate($"{enumValue.GetType().Name}.{enumValue}", parameters);
        }

        #endregion
    }
}
