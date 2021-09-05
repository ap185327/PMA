// <copyright file="RemoveSolutionWithExcessiveDepthUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.Factories;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The remove solution with excessive depth use case class.
    /// </summary>
    public sealed class RemoveSolutionWithExcessiveDepthUseCase : UseCaseBase<RemoveSolutionWithExcessiveDepthUseCase, MorphParserInputPort>, IRemoveSolutionWithExcessiveDepthUseCase
    {
        /// <summary>
        /// Options that configure the operation of methods on the <see cref="Parallel"/> class.
        /// </summary>
        private readonly ParallelOptions _parallelOptions = new();

        /// <summary>
        /// Maximum number of solutions for current thread.
        /// </summary>
        private const int MaxSolutionsForCurrentThread = 20;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// THe input data.
        /// </summary>
        private MorphParserInputPort _inputData;

        /// <summary>
        /// The max depth level value.
        /// </summary>
        private int _maxDepthLevel;

        /// <summary>
        /// Initializes a new instance of <see cref="RemoveSolutionWithExcessiveDepthUseCase"/> class.
        /// </summary>
        /// <param name="settingService">The setting service.</param>
        /// <param name="logger">The logger.</param>
        public RemoveSolutionWithExcessiveDepthUseCase(ISettingService settingService, ILogger<RemoveSolutionWithExcessiveDepthUseCase> logger) : base(logger)
        {
            _settingService = settingService;
        }

        #region Overrides of UseCaseBase<RemoveSolutionWithExcessiveDepthUseCase,MorphParserInputPort>

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

            _parallelOptions.CancellationToken = token;

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

            _inputData = inputPort;
            _maxDepthLevel = _settingService.GetValue<int>("Options.MaxDepthLevel");

            var time = new Stopwatch();

            time.Start();
            var wordForm = _inputData.WordForm;
            await Task.Run(() => NeedToRemove(ref wordForm, 0), token);
            _inputData.WordForm = wordForm;
            time.Stop();

#if DEBUG
            Trace.WriteLine($"RemoveSolutionWithExcessiveDepthUseCase.Execute() => {time.ElapsedMilliseconds} ms ({_inputData.WordForm.TotalSolutionCount()})");
#endif
            Logger.LogInformation($"RemoveSolutionWithExcessiveDepthUseCase.Execute() => {time.ElapsedMilliseconds} ms");

            _inputData = null;

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion

        /// <summary>
        /// Determines if the wordform needs to be removed or not.
        /// </summary>
        /// <param name="wordForm">The wordform.</param>
        /// <param name="currentDepthLevel">The current depth level.</param>
        /// <returns>The wordform needs to be removed or not.</returns>
        private bool NeedToRemove(ref WordForm wordForm, int currentDepthLevel)
        {
            if (wordForm is null) return false;

            bool isChanged = false;

            var solutions = wordForm.Solutions;

            var newSolutions = new List<Solution>();

            if (solutions.Count <= MaxSolutionsForCurrentThread)
            {
                for (int i = 0; i < solutions.Count; i++)
                {
                    var solution = solutions[i];

                    var newSolution = solution;

                    if (!NeedToRemove(ref newSolution, currentDepthLevel))
                    {
                        if (newSolution != solution)
                        {
                            isChanged = true;
                        }

                        newSolutions.Add(newSolution);
                    }
                    else
                    {
                        isChanged = true;
                    }
                }
            }
            else
            {
                var newSolutionBag = new ConcurrentBag<Solution>();

                Parallel.ForEach(solutions, _parallelOptions, solution =>
                {
                    _parallelOptions.CancellationToken.ThrowIfCancellationRequested();

                    var newSolution = solution;

                    if (!NeedToRemove(ref newSolution, currentDepthLevel))
                    {
                        if (newSolution != solution)
                        {
                            isChanged = true;
                        }

                        newSolutionBag.Add(newSolution);
                    }
                    else
                    {
                        isChanged = true;
                    }
                });

                newSolutions.AddRange(newSolutionBag);
            }

            if (isChanged)
            {
                wordForm = WordFormFactory.Clone(wordForm, newSolutions);
            }

            return wordForm.Solutions.Count == 0;
        }

        /// <summary>
        ///  Determines if the solution needs to be removed or not.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <param name="currentDepthLevel">The current depth level.</param>
        /// <returns>The solution needs to be removed or not.</returns>
        private bool NeedToRemove(ref Solution solution, int currentDepthLevel)
        {
            if (solution.Rules is null || !solution.Rules[0].IsCollapsed)
            {
                currentDepthLevel++;
            }

            if (solution.Content.Id == 0 && currentDepthLevel > _maxDepthLevel) return true;

            var left = solution.Left;
            var right = solution.Right;

            if (NeedToRemove(ref left, currentDepthLevel))
            {
                return true;
            }

            if (NeedToRemove(ref right, currentDepthLevel))
            {
                return true;
            }

            if (left == solution.Left && right == solution.Right) return false;

            solution = SolutionFactory.Clone(solution, left, right);

            return false;
        }
    }
}
