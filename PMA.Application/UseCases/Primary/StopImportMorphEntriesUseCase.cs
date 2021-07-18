// <copyright file="StopImportMorphEntriesUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Enums;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The stop import morphological entries use case class.
    /// </summary>
    public sealed class StopImportMorphEntriesUseCase : UseCaseBase<StopImportMorphEntriesUseCase>, IStopImportMorphEntriesUseCase, INotificationHandler<CancellationTokenResourceNotification>
    {
        /// <summary>
        /// The cancellation token resource.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// The translate service.
        /// </summary>
        private readonly ITranslateService _translateService;

        /// <summary>
        /// The log message service.
        /// </summary>
        private readonly ILogMessageService _logMessageService;

        /// <summary>
        /// Initializes a new instance of <see cref="StopImportMorphEntriesUseCase"/> class.
        /// </summary>
        /// <param name="translateService">The translate service.</param>
        /// <param name="logMessageService">The log message service.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public StopImportMorphEntriesUseCase(ITranslateService translateService,
            ILogMessageService logMessageService,
            IMediator mediator,
            ParallelOptions parallelOptions,
            ILogger<StopImportMorphEntriesUseCase> logger) : base(mediator,
            parallelOptions,
            logger)
        {
            _translateService = translateService;
            _logMessageService = logMessageService;

            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<StopImportMorphEntriesUseCase>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute()
        {
            if (_cancellationTokenSource is { IsCancellationRequested: true })
            {
                Logger.LogError(ErrorMessageConstants.CancellationIsAlreadyRequested);
                return OperationResult.FailureResult(ErrorMessageConstants.CancellationIsAlreadyRequested);
            }

            Mediator.Publish(new ImportMorphEntryNotification
            {
                State = ProcessState.Canceling
            });

            _logMessageService.SendMessage(MessageLevel.Warning, _translateService.Translate(LogMessageType.ImportCanceling));

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;

            return OperationResult.SuccessResult();
        }

        #endregion

        #region Implementation of INotificationHandler<in CancellationTokenResourceNotification>

        /// <summary>Handles a notification</summary>
        /// <param name="notification">The notification</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task Handle(CancellationTokenResourceNotification notification, CancellationToken cancellationToken)
        {
            _cancellationTokenSource = notification.CancellationTokenSource;

            return Task.CompletedTask;
        }

        #endregion
    }
}
