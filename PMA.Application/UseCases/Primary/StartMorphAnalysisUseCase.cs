// <copyright file="StartMorphAnalysisUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Secondary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The start morphological analysis use case class.
    /// </summary>
    public sealed class StartMorphAnalysisUseCase : UseCaseBase<StartMorphAnalysisUseCase, MorphParserInputPort>, IStartMorphAnalysisUseCase
    {
        /// <summary>
        /// The morphological parser interactor.
        /// </summary>
        private readonly IMorphParserInteractor _morphParserInteractor;

        /// <summary>
        /// The mediator.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartMorphAnalysisUseCase" /> class.
        /// </summary>
        /// <param name="morphParserInteractor">The morphological parser interactor.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="logger">The logger.</param>
        public StartMorphAnalysisUseCase(IMorphParserInteractor morphParserInteractor, IMediator mediator, ILogger<StartMorphAnalysisUseCase> logger) : base(logger)
        {
            _morphParserInteractor = morphParserInteractor;
            _mediator = mediator;
        }

        #region Overrides of UseCaseBase<StartMorphAnalysisInputPort,bool>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(MorphParserInputPort inputPort)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override async Task<OperationResult> ExecuteAsync(MorphParserInputPort inputPort, CancellationToken token = default)
        {

            if (inputPort is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputPort));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputPort));
            }

            if (inputPort.MorphEntry is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputPort.MorphEntry));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputPort.MorphEntry));
            }

            try
            {
                // ReSharper disable once MethodSupportsCancellation
                await _mediator.Publish(new MorphParserNotification
                {
                    State = ProcessState.InProgress
                }, token);

                var result = await _morphParserInteractor.ParseMorphEntryAsync(inputPort, token);

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                    return OperationResult.FailureResult(result.Messages);
                }

                result = await _morphParserInteractor.RemoveSolutionsWithExcessiveDepthAsync(inputPort, token);

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                    return OperationResult.FailureResult(result.Messages);
                }

                result = await _morphParserInteractor.CollapseSolutionsAsync(inputPort, token);

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                    return OperationResult.FailureResult(result.Messages);
                }

                result = await _morphParserInteractor.RemoveUnsuitableDerivativeSolutionsAsync(inputPort, token);

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                    return OperationResult.FailureResult(result.Messages);
                }

                result = await _morphParserInteractor.UpdateSolutionsAsync(inputPort, token);

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                    return OperationResult.FailureResult(result.Messages);
                }

                result = await _morphParserInteractor.RemoveDuplicatesAsync(inputPort, token);

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                    return OperationResult.FailureResult(result.Messages);
                }

                result = await _morphParserInteractor.RemoveUnsuitableSolutionsAsync(inputPort, token);

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                    return OperationResult.FailureResult(result.Messages);
                }

                result = await _morphParserInteractor.SortSolutionsAsync(inputPort, token);

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                    return OperationResult.FailureResult(result.Messages);
                }
#if DEBUG
                result = await _morphParserInteractor.ValidateSolutionsAsync(inputPort, token);

                if (!result.Success)
                {
                    Logger.LogErrors(result.Messages);
                    return OperationResult.FailureResult(result.Messages);
                }
#endif
                await _morphParserInteractor.ClearCacheAsync(token);

                await _mediator.Publish(new MorphParserNotification
                {
                    Result = inputPort.WordForm,
                    State = ProcessState.Completed
                }, token);

                return OperationResult.SuccessResult();
            }
            catch (Exception exception)
            {
                if (exception is not OperationCanceledException && exception is not AggregateException)
                {
                    return OperationResult.ExceptionResult(exception);
                }

                if (inputPort.WordForm is not null)
                {
                    inputPort.WordForm.Solutions = null;
                }

                await _morphParserInteractor.ClearCacheAsync(token);

                await _mediator.Publish(new MorphParserNotification
                {
                    Result = inputPort.WordForm,
                    State = ProcessState.Canceled
                }, CancellationToken.None);

                return OperationResult.SuccessResult();
            }
        }

        #endregion
    }
}
