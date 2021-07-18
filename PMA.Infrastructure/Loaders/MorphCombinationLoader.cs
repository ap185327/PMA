// <copyright file="MorphCombinationLoader.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Constants;
using PMA.Domain.Enums;
using PMA.Domain.Exceptions;
using PMA.Domain.Interfaces.Loaders;
using PMA.Domain.Interfaces.Locators;
using PMA.Infrastructure.Constants;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Entities;
using PMA.Infrastructure.Extensions;
using PMA.Infrastructure.Loaders.Base;
using PMA.Infrastructure.Models;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PMA.Infrastructure.Loaders
{
    /// <summary>
    /// The morphological combination loader class.
    /// </summary>
    public sealed class MorphCombinationLoader : DbLoaderBase<MorphCombinationLoader>, IMorphCombinationLoader
    {
        /// <summary>
        /// The raw data.
        /// </summary>
        private DataTable _rawData;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphCombinationLoader"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public MorphCombinationLoader(IServiceLocator serviceLocator, SqLiteDbContext context, IMapperBase mapper, ILogger<MorphCombinationLoader> logger) : base(serviceLocator, context, mapper, logger)
        {
            Logger.LogInit();
        }

        #region Overrides of LoaderBase<MorphCombinationLoader>

        /// <summary>
        /// Reads raw data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override bool ReadRawData()
        {
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DataFileReading, ExcelTableNameConstants.MorphCombinations));

            try
            {
                _rawData = ServiceLocator.ExcelDataService.GetData(ExcelTableNameConstants.MorphCombinations);
            }
            catch (CustomException exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DataFileTableNotFoundByName, ExcelTableNameConstants.MorphCombinations));

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

            if (!result) return false;

            var columnNames = _rawData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Skip(1).ToList();

            SplitRows(columnNames);

            result = SearchDuplicates(columnNames);

            if (!result) return false;

            result = ValidateValuesByTerm(columnNames);

            return result;
        }

        /// <summary>
        /// Loads mapping data to database.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override bool LoadData()
        {
            // Remove MorphCombinations
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DbTableDataRemoving, DbTableNameConstants.MorphCombinations));

            try
            {
                Context.MorphCombinations.RemoveRange(Context.MorphCombinations);
                Context.ResetPrimaryKeySequence(DbTableNameConstants.MorphCombinations);
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DbTableError, exception.Message));

                Logger.LogError(exception.Message);
                return false;
            }

            // Add MorphCombinations
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DbTableDataAdding, DbTableNameConstants.MorphCombinations));

            try
            {
                var morphCombinationEntities = _rawData.AsEnumerable().Select(Mapper.Map<MorphCombinationEntity>).ToList();
                Context.MorphCombinations.AddRange(morphCombinationEntities);
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
            var result = _rawData.Validate(ExcelTableStructureConstants.MorphCombinations);

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

        /// <summary>
        /// Splits data table rows by delimiter '|'.
        /// </summary>
        /// <param name="columnNames">The collection of key column names.</param>
        private void SplitRows(IEnumerable<string> columnNames)
        {
            _rawData.SplitRows(columnNames);
        }

        /// <summary>
        /// Searches duplicates in the data table.
        /// </summary>
        /// <param name="columnNames">The collection of key column names.</param>
        /// <returns>The result of operation.</returns>
        private bool SearchDuplicates(IList<string> columnNames)
        {
            var duplicates = _rawData.GetDuplicates(columnNames, _rawData.Columns[0].ColumnName);

            if (duplicates.Any())
            {
                foreach (string duplicateId in duplicates)
                {
                    ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DataTableDuplicateFound, _rawData.TableName, duplicateId));
                }

                Logger.LogError(ErrorMessageConstants.DataTableContainsDuplicates);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates data table values by terms.
        /// </summary>
        /// <param name="columnNames">A collection of columns for the validation.</param>
        /// <returns>The result of operation.</returns>
        private bool ValidateValuesByTerm(IList<string> columnNames)
        {
            var termEntities = Context.Terms.ToList();

            string unknownValue = termEntities.Single(x => x.Id == MorphConstants.UnknownTermId).Entry;

            var rawLogMessages = new List<RawLogMessage>();

            foreach (DataRow row in _rawData.Rows)
            {
                for (int i = 0; i < columnNames.Count; i++)
                {
                    string columnName = columnNames[i];
                    string value = (string)row[columnName];

                    if (string.IsNullOrEmpty(value))
                    {
                        value = unknownValue;
                    }

                    var term = termEntities.SingleOrDefault(x => x.Entry == value);

                    if (term is null)
                    {
                        object[] parameters = { (string)row[columnName], (string)row["Id"], columnName };

                        if (!rawLogMessages.Any(x =>
                            x.Parameters[0] == parameters[0] &&
                            x.Parameters[1] == parameters[1] &&
                            x.Parameters[2] == parameters[2]))
                        {
                            rawLogMessages.Add(new RawLogMessage
                            {
                                Type = LogMessageType.DataTableIncorrectRowValue,
                                Parameters = parameters
                            });
                        }
                    }
                    else
                    {
                        row[columnName] = term.Id;
                    }
                }
            }

            if (rawLogMessages.Any())
            {
                foreach (string message in rawLogMessages.Select(rawLogMessage => ServiceLocator.TranslateService.Translate(rawLogMessage.Type, rawLogMessage.Parameters)))
                {
                    ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, message);
                }

                Logger.LogError(ErrorMessageConstants.DataTableValuesAreNotValid);
                return false;
            }

            return true;
        }
    }
}