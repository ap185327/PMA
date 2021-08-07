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
        /// Initializes a new instance of the <see cref="StartMorphAnalysisUseCase" /> class.
        /// </summary>
        /// <param name="morphParserInteractor">The morphological parser interactor.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public StartMorphAnalysisUseCase(IMorphParserInteractor morphParserInteractor, IMediator mediator, ParallelOptions parallelOptions, ILogger<StartMorphAnalysisUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _morphParserInteractor = morphParserInteractor;

            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<StartMorphAnalysisInputPort,bool>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(MorphParserInputPort inputData)
        {
            if (inputData is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData));
            }

            if (inputData.MorphEntry is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData.MorphEntry));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData.MorphEntry));
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
                Mediator.Publish(new MorphParserNotification
                {
                    State = ProcessState.InProgress
                });

                try
                {
                    var result = _morphParserInteractor.ParseMorphEntry(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        return;
                    }

                    result = _morphParserInteractor.RemoveSolutionsWithExcessiveDepth(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        return;
                    }

                    result = _morphParserInteractor.CollapseSolutions(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        return;
                    }

                    result = _morphParserInteractor.RemoveUnsuitableDerivativeSolutions(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        return;
                    }

                    result = _morphParserInteractor.UpdateSolutions(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        return;
                    }

                    result = _morphParserInteractor.RemoveDuplicates(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        return;
                    }

                    result = _morphParserInteractor.RemoveUnsuitableSolutions(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        return;
                    }

                    result = _morphParserInteractor.SortSolutions(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        return;
                    }
#if DEBUG
                    result = _morphParserInteractor.ValidateSolutions(inputData);

                    if (!result.Success)
                    {
                        Logger.LogErrors(result.Messages);
                        return;
                    }
#endif
                }
                catch (OperationCanceledException)
                {
                    if (inputData.WordForm is not null)
                    {
                        inputData.WordForm.Solutions = null;
                    }
                }

                _morphParserInteractor.ClearCache();

                // ReSharper disable once MethodSupportsCancellation
                Mediator.Publish(new MorphParserNotification
                {
                    Result = inputData.WordForm,
                    State = cancellationTokenSource.IsCancellationRequested
                        ? ProcessState.Canceled
                        : ProcessState.Completed
                });

            }, cancellationTokenSource.Token);

            return OperationResult.SuccessResult();
        }

        #endregion
    }
}
