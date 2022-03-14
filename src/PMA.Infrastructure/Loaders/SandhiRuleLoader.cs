// <copyright file="SandhiRuleLoader.cs" company="Andrey Pospelov">
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
using PMA.Infrastructure.Enums;
using PMA.Infrastructure.Extensions;
using PMA.Infrastructure.Loaders.Base;
using PMA.Infrastructure.Models;
using PMA.Utils.Exceptions;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Infrastructure.Loaders
{
    /// <summary>
    /// The sandhi rule loader class.
    /// </summary>
    public sealed class SandhiRuleLoader : DbLoaderBase<SandhiRuleLoader>, ISandhiRuleLoader
    {
        /// <summary>
        /// The raw sandhi rule data.
        /// </summary>
        private DataTable _rawSandhiRuleData;

        /// <summary>
        /// The raw sandhi var data.
        /// </summary>
        private DataTable _rawSandhiVarData;

        /// <summary>
        /// The raw sandhi result data.
        /// </summary>
        private DataTable _rawSandhiResultData;

        /// <summary>
        /// Initializes the new instance of <see cref="SandhiRuleLoader"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public SandhiRuleLoader(IServiceLocator serviceLocator, SqLiteDbContext context, IMapperBase mapper, ILogger<SandhiRuleLoader> logger) : base(serviceLocator, context, mapper, logger)
        {
        }

        #region Overrides of LoaderBase<SandhiRuleLoader>

        /// <summary>
        /// Reads raw data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override bool ReadRawData()
        {
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DataFileReading, ExcelTableNameConstants.SandhiRules));

            try
            {
                _rawSandhiRuleData = ServiceLocator.ExcelDataService.GetData(ExcelTableNameConstants.SandhiRules);
            }
            catch (CustomException exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DataFileTableNotFoundByName, ExcelTableNameConstants.SandhiRules));

                Logger.LogError(exception.Message);
                return false;
            }

            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DataFileReading, ExcelTableNameConstants.SandhiVars));

            try
            {
                _rawSandhiVarData = ServiceLocator.ExcelDataService.GetData(ExcelTableNameConstants.SandhiVars);
            }
            catch (CustomException exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DataFileTableNotFoundByName, ExcelTableNameConstants.SandhiVars));

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
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DataTableValidating, _rawSandhiRuleData.TableName));

            bool result = ValidateSandhiRuleDataTable();

            if (!result) return false;

            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DataTableValidating, _rawSandhiVarData.TableName));

            result = ValidateSandhiVarDataTable();

            if (!result) return false;

            result = ValidateDataTableValues();

            if (!result) return false;

            ClearSandhiRuleDataTable();

            ReplaceVars();

            result = ValidateRegexValues();

            if (!result) return false;

            CreateSandhiResultDataTable();

            return true;
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
            // Remove SandhiResults
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DbTableDataRemoving, DbTableNameConstants.SandhiResults));

            try
            {
                Context.SandhiResults.RemoveRange(Context.SandhiResults);
                Context.ResetPrimaryKeySequence(DbTableNameConstants.SandhiResults);
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DbTableError, exception.Message));

                Logger.LogError(exception.Message);
                return false;
            }

            // Remove SandhiRules
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DbTableDataRemoving, DbTableNameConstants.SandhiRules));

            try
            {
                Context.SandhiRules.RemoveRange(Context.SandhiRules);
                Context.ResetPrimaryKeySequence(DbTableNameConstants.SandhiRules);
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DbTableError, exception.Message));

                Logger.LogError(exception.Message);
                return false;
            }

            // Add SandhiRules
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DbTableDataAdding, DbTableNameConstants.SandhiRules));

            try
            {
                var sandhiRuleEntities = _rawSandhiRuleData.AsEnumerable().Select(Mapper.Map<SandhiRuleEntity>).ToList();
                Context.SandhiRules.AddRange(sandhiRuleEntities);
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DbTableError, exception.Message));

                Logger.LogError(exception.Message);
                return false;
            }

            // Add SandhiResults
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DbTableDataAdding, DbTableNameConstants.SandhiResults));

            try
            {
                var sandhiResultEntities = _rawSandhiResultData.AsEnumerable().Select(Mapper.Map<SandhiResultEntity>).ToList();
                Context.SandhiResults.AddRange(sandhiResultEntities);
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
            if (_rawSandhiRuleData is not null)
            {
                _rawSandhiRuleData.Dispose();
                _rawSandhiRuleData = null;
            }

            if (_rawSandhiVarData is not null)
            {
                _rawSandhiVarData.Dispose();
                _rawSandhiVarData = null;
            }

            if (_rawSandhiRuleData is not null)
            {
                _rawSandhiRuleData.Dispose();
                _rawSandhiRuleData = null;
            }
        }

        #endregion

        /// <summary>
        /// Validates the sandhi rule data table.
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool ValidateSandhiRuleDataTable()
        {
            var result = _rawSandhiRuleData.Validate(ExcelTableStructureConstants.SandhiRules);

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
        /// Validates the sandhi var data table.
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool ValidateSandhiVarDataTable()
        {
            var result = _rawSandhiVarData.Validate(ExcelTableStructureConstants.SandhiVars);

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
        /// Validates the data table values.
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool ValidateDataTableValues()
        {
            var result = _rawSandhiRuleData.ValidateEnumValues<SandhiDirection>("Direction");

            result.AddRange(ConvertSandhiGroupNames());

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
        /// Converts sandhi group name to ID. 
        /// </summary>
        /// <returns>A collection of error messages, if any.</returns>
        private IEnumerable<RawLogMessage> ConvertSandhiGroupNames()
        {
            var rawLogMessages = new List<RawLogMessage>();

            const string columnName = "Group";

            var sandhiGroupEntities = Context.SandhiGroups.ToList();

            foreach (DataRow row in _rawSandhiRuleData.Rows)
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
        /// Clears the sandhi rule data table.
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private void ClearSandhiRuleDataTable()
        {
            string reverseDirection = SandhiDirection.Reverse.ToString().ToLower();

            for (int i = 0; i < _rawSandhiRuleData.Rows.Count; i++)
            {
                var row = _rawSandhiRuleData.Rows[i];

                if (row["Direction"].ToString().ToLower() == reverseDirection && row["IsActive"].ToString().ToLower() == "true") continue;

                _rawSandhiRuleData.Rows.Remove(row);
                i--;
            }

            _rawSandhiRuleData.Columns.Remove(_rawSandhiRuleData.Columns["Direction"]);
            _rawSandhiRuleData.Columns.Remove(_rawSandhiRuleData.Columns["IsActive"]);

            _rawSandhiRuleData.MergeColumns(new[] { "RegexLeft", "RegexBody", "RegexRight" }, "Regex");
        }

        /// <summary>
        /// Replaces sandhi vars in the sandhi rule data table.
        /// </summary>
        private void ReplaceVars()
        {
            foreach (DataRow sandhiVar in _rawSandhiVarData.Rows)
            {
                foreach (DataRow sandhiRule in _rawSandhiRuleData.Rows)
                {
                    sandhiRule["Regex"] = ((string)sandhiRule["Regex"]).Replace((string)sandhiVar["Name"], (string)sandhiVar["Regex"]);
                    sandhiRule["RegexResult"] = ((string)sandhiRule["RegexResult"]).Replace((string)sandhiVar["Name"], (string)sandhiVar["RegexResult"]);
                }
            }
        }

        /// <summary>
        /// Validates regex values in the data table.
        /// </summary>
        /// <returns>The result of operation.</returns>
        private bool ValidateRegexValues()
        {
            var result = _rawSandhiRuleData.ValidateRegexValues("Regex");

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
        /// Creates the sandhi result data table from the sandhi rule data table.
        /// </summary>
        private void CreateSandhiResultDataTable()
        {
            _rawSandhiResultData = _rawSandhiRuleData.Copy();

            _rawSandhiRuleData.Columns.Remove("RegexResult");

            _rawSandhiResultData.TableName = DbTableNameConstants.SandhiResults;
            _rawSandhiResultData.Columns.Remove("Group");
            _rawSandhiResultData.Columns.Remove("Regex");
            _rawSandhiResultData.Columns.Remove("Description");

            bool hasNewRows;

            do
            {
                var rgx = new Regex(@"\(.*?\)");
                var newRows = new List<DataRow>();

                foreach (DataRow row in _rawSandhiResultData.Rows)
                {
                    string value = (string)row["RegexResult"];

                    var match = rgx.Match(value);

                    if (!match.Success) continue;

                    string[] values = match.Value.Substring(1, match.Value.Length - 2).Split('|');

                    row["RegexResult"] = rgx.Replace(value, values[0], 1);

                    for (int i = 1; i < values.Length; i++)
                    {
                        var newRow = _rawSandhiResultData.NewRow();
                        newRow["Id"] = row["Id"];
                        newRow["RegexResult"] = rgx.Replace(value, values[i], 1);
                        newRows.Add(newRow);
                    }
                }

                foreach (var newRow in newRows)
                {
                    _rawSandhiResultData.Rows.Add(newRow);
                }

                hasNewRows = newRows.Count > 0;
            } while (hasNewRows);
        }
    }
}
