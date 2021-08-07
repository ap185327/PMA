// <copyright file="StopMorphAnalysisUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Threading;
using System.Threading.Tasks;
using PMA.Domain.Enums;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The stop morphological analysis use case class.
    /// </summary>
    public sealed class StopMorphAnalysisUseCase : UseCaseBase<StopMorphAnalysisUseCase>, IStopMorphAnalysisUseCase, INotificationHandler<CancellationTokenResourceNotification>
    {
        /// <summary>
        /// The cancellation token resource.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Initializes a new instance of <see cref="StopMorphAnalysisUseCase"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public StopMorphAnalysisUseCase(IMediator mediator, ParallelOptions parallelOptions, ILogger<StopMorphAnalysisUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<StopMorphAnalysisUseCase,CancellationTokenSource>

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

            Mediator.Publish(new MorphParserNotification
            {
                State = ProcessState.Canceling
            });

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
