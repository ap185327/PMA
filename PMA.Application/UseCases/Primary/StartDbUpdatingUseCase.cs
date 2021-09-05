// <copyright file="StartDbUpdatingUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Loaders;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Notifications;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The start database updating use case class.
    /// </summary>
    public sealed class StartDbUpdatingUseCase : UseCaseBase<StartDbUpdatingUseCase, UpdateDbInputPort>, IStartDbUpdatingUseCase
    {
        /// <summary>
        /// The morphological combination loader.
        /// </summary>
        private readonly IMorphCombinationLoader _morphCombinationLoader;

        /// <summary>
        /// The sandhi group loader.
        /// </summary>
        private readonly ISandhiGroupLoader _sandhiGroupLoader;

        /// <summary>
        /// The sandhi rule loader.
        /// </summary>
        private readonly ISandhiRuleLoader _sandhiRuleLoader;

        /// <summary>
        /// The morphological rule loader.
        /// </summary>
        private readonly IMorphRuleLoader _morphRuleLoader;

        /// <summary>
        /// The morphological combination database provider.
        /// </summary>
        private readonly IMorphCombinationDbProvider _morphCombinationDbProvider;

        /// <summary>
        /// The morphological rule database provider.
        /// </summary>
        private readonly IMorphRuleDbProvider _morphRuleDbProvider;

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
        /// Initializes a new instance of <see cref="StartDbUpdatingUseCase"/> class.
        /// </summary>
        /// <param name="morphCombinationLoader">The morphological combination loader.</param>
        /// <param name="sandhiGroupLoader">The sandhi group loader.</param>
        /// <param name="sandhiRuleLoader">The sandhi rule loader.</param>
        /// <param name="morphRuleLoader">The morphological rule loader.</param>
        /// <param name="morphCombinationDbProvider">The morphological combination database provider.</param>
        /// <param name="morphRuleDbProvider">The morphological rule database provider.</param>
        /// <param name="translateService">The translate service.</param>
        /// <param name="logMessageService">The log message service.</param>
        /// <param name="excelDataService">The excel data service.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public StartDbUpdatingUseCase(IMorphCombinationLoader morphCombinationLoader,
            ISandhiGroupLoader sandhiGroupLoader,
            ISandhiRuleLoader sandhiRuleLoader,
            IMorphRuleLoader morphRuleLoader,
            IMorphCombinationDbProvider morphCombinationDbProvider,
            IMorphRuleDbProvider morphRuleDbProvider,
            ITranslateService translateService,
            ILogMessageService logMessageService,
            IExcelDataService excelDataService,
            IMediator mediator,
            ILogger<StartDbUpdatingUseCase> logger) : base(logger)
        {
            _morphCombinationLoader = morphCombinationLoader;
            _sandhiGroupLoader = sandhiGroupLoader;
            _sandhiRuleLoader = sandhiRuleLoader;
            _morphRuleLoader = morphRuleLoader;
            _morphCombinationDbProvider = morphCombinationDbProvider;
            _morphRuleDbProvider = morphRuleDbProvider;
            _translateService = translateService;
            _logMessageService = logMessageService;
            _excelDataService = excelDataService;
            _mediator = mediator;
        }

        #region Overrides of UseCaseBase<StartDbUpdatingUseCase,UpdateDbInputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(UpdateDbInputPort inputData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override async Task<OperationResult> ExecuteAsync(UpdateDbInputPort inputPort, CancellationToken token = default)
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

            if (!inputPort.IsMorphCombinationDbTableChecked &&
                !inputPort.IsMorphRuleDbTableChecked &&
                !inputPort.IsSandhiGroupDbTableChecked &&
                !inputPort.IsSandhiRuleDbTableChecked)
            {
                return OperationResult.SuccessResult();
            }

            try
            {
                await Task.Run(() =>
                {
                    _mediator.Publish(new UpdateDbNotification
                    {
                        State = ProcessState.InProgress
                    }, token);

                    _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportStart));
                    _logMessageService.SendMessage(_translateService.Translate(LogMessageType.DataFileOpening));

                    try
                    {
                        _excelDataService.Open(inputPort.DataFilePath);
                    }
                    catch (FileNotFoundException exception)
                    {
                        _logMessageService.SendMessage(MessageLevel.Error, _translateService.Translate(LogMessageType.DataFileDoesNotExist, inputPort.DataFilePath));
                        _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                        Logger.LogError(exception.Message);

                        _mediator.Publish(new UpdateDbNotification
                        {
                            State = ProcessState.Error
                        }, token);

                        return;
                    }
                    catch (Exception exception)
                    {
                        _logMessageService.SendMessage(MessageLevel.Error, _translateService.Translate(LogMessageType.DataFileOpenError, exception.Message));
                        _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                        Logger.LogError(exception.Message);

                        _mediator.Publish(new UpdateDbNotification
                        {
                            State = ProcessState.Error
                        }, token);

                        return;
                    }

                    bool isSuccess = true;

                    if (inputPort.IsMorphCombinationDbTableChecked)
                    {
                        isSuccess = UpdateMorphCombinationDbTable(token);

                        if (isSuccess)
                        {
                            ReloadMorphCombinationDbProvider();
                        }
                    }

                    if (isSuccess && inputPort.IsSandhiGroupDbTableChecked)
                    {
                        isSuccess = UpdateSandhiGroupDbTable(token);
                    }

                    if (isSuccess && inputPort.IsSandhiRuleDbTableChecked)
                    {
                        isSuccess = UpdateSandhiRuleDbTable(token);
                    }

                    if (isSuccess && inputPort.IsMorphRuleDbTableChecked)
                    {
                        isSuccess = UpdateMorphRuleDbTable(token);
                    }

                    if (isSuccess &&
                        (inputPort.IsMorphRuleDbTableChecked ||
                         inputPort.IsSandhiGroupDbTableChecked ||
                         inputPort.IsSandhiRuleDbTableChecked))
                    {
                        ReloadMorphRuleDbProvider();
                    }

                    _logMessageService.SendMessage(_translateService.Translate(LogMessageType.DataFileClosing));

                    try
                    {
                        _excelDataService.Close();
                    }
                    catch (Exception exception)
                    {
                        _logMessageService.SendMessage(MessageLevel.Error, _translateService.Translate(LogMessageType.DataFileCloseError, exception.Message));
                    }

                    _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportCompleted));
                    _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                    _mediator.Publish(new UpdateDbNotification
                    {
                        State = ProcessState.Completed
                    }, token);

                }, token);

                return OperationResult.SuccessResult();
            }
            catch (OperationCanceledException)
            {
                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportCanceled));
                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                await _mediator.Publish(new UpdateDbNotification
                {
                    State = ProcessState.Canceled
                }, CancellationToken.None);

                return OperationResult.SuccessResult();
            }
            catch (Exception exception)
            {
                return OperationResult.ExceptionResult(exception);
            }
        }

        #endregion

        /// <summary>
        /// Updates morphological combination database table.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>True if the operation is completed; otherwise - False.</returns>
        private bool UpdateMorphCombinationDbTable(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (!_morphCombinationLoader.ReadRawData())
            {
                _morphCombinationLoader.Clear();
                return false;
            }

            token.ThrowIfCancellationRequested();

            if (!_morphCombinationLoader.ValidateAndFormatRawData())
            {
                _morphCombinationLoader.Clear();
                return false;
            }

            token.ThrowIfCancellationRequested();

            if (!_morphCombinationLoader.LoadData())
            {
                _morphCombinationLoader.Clear();
                return false;
            }

            _morphCombinationLoader.Clear();
            return true;
        }

        /// <summary>
        /// Updates sandhi group database table.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>True if the operation is completed; otherwise - False.</returns>
        private bool UpdateSandhiGroupDbTable(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (!_sandhiGroupLoader.ReadRawData())
            {
                _sandhiGroupLoader.Clear();
                return false;
            }

            token.ThrowIfCancellationRequested();

            if (!_sandhiGroupLoader.ValidateAndFormatRawData())
            {
                _sandhiGroupLoader.Clear();
                return false;
            }

            token.ThrowIfCancellationRequested();

            if (!_sandhiGroupLoader.LoadData())
            {
                _sandhiGroupLoader.Clear();
                return false;
            }

            _sandhiGroupLoader.Clear();
            return true;
        }

        /// <summary>
        /// Updates sandhi rule database table.
        /// </summary>
        /// <returns>True if the operation is completed; otherwise - False.</returns>
        private bool UpdateSandhiRuleDbTable(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (!_sandhiRuleLoader.ReadRawData())
            {
                _sandhiRuleLoader.Clear();
                return false;
            }

            token.ThrowIfCancellationRequested();

            if (!_sandhiRuleLoader.ValidateAndFormatRawData())
            {
                _sandhiRuleLoader.Clear();
                return false;
            }

            token.ThrowIfCancellationRequested();

            if (!_sandhiRuleLoader.LoadData())
            {
                _sandhiRuleLoader.Clear();
                return false;
            }

            _sandhiRuleLoader.Clear();
            return true;
        }

        /// <summary>
        /// Updates morphological rule database table.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>True if the operation is completed; otherwise - False.</returns>
        private bool UpdateMorphRuleDbTable(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (!_morphRuleLoader.ReadRawData())
            {
                _morphRuleLoader.Clear();
                return false;
            }

            token.ThrowIfCancellationRequested();

            if (!_morphRuleLoader.ValidateAndFormatRawDataAsync(token).Result)
            {
                _morphRuleLoader.Clear();
                return false;
            }

            token.ThrowIfCancellationRequested();

            bool result = _morphRuleLoader.LoadData();

            if (!result)
            {
                _morphRuleLoader.Clear();
                return false;
            }

            _morphRuleLoader.Clear();
            return true;
        }

        /// <summary>
        /// Reloads morphological combination database provider.
        /// </summary>
        private void ReloadMorphCombinationDbProvider()
        {
            _morphCombinationDbProvider.Reload();
        }

        /// <summary>
        /// Reloads morphological rule database provider.
        /// </summary>
        private void ReloadMorphRuleDbProvider()
        {
            _morphRuleDbProvider.Reload();
        }
    }
}
