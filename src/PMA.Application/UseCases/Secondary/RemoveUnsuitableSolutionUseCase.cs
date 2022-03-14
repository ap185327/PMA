// <copyright file="RemoveUnsuitableSolutionUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System;
using System.Diagnostics;
using System.Threading;
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
        /// <param name="logger">The logger.</param>
        public RemoveUnsuitableSolutionUseCase(ILogger<RemoveUnsuitableSolutionUseCase> logger) : base(logger)
        {
        }

        #region Overrides of UseCaseBase<RemoveUnsuitableSolutionUseCase,MorphParserInputPort>

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
            Logger.LogEntry();

            token.ThrowIfCancellationRequested();

            if (inputPort is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputPort));
                Logger.LogExit();
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputPort));
            }

            if (inputPort.WordForm is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputPort.WordForm));
                Logger.LogExit();
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputPort.WordForm));
            }

            if (inputPort.MorphEntry is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputPort.MorphEntry));
                Logger.LogExit();
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputPort.MorphEntry));
            }

            var time = new Stopwatch();

            time.Start();
            await Task.Run(() => InternalExecute(inputPort.WordForm, inputPort.MorphEntry), token);
            time.Stop();

#if DEBUG
            Trace.WriteLine($"RemoveUnsuitableSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms ({inputPort.WordForm.TotalSolutionCount()})");
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
