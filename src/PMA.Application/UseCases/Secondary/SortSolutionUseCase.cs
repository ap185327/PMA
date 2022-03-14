// <copyright file="SortSolutionUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The sort solution use case class.
    /// </summary>
    public sealed class SortSolutionUseCase : UseCaseBase<SortSolutionUseCase, MorphParserInputPort>, ISortSolutionUseCase
    {
        /// <summary>
        /// Options that configure the operation of methods on the <see cref="Parallel"/> class.
        /// </summary>
        private readonly ParallelOptions _parallelOptions = new();

        /// <summary>
        /// The frequency rating manager.
        /// </summary>
        private readonly IFreqRatingManager _freqRatingManager;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// Gets or sets a ratio of frequency rating (1 - maximum, 0 - minimum).
        /// </summary>
        private double _freqRatingRatio;

        /// <summary>
        /// Gets a ratio of morphological rule rating (1 - maximum, 0 - minimum).
        /// </summary>
        private double _morphRuleRatio;

        /// <summary>
        /// THe input data.
        /// </summary>
        private MorphParserInputPort _inputData;

        /// <summary>
        /// Initializes a new instance of <see cref="SortSolutionUseCase"/> class.
        /// </summary>
        /// <param name="freqRatingManager">The frequency rating manager.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="logger">The logger.</param>
        public SortSolutionUseCase(IFreqRatingManager freqRatingManager, ISettingService settingService, ILogger<SortSolutionUseCase> logger) : base(logger)
        {
            _freqRatingManager = freqRatingManager;
            _settingService = settingService;
        }

        #region Overrides of UseCaseBase<SortSolutionUseCase,MorphParserInputPort>

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
            UpdateLayer();
            UpdateRatingRatios();

            var time = new Stopwatch();

            time.Start();
            await Task.Run(() => InternalExecute(_inputData.WordForm), token);
            time.Stop();

#if DEBUG
            Trace.WriteLine($"SortSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms");
#endif
            Logger.LogInformation($"SortSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms");

            _inputData = null;

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion

        /// <summary>
        /// Updates chronological layer.
        /// </summary>
        private void UpdateLayer()
        {
            uint layer = _settingService.GetValue<uint>("Options.Layer");

            if (layer == MorphConstants.AutoLayer)
            {
                _freqRatingManager.SetLayerByEntry(_inputData.WordForm.Entry);
            }
            else
            {
                _freqRatingManager.SetLayer(layer);
            }
        }

        /// <summary>
        /// Updates rating ratios.
        /// </summary>
        private void UpdateRatingRatios()
        {
            _freqRatingRatio = _settingService.GetValue<double>("Options.FreqRatingRatio");
            _morphRuleRatio = 1 - _freqRatingRatio;
        }

        /// <summary>
        /// Internal version of the Execute method.
        /// </summary>
        /// <param name="wordForm">The WordForm.</param>
        private void InternalExecute(WordForm wordForm)
        {
            _parallelOptions.CancellationToken.ThrowIfCancellationRequested();

            if (wordForm is null) return;

            var solutions = wordForm.Solutions;

            for (int i = 0; i < solutions.Count; i++)
            {
                var solution = solutions[i];

                InternalExecute(solution.Left);
                InternalExecute(solution.Right);

                CalculateRating(solution);
            }

            if (solutions.Count > 1)
            {
                if (_inputData.ParsingType == MorphParsingType.Debug)
                {
                    wordForm.Solutions = solutions.Where(x => x.Rules is not null).OrderBy(x => x.Rules[0].Id).ToList();
                    wordForm.Solutions.AddRange(solutions.Where(x => x.Rules is null).OrderByDescending(x => x.Rating));
                }
                else
                {
                    wordForm.Solutions = solutions.OrderByDescending(x => x.Rating).ToList();
                }
            }
        }

        /// <summary>
        /// Calculates the solution rating.
        /// </summary>
        /// <param name="solution">The solution.</param>
        private void CalculateRating(Solution solution)
        {
            if (solution.Rating > -1d) return;

            if (solution.Content.Error != SolutionError.Success)
            {
                solution.Rating = 0d;
                return;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (solution.Rules is null && solution.CollapseRating == 1d)
            {
                solution.Rating = 2d;
                return;
            }

            double leftRating = 1d;
            double leftFreqRating = 1d;

            if (solution.Left != null)
            {
                leftRating = Math.Min(1, solution.Left.GetMaxRating());
                leftFreqRating = _freqRatingManager.GetRating(solution.Left.Entry);
            }

            double rightRating = 1;
            double rightFreqRating = 1;

            if (solution.Right != null)
            {
                rightRating = Math.Min(1, solution.Right.GetMaxRating());
                rightFreqRating = _freqRatingManager.GetRating(solution.Right.Entry);
            }

            double ruleRating = solution.Rules?.Max(x => x.Rating) ?? 1;

            double dictRating = solution.Content.Id > 0 ? 1 : 0;

            double collapseRating = solution.CollapseRating;

            solution.Rating = leftRating * rightRating * ruleRating * collapseRating * _morphRuleRatio + _freqRatingRatio * leftFreqRating * rightFreqRating + dictRating;
        }
    }
}
