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
using PMA.Utils.Extensions;
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
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
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
            ParallelOptions parallelOptions,
            ILogger<StartDbUpdatingUseCase> logger) : base(mediator,
            parallelOptions,
            logger)
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

            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<StartDbUpdatingUseCase,UpdateDbInputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(UpdateDbInputPort inputData)
        {
            if (inputData is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData));
            }

            if (string.IsNullOrEmpty(inputData.DataFilePath))
            {
                Logger.LogError(ErrorMessageConstants.FilePathIsEmpty);
                return OperationResult.FailureResult(ErrorMessageConstants.FilePathIsEmpty);
            }

            if (!File.Exists(inputData.DataFilePath))
            {
                Logger.LogError(ErrorMessageConstants.FileNotFound, inputData.DataFilePath);
                return OperationResult.FailureResult(ErrorMessageConstants.FileNotFound, inputData.DataFilePath);
            }

            if (!inputData.IsMorphCombinationDbTableChecked &&
                !inputData.IsMorphRuleDbTableChecked &&
                !inputData.IsSandhiGroupDbTableChecked &&
                !inputData.IsSandhiRuleDbTableChecked)
            {
                return OperationResult.SuccessResult();
            }

            var cancellationTokenSource = new CancellationTokenSource();

            // ReSharper disable once MethodSupportsCancellation
            Mediator.Publish(new CancellationTokenResourceNotification
            {
                CancellationTokenSource = cancellationTokenSource
            });

            ParallelOptions.CancellationToken = cancellationTokenSource.Token;

            Task.Run(() =>
            {
                // ReSharper disable once MethodSupportsCancellation
                Mediator.Publish(new UpdateDbNotification
                {
                    State = ProcessState.InProgress
                });

                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportStart));
                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.DataFileOpening));

                try
                {
                    _excelDataService.Open(inputData.DataFilePath);
                }
                catch (FileNotFoundException exception)
                {
                    _logMessageService.SendMessage(MessageLevel.Error, _translateService.Translate(LogMessageType.DataFileDoesNotExist, inputData.DataFilePath));
                    _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                    Logger.LogError(exception.Message);

                    // ReSharper disable once MethodSupportsCancellation
                    Mediator.Publish(new UpdateDbNotification
                    {
                        State = ProcessState.Error
                    });

                    return;
                }
                catch (Exception exception)
                {
                    _logMessageService.SendMessage(MessageLevel.Error, _translateService.Translate(LogMessageType.DataFileOpenError, exception.Message));
                    _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                    Logger.LogError(exception.Message);

                    // ReSharper disable once MethodSupportsCancellation
                    Mediator.Publish(new UpdateDbNotification
                    {
                        State = ProcessState.Error
                    });

                    return;
                }

                bool isSuccess = true;

                if (inputData.IsMorphCombinationDbTableChecked)
                {
                    isSuccess = UpdateMorphCombinationDbTable();

                    if (isSuccess)
                    {
                        ReloadMorphCombinationDbProvider();
                    }
                }

                if (isSuccess && inputData.IsSandhiGroupDbTableChecked)
                {
                    isSuccess = UpdateSandhiGroupDbTable();
                }

                if (isSuccess && inputData.IsSandhiRuleDbTableChecked)
                {
                    isSuccess = UpdateSandhiRuleDbTable();
                }

                if (isSuccess && inputData.IsMorphRuleDbTableChecked)
                {
                    isSuccess = UpdateMorphRuleDbTable();
                }

                if (isSuccess &&
                    (inputData.IsMorphRuleDbTableChecked ||
                    inputData.IsSandhiGroupDbTableChecked ||
                    inputData.IsSandhiRuleDbTableChecked))
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

                _logMessageService.SendMessage(ParallelOptions.CancellationToken.IsCancellationRequested
                    ? _translateService.Translate(LogMessageType.ImportCanceled)
                    : _translateService.Translate(LogMessageType.ImportCompleted));

                _logMessageService.SendMessage(_translateService.Translate(LogMessageType.ImportEnd));

                // ReSharper disable once MethodSupportsCancellation
                Mediator.Publish(new UpdateDbNotification
                {
                    State = ParallelOptions.CancellationToken.IsCancellationRequested
                        ? ProcessState.Completed
                        : ProcessState.Canceled
                });

            }, cancellationTokenSource.Token);

            return OperationResult.SuccessResult();
        }

        #endregion

        /// <summary>
        /// Updates morphological combination database table.
        /// </summary>
        /// <returns>True if the operation is completed; otherwise - False.</returns>
        private bool UpdateMorphCombinationDbTable()
        {
            if (ParallelOptions.CancellationToken.IsCancellationRequested) return false;

            if (!_morphCombinationLoader.ReadRawData())
            {
                _morphCombinationLoader.Clear();
                return false;
            }

            if (ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                _morphCombinationLoader.Clear();
                return false;
            }

            if (!_morphCombinationLoader.ValidateAndFormatRawData())
            {
                _morphCombinationLoader.Clear();
                return false;
            }

            if (ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                _morphCombinationLoader.Clear();
                return false;
            }

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
        /// <returns>True if the operation is completed; otherwise - False.</returns>
        private bool UpdateSandhiGroupDbTable()
        {
            if (ParallelOptions.CancellationToken.IsCancellationRequested) return false;

            if (!_sandhiGroupLoader.ReadRawData())
            {
                _sandhiGroupLoader.Clear();
                return false;
            }

            if (ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                _sandhiGroupLoader.Clear();
                return false;
            }

            if (!_sandhiGroupLoader.ValidateAndFormatRawData())
            {
                _sandhiGroupLoader.Clear();
                return false;
            }

            if (ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                _sandhiGroupLoader.Clear();
                return false;
            }

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
        private bool UpdateSandhiRuleDbTable()
        {
            if (ParallelOptions.CancellationToken.IsCancellationRequested) return false;

            if (!_sandhiRuleLoader.ReadRawData())
            {
                _sandhiRuleLoader.Clear();
                return false;
            }

            if (ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                _sandhiRuleLoader.Clear();
                return false;
            }

            if (!_sandhiRuleLoader.ValidateAndFormatRawData())
            {
                _sandhiRuleLoader.Clear();
                return false;
            }

            if (ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                _sandhiRuleLoader.Clear();
                return false;
            }

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
        /// <returns>True if the operation is completed; otherwise - False.</returns>
        private bool UpdateMorphRuleDbTable()
        {
            if (ParallelOptions.CancellationToken.IsCancellationRequested) return false;

            if (!_morphRuleLoader.ReadRawData())
            {
                _morphRuleLoader.Clear();
                return false;
            }

            if (ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                _morphRuleLoader.Clear();
                return false;
            }

            if (!_morphRuleLoader.ValidateAndFormatRawData())
            {
                _morphRuleLoader.Clear();
                return false;
            }

            if (ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                _morphRuleLoader.Clear();
                return false;
            }

            if (!_morphRuleLoader.LoadData())
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
