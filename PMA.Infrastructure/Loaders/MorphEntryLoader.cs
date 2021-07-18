// <copyright file="MorphEntryLoader.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Domain.Constants;
using PMA.Domain.Enums;
using PMA.Domain.Exceptions;
using PMA.Domain.Interfaces.Loaders;
using PMA.Domain.Interfaces.Locators;
using PMA.Domain.Models;
using PMA.Infrastructure.Constants;
using PMA.Infrastructure.DbContexts;
using PMA.Infrastructure.Extensions;
using PMA.Infrastructure.Loaders.Base;
using PMA.Infrastructure.Models;
using PMA.Utils.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PMA.Infrastructure.Loaders
{
    /// <summary>
    /// The morphological entry loader class.
    /// </summary>
    public sealed class MorphEntryLoader : MemoryLoaderBase<MorphEntryLoader, IList<MorphEntry>>, IMorphEntryLoader
    {
        /// <summary>
        /// The raw data.
        /// </summary>
        private DataTable _rawData;

        /// <summary>
        /// Initializes the new instance of <see cref="MorphEntryLoader"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public MorphEntryLoader(IServiceLocator serviceLocator, SqLiteDbContext context, IMapperBase mapper, ILogger<MorphEntryLoader> logger) : base(serviceLocator, context, mapper, logger)
        {
            Logger.LogInit();
        }

        #region Overrides of LoaderBase<MorphEntryLoader>

        /// <summary>
        /// Reads raw data.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override bool ReadRawData()
        {
            ServiceLocator.LogMessageService.SendMessage(ServiceLocator.TranslateService.Translate(LogMessageType.DataFileReading, ExcelTableNameConstants.MorphEntries));

            try
            {
                _rawData = ServiceLocator.ExcelDataService.GetData(ExcelTableNameConstants.MorphEntries);
            }
            catch (CustomException exception)
            {
                ServiceLocator.LogMessageService.SendMessage(MessageLevel.Error, ServiceLocator.TranslateService.Translate(LogMessageType.DataFileTableNotFoundByName, ExcelTableNameConstants.MorphEntries));

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

            result = ValidateDataTableValues();

            return result;
        }

        /// <summary>
        /// Loads mapping data to database.
        /// </summary>
        /// <returns>The result of operation.</returns>
        public override IList<MorphEntry> GetData()
        {
            return _rawData.AsEnumerable().Select(Mapper.Map<MorphEntry>).ToList();
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
            var result = _rawData.Validate(ExcelTableStructureConstants.MorphEntries);

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
            var result = _rawData.ValidateEnumValues<MorphBase>("Base");

            result.AddRange(ValidateValuesByTerm());

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
        /// Validates data table values by terms.
        /// </summary>
        /// <returns>A collection of error messages, if any.</returns>
        private IEnumerable<RawLogMessage> ValidateValuesByTerm()
        {
            string[] columnNames =
            {
                "Language", "Part", "PoS1", "PoS2", "PoS3", "Tense", "Voice", "Gender", "Mode", "Person", "Number",
                "Case", "IsIrregular", "IsName", "IsNegative", "WithAugment", "Formation", "Parent", "Type1", "Type2",
                "Type3"
            };

            string unknownValue = Context.Terms.Find(MorphConstants.UnknownTermId).Entry;

            var rawLogMessages = new List<RawLogMessage>();

            foreach (DataRow row in _rawData.Rows)
            {
                for (int i = 0; i < columnNames.Length; i++)
                {
                    string columnName = columnNames[i];
                    string value = (string)row[columnName];

                    if (string.IsNullOrEmpty(value))
                    {
                        value = unknownValue;
                    }

                    var term = Context.Terms.SingleOrDefault(x => x.Entry == value);

                    if (term is null)
                    {
                        rawLogMessages.Add(new RawLogMessage
                        {
                            Type = LogMessageType.DataTableIncorrectRowValue,
                            Parameters = new object[] { (string)row[columnName], (string)row["Id"], columnName }
                        });
                    }
                    else
                    {
                        row[columnName] = term.Id;
                    }
                }
            }

            return rawLogMessages;
        }
    }
}
