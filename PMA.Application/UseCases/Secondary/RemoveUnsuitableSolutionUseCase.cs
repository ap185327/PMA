// <copyright file="RemoveUnsuitableSolutionUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The remove unsuitable solution use case class.
    /// </summary>
    public sealed class RemoveUnsuitableSolutionUseCase : UseCaseBase<RemoveUnsuitableSolutionUseCase, MorphParserInputPort>, IRemoveUnsuitableSolutionUseCase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RemoveUnsuitableSolutionUseCase"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public RemoveUnsuitableSolutionUseCase(IMediator mediator, ParallelOptions parallelOptions, ILogger<RemoveUnsuitableSolutionUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<RemoveUnsuitableSolutionUseCase,MorphParserInputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(MorphParserInputPort inputData)
        {
            Logger.LogEntry();

            if (inputData is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData));
                Logger.LogExit();
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData));
            }

            if (inputData.WordForm is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData.WordForm));
                Logger.LogExit();
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData.WordForm));
            }

            if (inputData.MorphEntry is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData.MorphEntry));
                Logger.LogExit();
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData.MorphEntry));
            }

            if (ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                Logger.LogExit();
                return OperationResult.SuccessResult();
            }

            var time = new Stopwatch();

            time.Start();
            InternalExecute(inputData.WordForm, inputData.MorphEntry);
            time.Stop();

#if DEBUG
            Trace.WriteLine($"RemoveUnsuitableSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms ({inputData.WordForm.TotalSolutionCount()})");
#endif
            Logger.LogInformation($"RemoveUnsuitableSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms");

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion

        /// <summary>
        /// Internal version of the Execute method.
        /// </summary>
        /// <param name="wordForm">The wordform.</param>
        /// <param name="morphEntry">The input MorphEntry.</param>
        private static void InternalExecute(WordForm wordForm, MorphEntry morphEntry)
        {
            var solutions = wordForm.Solutions;

            if (solutions.Count == 0) return;

            for (int i = 0; i < solutions.Count; i++)
            {
                int length = morphEntry.Parameters.Length;
                for (int j = 0; j < length; j++)
                {
                    if (morphEntry.Parameters[j] == 0 || morphEntry.Parameters[j] == solutions[i].Content.Parameters[j]) continue;

                    solutions.RemoveAt(i);
                    i--;
                    break;
                }
            }

            if (solutions.Count == 0) return;

            if (morphEntry.Left is not null)
            {
                for (int i = 0; i < solutions.Count; i++)
                {
                    var solution = solutions[i];

                    if (solution.Left is not null && morphEntry.Left.Entry == solution.Left.Entry) continue;

                    solutions.RemoveAt(i);
                    i--;
                }

                if (solutions.Count == 0) return;

                if (morphEntry.Left.Id > 0)
                {
                    for (int i = 0; i < solutions.Count; i++)
                    {
                        if (morphEntry.Left.Id == solutions[i].Left.Solutions[0].Content.Id) continue;

                        solutions.RemoveAt(i);
                        i--;
                    }
                }

                if (solutions.Count == 0) return;
            }

            if (morphEntry.Right is null) return;

            for (int i = 0; i < solutions.Count; i++)
            {
                var solution = solutions[i];

                if (solution.Right is not null && morphEntry.Right.Entry == solution.Right.Entry) continue;

                solutions.RemoveAt(i);
                i--;
            }

            if (solutions.Count == 0) return;

            if (morphEntry.Right.Id == 0) return;

            for (int i = 0; i < solutions.Count; i++)
            {
                if (morphEntry.Right.Id == solutions[i].Right.Solutions[0].Content.Id) continue;

                solutions.RemoveAt(i);
                i--;
            }
        }
    }
}
