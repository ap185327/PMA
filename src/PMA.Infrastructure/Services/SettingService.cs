// <copyright file="SettingService.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Domain.Constants;
using PMA.Domain.EventArguments;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Models;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Services.Base;
using PMA.Utils.Exceptions;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Infrastructure.Services
{
    public class SettingService : ServiceBase<SettingService>, ISettingService
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly SqLiteDbContext _context;

        /// <summary>
        /// The collection of setting name/value pairs. 
        /// </summary>
        private readonly IDictionary<string, string> _settings;

        /// <summary>
        /// Initializes the new instance of <see cref="SettingService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logger.</param>
        public SettingService(SqLiteDbContext context, ILogger<SettingService> logger) : base(logger)
        {
            _context = context;

            Logger.LogInit();

            _settings = _context.Settings.Select(x => new
            {
                Key = x.Name,
                x.Value,
            }).ToDictionary(y => y.Key, y => y.Value);
        }

        #region Implementation of ISettingService

        /// <summary>
        ///  The event that the setting was changed.
        /// </summary>
        public event EventHandler<SettingEventArgs> SettingChanged;

        /// <summary>
        /// Gets a setting value by name.
        /// </summary>
        /// <typeparam name="T">A type of value.</typeparam>
        /// <param name="name">A setting name.</param>
        /// <returns>A setting value.</returns>
        public T GetValue<T>(string name)
        {
            if (!_settings.TryGetValue(name, out string value))
            {
                throw new CustomException($"'{name}' setting doesn't exist");
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Sets a setting value.
        /// </summary>
        /// <param name="name">A setting name.</param>
        /// <param name="value">A setting value.</param>
        public void SetValue(string name, object value)
        {
            if (value is null)
            {
                throw new CustomException(string.Format(ErrorMessageConstants.ValueIsNull, name));
            }

            if (!_settings.TryGetValue(name, out string oldValue))
            {
                throw new CustomException($"'{name}' setting doesn't exist");
            }

            string newValue = value.ToString();

            if (newValue == oldValue) return;

            var setting = new Setting
            {
                Name = name,
                Value = newValue
            };

            var settingEntity = _context.Settings.Single(x => x.Name == setting.Name);
            settingEntity.Value = setting.Value;

            _settings[name] = newValue;

            SettingChanged?.Invoke(this, new SettingEventArgs
            {
                SettingName = name
            });
        }

        /// <summary>
        /// Saves setting changes.
        /// </summary>
        public void Commit()
        {
            _context.SaveChanges();
        }

        #endregion
    }
}
