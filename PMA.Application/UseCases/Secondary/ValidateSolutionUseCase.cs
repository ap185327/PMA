// <copyright file="ValidateSolutionUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Enums;
using PMA.Domain.Exceptions;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The validate solution use case class.
    /// </summary>
    public sealed class ValidateSolutionUseCase : UseCaseBase<ValidateSolutionUseCase, MorphParserInputPort>, IValidateSolutionUseCase
    {
        /// <summary>
        /// Maximum number of solutions for current thread.
        /// </summary>
        private const int MaxSolutionsForCurrentThread = 20;

        /// <summary>
        /// The input data.
        /// </summary>
        private MorphParserInputPort _inputData;

        /// <summary>
        /// Initializes a new instance of <see cref="ValidateSolutionUseCase"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public ValidateSolutionUseCase(IMediator mediator, ParallelOptions parallelOptions, ILogger<ValidateSolutionUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<ValidateSolutionUseCase,MorphParserInputPort>

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

            if (inputData.ParsingType == MorphParsingType.Debug || ParallelOptions.CancellationToken.IsCancellationRequested)
            {
                Logger.LogExit();
                return OperationResult.SuccessResult();
            }

            _inputData = inputData;

            var time = new Stopwatch();

            time.Start();
            Check(_inputData.WordForm, true);
            time.Stop();

#if DEBUG
            Trace.WriteLine($"ValidateSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms");
#endif
            Logger.LogInformation($"ValidateSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms");

            _inputData = null;

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion

        /// <summary>
        /// Checks the wordform.
        /// </summary>
        /// <param name="wordForm">The wordform.</param>
        /// <param name="first">The first check iteration.</param>
        private void Check(WordForm wordForm, bool first = false)
        {
            if (wordForm is null) return;

            var solutions = wordForm.Solutions;

            if (solutions is null) throw new CustomException("WordForm.Solutions is null");

            switch (solutions.Count)
            {
                case 0 when _inputData.ParsingType != MorphParsingType.Debug && !first:
                    throw new CustomException("WordForm.Solutions.Count == 0");
                case <= MaxSolutionsForCurrentThread:
                    {
                        for (int i = 0; i < solutions.Count; i++)
                        {
                            Check(solutions[i]);
                        }

                        break;
                    }
                default:
                    Parallel.ForEach(solutions, ParallelOptions, Check);
                    break;
            }
        }

        /// <summary>
        /// Checks the solution.
        /// </summary>
        /// <param name="solution">The solution.</param>
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void Check(Solution solution)
        {
            if (solution.Content is null) throw new CustomException("Solution.Content is null");

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (_inputData.ParsingType != MorphParsingType.Debug && solution.Content.Error != SolutionError.Success) throw new CustomException("Solution.Content.Error != SolutionErrorCode.Success");

            if (_inputData.ParsingType != MorphParsingType.Debug && solution.Rules != null && solution.Rules[0].IsCollapsed && solution.Left is null) throw new CustomException("Solution.Rules != null && solution.Rules[0].IsCollapsed && Solution.LeftEntry is null");

            Check(solution.Left);
            Check(solution.Right);
        }
    }
}
