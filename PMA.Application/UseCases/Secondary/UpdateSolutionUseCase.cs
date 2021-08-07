// <copyright file="UpdateSolutionUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.Factories;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Models;
using PMA.Utils.Collections;
using PMA.Utils.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The update solution use case class.
    /// </summary>
    public sealed class UpdateSolutionUseCase : UseCaseBase<UpdateSolutionUseCase, MorphParserInputPort>, IUpdateSolutionUseCase
    {
        /// <summary>
        /// Maximum number of solutions for current thread.
        /// </summary>
        private const int MaxSolutionsForCurrentThread = 40;

        /// <summary>
        /// The number of processors on the current machine.
        /// </summary>
        private readonly int _processorCount = Environment.ProcessorCount;

        /// <summary>
        /// The morphological combination manager.
        /// </summary>
        private readonly IMorphCombinationManager _morphCombinationManager;

        /// <summary>
        /// The input data.
        /// </summary>
        private MorphParserInputPort _inputData;

        /// <summary>
        /// Initializes a new instance of <see cref="UpdateSolutionUseCase"/> class.
        /// </summary>
        /// <param name="morphCombinationManager">The morphological combination manager.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public UpdateSolutionUseCase(IMorphCombinationManager morphCombinationManager, IMediator mediator, ParallelOptions parallelOptions, ILogger<UpdateSolutionUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _morphCombinationManager = morphCombinationManager;

            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<UpdateSolutionUseCase,MorphParserInputPort>

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
            UpdateWordForm(_inputData.WordForm);
            time.Stop();

#if DEBUG
            Trace.WriteLine($"UpdateSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms");
#endif
            Logger.LogInformation($"UpdateSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms");

            _inputData = null;

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion

        /// <summary>
        /// Inherits the morphological parameters of the child solutions, if possible.
        /// </summary>
        /// <param name="wordForm">The WordForm.</param>
        private void UpdateWordForm(WordForm wordForm)
        {
            if (wordForm is null) return;

            var solutions = wordForm.Solutions;

            if (solutions.Count <= MaxSolutionsForCurrentThread)
            {
                for (int i = 0; i < solutions.Count; i++)
                {
                    UpdateSolution(solutions[i]);
                }
            }
            else
            {
                var dynamicPartitions = new SingleElementPartitioner<Solution>(solutions).GetDynamicPartitions();

                void DynamicAction()
                {
                    // ReSharper disable once PossibleMultipleEnumeration
                    // ReSharper disable once AccessToDisposedClosure
                    using var enumerator = dynamicPartitions.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        UpdateSolution(enumerator.Current);
                    }
                }

                var actions = new Action[_processorCount];
                actions.Fill(DynamicAction);

                Parallel.Invoke(ParallelOptions, actions);
                ((IDisposable)dynamicPartitions).Dispose();

            }
        }

        /// <summary>
        /// Inherits the morphological parameters of the child solutions, if possible.
        /// </summary>
        /// <param name="solution">The Solution.</param>
        private void UpdateSolution(Solution solution)
        {
            UpdateWordForm(solution.Left);
            UpdateWordForm(solution.Right);

            if (solution.Content.Parameters.All(x => x != 0)) return;

            if (solution.Left != null && solution.Content.Base is MorphBase.Left or MorphBase.Both)
            {
                UpdateParameters(solution, solution.Left);
            }

            if (solution.Right != null && solution.Content.Base is MorphBase.Right or MorphBase.Both)
            {
                UpdateParameters(solution, solution.Right);
            }
        }

        /// <summary>
        /// Inherits the morphological parameters of the child solutions, if possible.
        /// </summary>
        /// <param name="solution">The Solution.</param>
        /// <param name="part">The Solution part.</param>
        private void UpdateParameters(Solution solution, WordForm part)
        {
            byte[] parameters = ParameterFactory.Clone(solution.Content.Parameters);

            var parameterCollection = part.Solutions.Select(x => x.Content.Parameters).ToList();

            byte[] solutionCollectiveParameters = parameterCollection.GetCollectiveParameters();

            bool isUpdated = parameters.UpdateByParameters(solutionCollectiveParameters);

            bool isUpdated2;

            if (_morphCombinationManager.CheckAndCache(parameters, out byte[] collectiveParameters))
            {
                isUpdated2 = parameters.UpdateByParameters(collectiveParameters);

                if (isUpdated || isUpdated2)
                {
                    solution.Content = SolutionContentFactory.Clone(solution.Content, parameters);
                }

                return;
            }

            isUpdated = false;
            parameters = ParameterFactory.Clone(solution.Content.Parameters);

            for (int i = 0; i < MorphConstants.ParameterCount; i++)
            {
                if (solutionCollectiveParameters[i] == MorphConstants.UnknownTermId || parameters[i] != MorphConstants.UnknownTermId) continue;

                parameters[i] = solutionCollectiveParameters[i];

                if (!_morphCombinationManager.CheckAndCache(parameters))
                {
                    parameters[i] = MorphConstants.UnknownTermId;
                }
                else
                {
                    isUpdated = true;
                }
            }

            _morphCombinationManager.CheckAndCache(parameters, out collectiveParameters);

            isUpdated2 = parameters.UpdateByParameters(collectiveParameters);

            if (isUpdated || isUpdated2)
            {
                solution.Content = SolutionContentFactory.Clone(solution.Content, parameters);
            }
        }
    }
}
