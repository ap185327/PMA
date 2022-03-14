// <copyright file="SandhiGroupLoader.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Constants;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Loaders;
using PMA.Domain.Interfaces.Locators;
using PMA.Infrastructure.Constants;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Entities;
using PMA.Infrastructure.Extensions;
using PMA.Infrastructure.Loaders.Base;
using PMA.Utils.Exceptions;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Infrastructure.Loaders
{
    /// <summary>
    /// The sandhi group loader class.
    /// </summary>
    public sealed class SandhiGroupLoader : DbLoaderBase<SandhiGroupLoader>, ISandhiGroupLoader
    {
        /// <summary>
        /// The raw data.
        /// </summary>
        private DataTable _rawData;

        /// <summary>
        /// Initializes the new instance of <see cref="SandhiGroupLoader"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public SandhiGroupLoader(IServiceLocator serviceLocator, SqLiteDbContext context, IMapperBase mapper, ILogger<SandhiGroupLoader> logger) : base(serviceLocator, context, mapper, logger)
        {
        }

        #region Overrides of LoaderBase<SandhiGroupLoader>

        /// <summary>
        /// Reads raw data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override bool ReadRawData()
        {
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DataFileReading, ExcelTableNameConstants.SandhiGroups));

            try
            {
                _rawData = ServiceLocator.ExcelDataService.GetData(ExcelTableNameConstants.SandhiGroups);
            }
            catch (CustomException exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DataFileTableNotFoundByName, ExcelTableNameConstants.SandhiGroups));

                Logger.LogError(exception.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates and formats raw data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override bool ValidateAndFormatRawData()
        {
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DataTableValidating, _rawData.TableName));

            bool result = ValidateDataTable();

            return result;
        }

        /// <summary>
        /// Validates and formats raw data.
        /// </summary>
        /// <returns>True if the raw data has been validated; otherwise - False.</returns>
        public override Task<bool> ValidateAndFormatRawDataAsync(CancellationToken token = default)
        {
            return Task.FromResult(ValidateAndFormatRawData());
        }

        /// <summary>
        /// Loads mapping data to database.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override bool LoadData()
        {
            // Update SandhiGroups
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DbTableDataUpdating, DbTableNameConstants.SandhiGroups));

            try
            {
                var newSandhiGroupEntities = _rawData.AsEnumerable().Select(Mapper.Map<SandhiGroupEntity>).ToList();

                var sandhiGroupEntities = Context.SandhiGroups.ToList();

                foreach (var newSandhiGroupEntity in newSandhiGroupEntities)
                {
                    var sandhiGroupEntity = sandhiGroupEntities.SingleOrDefault(x => x.Id == newSandhiGroupEntity.Id);

                    if (sandhiGroupEntity is null)
                    {
                        Context.SandhiGroups.Add(newSandhiGroupEntity);
                    }
                    else
                    {
                        sandhiGroupEntity.Entry = newSandhiGroupEntity.Entry;
                    }
                }

                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DbTableError, exception.Message));

                Logger.LogError(exception.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clears temp data.
        /// </summary>
        public override void Clear()
        {
            if (_rawData is not null)
            {
                _rawData.Dispose();
                _rawData = null;
            }
        }

        #endregion

        /// <summary>
        /// Validates the data table.
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool ValidateDataTable()
        {
            var result = _rawData.Validate(ExcelTableStructureConstants.SandhiGroups);

            if (result.Any())
            {
                foreach (string message in result.Select(rawLogMessage => ServiceLocator.TranslateService.Translate(rawLogMessage.Type, rawLogMessage.Parameters)))
                {
                    ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, message);
                }

                Logger.LogError(ErrorMessageConstants.DataTableStructureIsNotValid);
                return false;
            }

            return true;
        }
    }
}
