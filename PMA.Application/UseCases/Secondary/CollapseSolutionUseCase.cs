// <copyright file="CollapseSolutionUseCase.cs" company="Andrey Pospelov">
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
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Models;
using PMA.Utils.Collections;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The collapse solution use case class.
    /// </summary>
    public sealed class CollapseSolutionUseCase : UseCaseBase<CollapseSolutionUseCase, MorphParserInputPort>, ICollapseSolutionUseCase
    {
        /// <summary>
        /// Maximum number of solutions for current thread.
        /// </summary>
        private const int MaxSolutionsForCurrentThread = 20;

        /// <summary>
        /// The number of processors on the current machine.
        /// </summary>
        private readonly int _processorCount = Environment.ProcessorCount;

        /// <summary>
        /// The input data.
        /// </summary>
        private MorphParserInputPort _inputData;

        /// <summary>
        /// Initializes a new instance of <see cref="CollapseSolutionUseCase"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public CollapseSolutionUseCase(IMediator mediator, ParallelOptions parallelOptions, ILogger<CollapseSolutionUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<CollapseSolutionUseCase,MorphParserInputPort>

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
            var wordForm = _inputData.WordForm;
            InternalExecute(ref wordForm, null);
            _inputData.WordForm = wordForm;
            time.Stop();

#if DEBUG
            Trace.WriteLine($"CollapseSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms ({_inputData.WordForm.TotalSolutionCount()})");
#endif
            Logger.LogInformation($"CollapseSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms");

            _inputData = null;

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion

        /// <summary>
        /// Internal version of the Execute method.
        /// </summary>
        /// <param name="wordForm">The WordForm.</param>
        /// <param name="parent">The parent solution.</param>
        /// <returns>Whether it is required to remove the WordForm parent solution.</returns>
        private (bool RemoveSolution, List<Solution> NewSolutions) InternalExecute(ref WordForm wordForm, Solution parent)
        {
            if (wordForm is null)
            {
                return (false, null);
            }

            bool isChanged = false;

            var newSolutions = new List<Solution>();
            var solutions = wordForm.Solutions;

            int count = solutions.Count;

            if (count <= MaxSolutionsForCurrentThread)
            {
                for (int i = 0; i < count; i++)
                {
                    var solution = solutions[i];

                    var left = solution.Left;
                    var right = solution.Right;

                    (bool leftRemoveSolution, var leftNewSolutions) = InternalExecute(ref left, solution);
                    (bool rightRemoveSolution, var rightNewSolutions) = InternalExecute(ref right, solution);

                    if (!leftRemoveSolution && !rightRemoveSolution)
                    {
                        if (left != solution.Left || right != solution.Right)
                        {
                            isChanged = true;

                            solution = SolutionFactory.Clone(solution, left, right);
                        }

                        newSolutions.Add(solution);
                    }
                    else
                    {
                        isChanged = true;
                    }

                    if (leftNewSolutions != null)
                    {
                        isChanged = true;
                        newSolutions.AddRange(leftNewSolutions);
                    }

                    if (rightNewSolutions == null) continue;

                    isChanged = true;
                    newSolutions.AddRange(rightNewSolutions);
                }
            }
            else
            {
                var dynamicPartitions = new SingleElementPartitioner<Solution>(solutions).GetDynamicPartitions();

                void DynamicAction()
                {
                    // ReSharper disable once AccessToDisposedClosure
                    // ReSharper disable once PossibleMultipleEnumeration
                    using var enumerator = dynamicPartitions.GetEnumerator();
                    var newSolutionChunk = new List<Solution>();

                    while (enumerator.MoveNext())
                    {
                        var solution = enumerator.Current;

                        // ReSharper disable once PossibleNullReferenceException
                        var left = solution.Left;
                        var right = solution.Right;

                        // ReSharper disable once PossibleNullReferenceException
                        (bool leftRemoveSolution, var leftNewSolutions) = InternalExecute(ref left, solution);
                        (bool rightRemoveSolution, var rightNewSolutions) = InternalExecute(ref right, solution);

                        if (!leftRemoveSolution && !rightRemoveSolution)
                        {
                            if (left != solution.Left || right != solution.Right)
                            {
                                isChanged = true;

                                solution = SolutionFactory.Clone(solution, left, right);
                            }

                            newSolutionChunk.Add(solution);
                        }
                        else
                        {
                            isChanged = true;
                        }

                        if (leftNewSolutions != null)
                        {
                            isChanged = true;
                            newSolutionChunk.AddRange(leftNewSolutions);
                        }

                        if (rightNewSolutions == null) continue;

                        isChanged = true;
                        newSolutionChunk.AddRange(rightNewSolutions);
                    }

                    lock (newSolutions)
                    {
                        newSolutions.AddRange(newSolutionChunk);
                    }
                }

                var actions = new Action[_processorCount];
                actions.Fill(DynamicAction);

                Parallel.Invoke(ParallelOptions, actions);
                ((IDisposable)dynamicPartitions).Dispose();
            }

            int newCount = newSolutions.Count;

            if (newCount == 0)
            {
                return (true, null);
            }

            if (!newSolutions.Any(x => x.Rules != null && x.Rules[0].IsCollapsed))
            {
                if (isChanged)
                {
                    wordForm = WordFormFactory.Clone(wordForm, newSolutions);
                }

                return (false, null);
            }

            var solutionGroups = new List<SolutionGroup>();

            for (int i = 0; i < newCount; i++)
            {
                var newSolution = newSolutions[i];

                if (newSolution.Rules != null && newSolution.Rules[0].IsCollapsed)
                {
                    int leftCount = newSolution.Left.Solutions.Count;

                    for (int j = 0; j < leftCount; j++)
                    {
                        var leftSolution = newSolution.Left.Solutions[j];

                        var leftSolutionClone = SolutionFactory.Clone(leftSolution, leftSolution.Left, leftSolution.Right);

                        leftSolutionClone.CollapseRating *= newSolution.Rules[0].Rating;

                        solutionGroups.Add(new SolutionGroup
                        {
                            Parent = newSolution.Left,
                            Solution = leftSolutionClone
                        });
                    }
                }
                else
                {
                    solutionGroups.Add(new SolutionGroup
                    {
                        Parent = wordForm,
                        Solution = newSolution
                    });
                }
            }

            var entryGroups = solutionGroups.GroupBy(x => x.Parent.Entry);

            // ReSharper disable once PossibleMultipleEnumeration
            if (entryGroups.Count() == 1)
            {
                wordForm = WordFormFactory.Create(solutionGroups[0].Parent.Entry, solutionGroups.Select(x => x.Solution).ToList());

                return (false, null);
            }

            bool removeParentSolution = true;
            var parentSolutions = new List<Solution>();

            // ReSharper disable once PossibleMultipleEnumeration
            using (var enumerator = entryGroups.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var group = enumerator.Current.ToList();

                    string entry = group[0].Parent.Entry;

                    if (entry == wordForm.Entry)
                    {
                        removeParentSolution = false;

                        wordForm = WordFormFactory.Clone(wordForm, group.Select(x => x.Solution).ToList());
                    }
                    else
                    {
                        WordForm parentRight = null;

                        if (parent.Right != null)
                        {
                            parentRight = WordFormFactory.Clone(parent.Right,
                                new List<Solution>(parent.Right.Solutions));
                        }

                        parentSolutions.Add(SolutionFactory.Clone(parent,
                            WordFormFactory.Create(entry, group.Select(x => x.Solution).ToList()),
                            parentRight));
                    }
                }
            }

            if (parentSolutions.Count == 0)
            {
                parentSolutions = null;
            }

            return (removeParentSolution, parentSolutions);
        }
    }
}
