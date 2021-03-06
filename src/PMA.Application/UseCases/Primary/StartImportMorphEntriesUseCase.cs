// <copyright file="StartImportMorphEntriesUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.Factories;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Secondary;
using PMA.Domain.Interfaces.Loaders;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Models;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The start import morphological entries use case class.
    /// </summary>
    public sealed class StartImportMorphEntriesUseCase : UseCaseBase<StartImportMorphEntriesUseCase, ImportMorphEntryInputPort>, IStartImportMorphEntriesUseCase
    {
        /// <summary>
        /// The morphological parser interactor.
        /// </summary>
        private readonly IMorphParserInteractor _interactor;

        /// <summary>
        /// The morphological entry database provider.
        /// </summary>
        private readonly IMorphEntryDbProvider _morphEntryDbProvider;

        /// <summary>
        /// The morphological entry manager.
        /// </summary>
        private readonly IMorphEntryManager _morphEntryManager;

        /// <summary>
        /// The morphological combination manager.
        /// </summary>
        private readonly IMorphCombinationManager _morphCombinationManager;

        /// <summary>
        /// The morphological entry loader.
        /// </summary>
        private readonly IMorphEntryLoader _morphEntryLoader;

        /// <summary>
        /// The translate service.
        /// </summary>
        private readonly ITranslateService _translateService;

        /// <summary>
        /// The log message service.
        /// </summary>
        private readonly ILogMessageService _logMessageService;

        /// <summary>
        /// The excel data service.
        /// </summary>
        private readonly IExcelDataService _excelDataService;

        /// <summary>
        /// The mediator.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of <see cref="StartImportMorphEntriesUseCase"/> class.
        /// </summary>
        /// <param name="interactor">The morphological parser interactor.</param>
        /// <param name="morphEntryDbProvider">The morphological entry database provider.</param>
        /// <param name="morphEntryManager">The morphological entry manager.</param>
        /// <param name="morphCombinationManager">The morphological combination manager.</param>
        /// <param name="morphEntryLoader">The morphological entry loader.</param>
        /// /// <param name="translateService">The translate service.</param>
        /// <param name="logMessageService">The log message service.</param>
        /// <param name="excelDataService">The excel data service.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public StartImportMorphEntriesUseCase(IMorphParserInteractor interactor,
            IMorphEntryDbProvider morphEntryDbProvider,
            IMorphEntryManager morphEntryManager,
            IMorphCombinationManager morphCombinationManager,
            IMorphEntryLoader morphEntryLoader,
            ITranslateService translateService,
            ILogMessageService logMessageService,
            IExcelDataService excelDataService,
            IMediator mediator,
            ILogger<StartImportMorphEntriesUseCase> logger) : base(logger)
        {
            _interactor = interactor;
            _morphEntryDbProvider = morphEntryDbProvider;
            _morphEntryManager = morphEntryManager;
            _morphCombinationManager = morphCombinationManager;
            _morphEntryLoader = morphEntryLoader;
            _translateService = translateService;
            _logMessageService = logMessageService;
            _excelDataService = excelDataService;
            _mediator = mediator;
        }

        #region Overrides of UseCaseBase<StartImportMorphEntriesUseCase,ImportMorphEntryInputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(ImportMorphEntryInputPort inputData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override async Task<OperationResult> ExecuteAsync(ImportMorphEntryInputPort inputPort, CancellationToken token = default)
        {
            if (inputPort is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputPort));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputPort));
            }

            if (string.IsNullOrEmpty(inputPort.DataFilePath))
            {
                Logger.LogError(ErrorMessageConstants.FilePathIsEmpty);
                return OperationResult.FailureResult(ErrorMessageConstants.FilePathIsEmpty);
            }

            if (!File.Exists(inputPort.DataFilePath))
            {
                Logger.LogError(ErrorMessageConstants.FileNotFound, inputPort.DataFilePath);
                return OperationResult.FailureResult(ErrorMessageConstants.FileNotFound, inputPort.DataFilePath);
            }

            try
            {
                await _mediator.Publish(new ImportMorphEntryNotification
                {
                    State = ProcessState.InProgress,
                    AnalyzeProgressBarValue = 0
                }, token);

                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportStart));
                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.DataFileOpening));

                _excelDataService.Open(inputPort.DataFilePath);

                bool readRawDataResult = _morphEntryLoader.ReadRawData();

                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.DataFileClosing));

                _excelDataService.Close();

                if (readRawDataResult && await _morphEntryLoader.ValidateAndFormatRawDataAsync(token))
                {
                    var morphEntries = _morphEntryLoader.GetData();

                    for (int i = 0; i < morphEntries.Count; i++)
                    {
                        var morphEntry = morphEntries[i];

                        if (inputPort.IsAnalyzeBeforeImportChecked)
                        {
                            var parserInputData = new MorphParserInputPort
                            {
                                MorphEntry = morphEntries[i],
                                ParsingType = MorphParsingType.Import
                            };

                            var result = await _interactor.ParseMorphEntryAsync(parserInputData, token);

                            if (!result.Success)
                            {
                                Logger.LogErrors(result.Messages);
                                return OperationResult.FailureResult(result.Messages);
                            }

                            result = await _interactor.RemoveSolutionsWithExcessiveDepthAsync(parserInputData, token);

                            if (!result.Success)
                            {
                                Logger.LogErrors(result.Messages);
                                return OperationResult.FailureResult(result.Messages);
                            }

                            result = _interactor.CollapseSolutionsAsync(parserInputData, token).Result;

                            if (!result.Success)
                            {
                                Logger.LogErrors(result.Messages);
                                return OperationResult.FailureResult(result.Messages);
                            }

                            result = await _interactor.RemoveUnsuitableDerivativeSolutionsAsync(parserInputData, token);

                            if (!result.Success)
                            {
                                Logger.LogErrors(result.Messages);
                                return OperationResult.FailureResult(result.Messages);
                            }

                            result = await _interactor.UpdateSolutionsAsync(parserInputData, token);

                            if (!result.Success)
                            {
                                Logger.LogErrors(result.Messages);
                                return OperationResult.FailureResult(result.Messages);
                            }

                            result = await _interactor.RemoveDuplicatesAsync(parserInputData, token);

                            if (!result.Success)
                            {
                                Logger.LogErrors(result.Messages);
                                return OperationResult.FailureResult(result.Messages);
                            }

                            result = await _interactor.RemoveUnsuitableSolutionsAsync(parserInputData, token);

                            if (!result.Success)
                            {
                                Logger.LogErrors(result.Messages);
                                return OperationResult.FailureResult(result.Messages);
                            }
#if DEBUG
                            result = await _interactor.ValidateSolutionsAsync(parserInputData, token);

                            if (!result.Success)
                            {
                                Logger.LogErrors(result.Messages);
                                return OperationResult.FailureResult(result.Messages);
                            }
#endif
                            morphEntry = parserInputData.WordForm.GetMorphEntry();
                            morphEntry.Source = MorphEntrySource.ImportWithAnalysis;
                        }
                        else
                        {
                            morphEntry.Source = MorphEntrySource.ImportWithoutAnalysis;
                        }

                        TryToAddOrUpdate(morphEntry, i + 1, morphEntries[i].Entry);

                        if (inputPort.IsAnalyzeBeforeImportChecked)
                        {
                            _morphEntryDbProvider.Commit();
                        }

                        await _mediator.Publish(new ImportMorphEntryNotification
                        {
                            State = ProcessState.InProgress,
                            AnalyzeProgressBarValue = Convert.ToInt32(100 * (i + 1) / morphEntries.Count)
                        }, token);
                    }

                    if (!inputPort.IsAnalyzeBeforeImportChecked)
                    {
                        _morphEntryDbProvider.Commit();
                    }
                }

                await _mediator.Publish(new ImportMorphEntryNotification
                {
                    State = ProcessState.Completed,
                    AnalyzeProgressBarValue = 100
                }, token);

                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportCompleted));
                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                return OperationResult.SuccessResult();
            }
            catch (Exception exception)
            {
                switch (exception)
                {
                    case OperationCanceledException or AggregateException:
                        await _mediator.Publish(new ImportMorphEntryNotification
                        {
                            State = ProcessState.Canceled,
                            AnalyzeProgressBarValue = 0
                        }, CancellationToken.None);

                        _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportCanceled));
                        _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                        return OperationResult.SuccessResult();
                    case FileNotFoundException:
                        _logMessageService.SendMessage(MessageLevel.Error, _translateService.Translate(LogMessageType.DataFileDoesNotExist, inputPort.DataFilePath));

                        await _mediator.Publish(new ImportMorphEntryNotification
                        {
                            State = ProcessState.Completed,
                            AnalyzeProgressBarValue = 100
                        }, token);

                        _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportCompleted));
                        _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                        return OperationResult.SuccessResult();
                    default:
                        return OperationResult.ExceptionResult(exception);
                }
            }
        }

        #endregion

        /// <summary>
        /// Validates a morphological entry.
        /// </summary>
        /// <param name="morphEntry">A morphological entry.</param>
        /// <returns>An error.</returns>
        private string Validate(MorphEntry morphEntry)
        {
            string error = null;

            if (morphEntry.Id > 0 && _morphEntryDbProvider.GetValues().SingleOrDefault(x => x.Id == morphEntry.Id) is null)
            {
                error = _translateService.Translate(MorphEntryError.IdDoesNotExist, morphEntry.Id);
            }
            else if (string.IsNullOrEmpty(morphEntry.Entry))
            {
                error = _translateService.Translate(MorphEntryError.EntryIsEmpty);
            }
            else if (morphEntry.Parameters is null)
            {
                error = _translateService.Translate(MorphEntryError.ParametersAreEmpty);
            }
            else if (morphEntry.Left != null && morphEntry.Base == MorphBase.None)
            {
                error = _translateService.Translate(MorphEntryError.LeftDoesNotMatchBase, morphEntry.Left.Entry, morphEntry.Base);
            }
            else if (morphEntry.Right != null && morphEntry.Base == MorphBase.None)
            {
                error = _translateService.Translate(MorphEntryError.RightDoesNotMatchBase, morphEntry.Right.Entry, morphEntry.Base);
            }
            else if (!_morphCombinationManager.Check(morphEntry.Parameters))
            {
                error = _translateService.Translate(MorphEntryError.NoMorphCombinationMatches);
            }
            else
            {
                if (morphEntry.Left is not null)
                {
                    if (morphEntry.Left.Id == 0)
                    {
                        var leftIds = _morphEntryDbProvider.GetValues().Where(x => x.Entry == morphEntry.Left.Entry).Select(x => x.Id).ToList();

                        switch (leftIds.Count)
                        {
                            case 0:
                                error = _translateService.Translate(MorphEntryError.LeftEntryDoesNotExist, morphEntry.Left.Entry);
                                break;
                            case > 1:
                                error = _translateService.Translate(MorphEntryError.FoundMoreThanOneLeft, string.Join(",", leftIds));
                                break;
                            default:
                                morphEntry.Left.Id = leftIds[0];
                                break;
                        }
                    }
                    else
                    {
                        var leftEntry = _morphEntryDbProvider.GetValues().SingleOrDefault(x => x.Id == morphEntry.Left.Id);

                        if (leftEntry is null)
                        {
                            error = _translateService.Translate(MorphEntryError.LeftIdDoesNotExist, morphEntry.Left.Id);
                        }
                        else
                        {
                            morphEntry.Left = leftEntry;
                        }
                    }
                }

                if (string.IsNullOrEmpty(error) && morphEntry.Right is not null)
                {
                    if (morphEntry.Right.Id == 0)
                    {
                        var rightIds = _morphEntryDbProvider.GetValues().Where(x => x.Entry == morphEntry.Right.Entry).Select(x => x.Id).ToList();

                        switch (rightIds.Count)
                        {
                            case 0:
                                error = _translateService.Translate(MorphEntryError.RightEntryDoesNotExist, morphEntry.Right.Entry);
                                break;
                            case > 1:
                                error = _translateService.Translate(MorphEntryError.FoundMoreThanOneRight, string.Join(",", rightIds));
                                break;
                            default:
                                morphEntry.Right.Id = rightIds[0];
                                break;
                        }
                    }
                    else
                    {
                        var rightEntry = _morphEntryDbProvider.GetValues().SingleOrDefault(x => x.Id == morphEntry.Right.Id);

                        if (rightEntry is null)
                        {
                            error = _translateService.Translate(MorphEntryError.RightIdDoesNotExist, morphEntry.Right.Id);
                        }
                        else
                        {
                            morphEntry.Right = rightEntry;
                        }
                    }
                }
            }

            return error;
        }

        /// <summary>
        /// Adds or updates the morphological entry to the collection.
        /// </summary>
        /// <param name="newMorphEntry">A new input morphological entry.</param>
        /// <param name="row">The row number in the data table.</param>
        /// <param name="entry">The entry.</param>
        private void TryToAddOrUpdate(MorphEntry newMorphEntry, int row, string entry)
        {
            if (newMorphEntry is null)
            {
                _logMessageService.SendMessage(MessageLevel.Warning, _translateService.Translate(LogMessageType.MorphEntryNotFound, row, entry));
                return;
            }

            string error = Validate(newMorphEntry);

            if (!string.IsNullOrEmpty(error))
            {
                _logMessageService.SendMessage(MessageLevel.Error, _translateService.Translate(LogMessageType.MorphEntryError, row, entry, error));
                return;
            }

            // Update new entry if its Id = 0
            if (newMorphEntry.Id > 0)
            {
                var morphEntry = _morphEntryDbProvider.GetValues().Single(x => x.Id == newMorphEntry.Id);

                morphEntry.Entry = newMorphEntry.Entry;
                morphEntry.Parameters = newMorphEntry.Parameters;
                morphEntry.Base = newMorphEntry.Base;
                morphEntry.Left = newMorphEntry.Left;
                morphEntry.Right = newMorphEntry.Right;
                morphEntry.IsVirtual = newMorphEntry.IsVirtual;
                morphEntry.Source = newMorphEntry.Source;

                _morphEntryDbProvider.Update(morphEntry, false);

                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.MorphEntryUpdated, row, entry, morphEntry.Id));

                return;
            }

            var values = _morphEntryManager.GetValues(newMorphEntry.Entry, newMorphEntry.Parameters, newMorphEntry.Base, newMorphEntry.IsVirtual, newMorphEntry.Left, newMorphEntry.Right);

            // Insert new entry if there are no similar entries
            if (values.Count == 0)
            {
                _morphEntryDbProvider.Insert(newMorphEntry, false);

                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.MorphEntryInserted, row, entry, newMorphEntry.Id));

                return;
            }

            var newMorphEntries = new List<MorphEntry>();

            // Update similar entries by new entry
            foreach (var value in values)
            {
                byte[] tempParameters = ParameterFactory.Clone(value.Parameters);

                bool isUpdated = value.Parameters.UpdateByParameters(newMorphEntry.Parameters);

                if (!isUpdated || _morphCombinationManager.Check(value.Parameters))
                {
                    value.Base = newMorphEntry.Base;
                    value.Source = newMorphEntry.Source;

                    _morphEntryDbProvider.Update(value, false);

                    newMorphEntries.Add(value);
                }
                else
                {
                    value.Parameters = tempParameters;
                }
            }

            if (newMorphEntries.Count > 0)
            {
                if (newMorphEntries.Count == 1)
                {
                    _logMessageService.SendMessage(_translateService.Translate(LogMessageType.MorphEntryUpdated, row, entry, newMorphEntries[0].Id));
                }
                else
                {
                    _logMessageService.SendMessage(_translateService.Translate(LogMessageType.MorphEntriesUpdated, row, entry,
                        string.Join(",", newMorphEntries.Select(x => x.Id))));
                }

                return;
            }

            // Insert new entry if all updated similar entries aren't checked by morphological combinations
            _morphEntryDbProvider.Insert(newMorphEntry, false);

            _logMessageService.SendMessage(_translateService.Translate(LogMessageType.MorphEntryInserted, row, entry, newMorphEntry.Id));
        }
    }
}
