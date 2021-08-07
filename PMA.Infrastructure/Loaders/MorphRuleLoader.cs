// <copyright file="MorphRuleLoader.cs" company="Andrey Pospelov">
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PMA.Infrastructure.Loaders
{
    /// <summary>
    /// The morphological rule loader class.
    /// </summary>
    public sealed class MorphRuleLoader : DbLoaderBase<MorphRuleLoader>, IMorphRuleLoader
    {
        /// <summary>
        /// Options that configure the operation of methods on the <see cref="Parallel"/> class.
        /// </summary>
        private readonly ParallelOptions _parallelOptions;

        /// <summary>
        /// The raw data.
        /// </summary>
        private DataTable _rawData;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphRuleLoader"/> class.
        /// </summary>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public MorphRuleLoader(ParallelOptions parallelOptions, IServiceLocator serviceLocator, SqLiteDbContext context, IMapperBase mapper, ILogger<MorphRuleLoader> logger) : base(serviceLocator, context, mapper, logger)
        {
            _parallelOptions = parallelOptions;

            Logger.LogInit();
        }

        #region Overrides of LoaderBase<MorphRuleLoader>

        /// <summary>
        /// Reads raw data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override bool ReadRawData()
        {
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DataFileReading, ExcelTableNameConstants.MorphRules));

            try
            {
                _rawData = ServiceLocator.ExcelDataService.GetData(ExcelTableNameConstants.MorphRules);
            }
            catch (CustomException exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DataFileTableNotFoundByName, ExcelTableNameConstants.MorphRules));

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

            bool result = ValidateDataTableStructure();

            if (!result) return false;

            result = ValidateDataTableValuesByPrimaryRules();

            if (!result) return false;

            ClearDataTable();

            SplitRows();

            result = SearchDuplicates();

            if (!result) return false;

            result = ValidateDataTableValuesBySecondaryRules();

            if (!result) return false;

            result = ValidateDataTableValuesByTertiaryRules();

            return result;
        }

        /// <summary>
        /// Loads mapping data to database.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override bool LoadData()
        {
            // Remove MorphRules
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DbTableDataRemoving, DbTableNameConstants.MorphRules));

            try
            {
                Context.MorphRules.RemoveRange(Context.MorphRules);
                Context.ResetPrimaryKeySequence(DbTableNameConstants.MorphRules);
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DbTableError, exception.Message));

                Logger.LogError(exception.Message);
                return false;
            }

            // Add MorphRules
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DbTableDataAdding, DbTableNameConstants.MorphRules));

            try
            {
                var morphRuleEntities = _rawData.AsEnumerable().Select(Mapper.Map<MorphRuleEntity>).ToList();
                Context.MorphRules.AddRange(morphRuleEntities);
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
        /// Validates the data table structure.
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool ValidateDataTableStructure()
        {
            var result = _rawData.Validate(ExcelTableStructureConstants.MorphRules);

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
        /// Validates the data table values by primary rules
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool ValidateDataTableValuesByPrimaryRules()
        {
            var result = ValidateSandhiGroupValues();

            result.AddRange(_rawData.ValidateEnumValues<MorphBase>("Base"));
            result.AddRange(_rawData.ValidateEnumValues<MorphRuleType>("LeftType", new object[] { MorphRuleType.None }));
            result.AddRange(_rawData.ValidateEnumValues<MorphRuleType>("RightType"));

            result.AddRange(ValidateInfoColumnValues());
            result.AddRange(ValidateInfoColumnValues("Left"));
            result.AddRange(ValidateInfoColumnValues("Right"));

            if (result.Any())
            {
                foreach (string message in result.Select(rawLogMessage => ServiceLocator.TranslateService.Translate(rawLogMessage.Type, rawLogMessage.Parameters)))
                {
                    ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, message);
                }

                Logger.LogError(ErrorMessageConstants.DataTableValuesAreNotValid);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the data table values by secondary rules
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool ValidateDataTableValuesBySecondaryRules()
        {
            var result = ValidateValuesByTerm();

            result.AddRange(ValidateValuesByTerm("Left"));
            result.AddRange(ValidateValuesByTerm("Right"));

            if (result.Any())
            {
                foreach (string message in result.Select(rawLogMessage => ServiceLocator.TranslateService.Translate(rawLogMessage.Type, rawLogMessage.Parameters)))
                {
                    ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, message);
                }

                Logger.LogError(ErrorMessageConstants.DataTableValuesAreNotValid);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the data table values by tertiary rules
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool ValidateDataTableValuesByTertiaryRules()
        {
            var result = ValidateValuesByMorphCombinations();

            result.AddRange(ValidateValuesByMorphCombinations("Left"));
            result.AddRange(ValidateValuesByMorphCombinations("Right"));

            if (result.Any())
            {
                foreach (string message in result.Select(rawLogMessage => ServiceLocator.TranslateService.Translate(rawLogMessage.Type, rawLogMessage.Parameters)))
                {
                    ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, message);
                }

                Logger.LogError(ErrorMessageConstants.DataTableValuesAreNotValid);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clears a data table.
        /// </summary>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private void ClearDataTable()
        {
            _rawData.Columns.Remove(_rawData.Columns["Group"]);
            _rawData.Columns.Remove(_rawData.Columns["Info"]);
            _rawData.Columns.Remove(_rawData.Columns["LeftInfo"]);
            _rawData.Columns.Remove(_rawData.Columns["RightInfo"]);

            for (int i = 0; i < _rawData.Rows.Count; i++)
            {
                var row = _rawData.Rows[i];

                if (row["IsActive"].ToString().ToLower() == "true") continue;

                _rawData.Rows.Remove(row);
                i--;
            }

            _rawData.Columns.Remove(_rawData.Columns["IsActive"]);
        }

        /// <summary>
        /// Splits data table rows by delimiter '|'.
        /// </summary>
        private void SplitRows()
        {
            var splitColumns = new List<string> { "Label", "LeftLabel", "LeftEntry", "RightLabel", "RightEntry" };

            for (int i = 0; i < MorphConstants.ParameterCount; i++)
            {
                splitColumns.Add(i.ToString());
                splitColumns.Add("Left" + i);
                splitColumns.Add("Right" + i);
            }

            _rawData.SplitRows(splitColumns);
        }

        /// <summary>
        /// Validates sandhi group values.
        /// </summary>
        /// <returns>A collection of error messages, if any.</returns>
        private List<RawLogMessage> ValidateSandhiGroupValues()
        {
            const string columnName = "Sandhi";

            var sandhiGroupEntities = Context.SandhiGroups.ToList();

            var rawLogMessages = new List<RawLogMessage>();

            foreach (DataRow row in _rawData.Rows)
            {
                var sandhiGroupEntity = sandhiGroupEntities.SingleOrDefault(x => x.Entry == row[columnName].ToString());

                if (sandhiGroupEntity is null)
                {
                    rawLogMessages.Add(new RawLogMessage
                    {
                        Type = LogMessageType.DataTableIncorrectRowValue,
                        Parameters = new object[] { (string)row[columnName], (string)row["Id"], columnName }
                    });
                }
                else
                {
                    row[columnName] = sandhiGroupEntity.Id;
                }
            }

            return rawLogMessages;
        }

        /// <summary>
        /// Validates Info column values.
        /// </summary>
        /// <param name="prefix">The info column prefix.</param>
        /// <returns>A collection of error messages, if any.</returns>
        private IEnumerable<RawLogMessage> ValidateInfoColumnValues(string prefix = "")
        {
            var rawLogMessages = new List<RawLogMessage>();

            string columnName = prefix + "Info";

            var morphParameterNames = Context.MorphParameters
                .Select(x => x.Id)
                .ToList()
                .Select(id => ServiceLocator.TranslateService.Translate($"MorphParameters[{id}].Name"))
                .ToList();

            for (int i = 0; i < MorphConstants.ParameterCount; i++)
            {
                _rawData.Columns.Add(new DataColumn(prefix + i));
            }

            var rgx = new Regex(@"\A(.+)\=""(.+)""\z");

            foreach (DataRow row in _rawData.Rows)
            {
                string rowString = (string)row[columnName];

                if (!string.IsNullOrEmpty(rowString))
                {
                    string[] parameters = rowString.Split(',');

                    foreach (string parameter in parameters)
                    {
                        var match = rgx.Match(parameter);
                        if (match.Success)
                        {
                            string column = match.Groups[1].Value;
                            string value = match.Groups[2].Value;

                            int index = morphParameterNames.IndexOf(column);
                            if (index == -1)
                            {
                                rawLogMessages.Add(new RawLogMessage
                                {
                                    Type = LogMessageType.DataTableIncorrectRowValue,
                                    Parameters = new object[] { (string)row[columnName], (string)row["Id"], columnName }
                                });
                            }
                            else
                            {
                                row[prefix + index] = value;
                            }
                        }
                        else
                        {
                            rawLogMessages.Add(new RawLogMessage
                            {
                                Type = LogMessageType.DataTableIncorrectRowValue,
                                Parameters = new object[] { (string)row[columnName], (string)row["Id"], columnName }
                            });
                        }
                    }
                }

                for (int i = 0; i < MorphConstants.ParameterCount; i++)
                {
                    if (row[prefix + i] == DBNull.Value)
                    {
                        row[prefix + i] = string.Empty;
                    }
                }
            }

            return rawLogMessages;
        }

        /// <summary>
        /// Searches duplicates in the data table.
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool SearchDuplicates()
        {
            var columnNames = new List<string> { "Label", "Sandhi", "Entry", "Base", "LeftLabel", "LeftEntry", "RightLabel", "RightEntry" };

            for (int i = 0; i < MorphConstants.ParameterCount; i++)
            {
                columnNames.Add(i.ToString());
                columnNames.Add("Left" + i);
                columnNames.Add("Right" + i);
            }

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
        /// <param name="prefix">The info column prefix.</param>
        /// <returns>A collection of error messages, if any.</returns>
        private List<RawLogMessage> ValidateValuesByTerm(string prefix = "")
        {
            var termEntities = Context.Terms.ToList();

            string unknownValue = termEntities.Single(x => x.Id == MorphConstants.UnknownTermId).Entry;

            var rawLogMessages = new List<RawLogMessage>();

            foreach (DataRow row in _rawData.Rows)
            {
                for (int i = 0; i < MorphConstants.ParameterCount; i++)
                {
                    string columnName = prefix + i;
                    string value = (string)row[columnName];

                    if (string.IsNullOrEmpty(value))
                    {
                        value = unknownValue;
                    }

                    var term = termEntities.SingleOrDefault(x => x.Entry == value);

                    if (term is null)
                    {
                        object[] parameters = { (string)row[columnName], (string)row["Id"], prefix + "Info" };

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

            return rawLogMessages;
        }

        /// <summary>
        /// Validates data table values by morphological combinations.
        /// </summary>
        /// <param name="prefix">The info column prefix.</param>
        /// <returns>A collection of error messages, if any.</returns>
        private List<RawLogMessage> ValidateValuesByMorphCombinations(string prefix = "")
        {
            var rawLogMessages = new ConcurrentBag<RawLogMessage>();

            var morphCombinations = Context.MorphCombinations.ToList();

            var type = typeof(MorphCombinationEntity);
            string unknownTermId = MorphConstants.UnknownTermId.ToString();
            string alternativeUnknownTermId = MorphConstants.AlternativeUnknownTermId.ToString();

            Parallel.ForEach(_rawData.AsEnumerable(), _parallelOptions, row =>
            {
                var filteredMorphCombinations = morphCombinations;

                for (int i = 0; i < MorphConstants.ParameterCount; i++)
                {
                    string columnName = prefix + i;
                    string value = (string)row[columnName];

                    if (value != unknownTermId && value != alternativeUnknownTermId)
                    {
                        filteredMorphCombinations = filteredMorphCombinations.Where(x => type.GetProperty("Parameter" + i)?.GetValue(x)?.ToString() == value).ToList();
                    }

                    if (filteredMorphCombinations.Any()) continue;

                    object[] parameters = { (string)row["Id"], prefix + "Info" };

                    if (!rawLogMessages.Any(x =>
                        x.Parameters[0] == parameters[0] &&
                        x.Parameters[1] == parameters[1]))
                    {
                        rawLogMessages.Add(new RawLogMessage
                        {
                            Type = LogMessageType.DataTableMorphCombinationValidationFailed,
                            Parameters = parameters
                        });
                    }

                    break;
                }
            });

            return rawLogMessages.ToList();
        }
    }
}
