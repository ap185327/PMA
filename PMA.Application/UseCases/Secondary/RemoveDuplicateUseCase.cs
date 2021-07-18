// <copyright file="RemoveDuplicateUseCase.cs" company="Andrey Pospelov">
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
using PMA.Utils.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The remove duplicate use case class.
    /// </summary>
    public sealed class RemoveDuplicateUseCase : UseCaseBase<RemoveDuplicateUseCase, MorphParserInputPort>, IRemoveDuplicateUseCase
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
        /// Initializes a new instance of <see cref="RemoveDuplicateUseCase"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public RemoveDuplicateUseCase(IMediator mediator, ParallelOptions parallelOptions, ILogger<RemoveDuplicateUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<RemoveDuplicateUseCase,MorphParserInputPort>

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
            InternalExecute(ref wordForm);
            _inputData.WordForm = wordForm;
            time.Stop();

#if DEBUG
            Trace.WriteLine($"RemoveDuplicateUseCase.Execute() => {time.ElapsedMilliseconds} ms ({_inputData.WordForm.TotalSolutionCount()})");
#endif
            Logger.LogInformation($"RemoveDuplicateUseCase.Execute() => {time.ElapsedMilliseconds} ms");

            _inputData = null;

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion

        /// <summary>
        /// Internal version of the Execute method.
        /// </summary>
        /// <param name="wordForm">The WordForm.</param>
        private void InternalExecute(ref WordForm wordForm)
        {
            if (wordForm is null) return;

            var newWordForm = WordFormFactory.Clone(wordForm, new List<Solution>(wordForm.Solutions));

            bool isChanged = MergeDuplicates(newWordForm);

            isChanged = MergeSimilarSolutions(newWordForm) || isChanged;

            var solutions = newWordForm.Solutions;

            int count = solutions.Count;

            if (count <= MaxSolutionsForCurrentThread)
            {
                for (int i = 0; i < count; i++)
                {
                    var solution = solutions[i];

                    var left = solution.Left;
                    var right = solution.Right;

                    InternalExecute(ref left);
                    InternalExecute(ref right);

                    if (left != solution.Left || right != solution.Right)
                    {
                        isChanged = true;

                        newWordForm.Solutions.Remove(solution);

                        var newSolution = SolutionFactory.Clone(solution, left, right);

                        newWordForm.Solutions.Insert(i, newSolution);
                    }
                }
            }
            else
            {
                var newSolutionBag = new ConcurrentBag<Solution>();

                Parallel.ForEach(solutions, ParallelOptions, solution =>
                {
                    var left = solution.Left;
                    var right = solution.Right;

                    InternalExecute(ref left);
                    InternalExecute(ref right);

                    if (left != solution.Left || right != solution.Right)
                    {
                        isChanged = true;

                        var newSolution = SolutionFactory.Clone(solution, left, right);

                        newSolutionBag.Add(newSolution);
                    }
                    else
                    {
                        newSolutionBag.Add(solution);
                    }
                });

                newWordForm.Solutions = new List<Solution>(newSolutionBag);
            }

            if (isChanged)
            {
                wordForm = newWordForm;
            }
        }

        /// <summary>
        /// Merges duplicates.
        /// </summary>
        /// <param name="wordForm">The wordform.</param>
        private static bool MergeDuplicates(WordForm wordForm)
        {
            var solutionGroups = wordForm.Solutions
                .GroupBy(solution => solution.GetKey())
                .Where(solutionGroup => solutionGroup.Count() > 1);

            // ReSharper disable once PossibleMultipleEnumeration
            if (!solutionGroups.Any())
            {
                return false;
            }

            // ReSharper disable once PossibleMultipleEnumeration
            using var enumeratorGroups = solutionGroups.GetEnumerator();

            while (enumeratorGroups.MoveNext())
            {
                // ReSharper disable once PossibleNullReferenceException
                using var enumeratorGroup = enumeratorGroups.Current.GetEnumerator();

                enumeratorGroup.MoveNext();
                var firstSolution = enumeratorGroup.Current;

                wordForm.Solutions.Remove(firstSolution);

                while (enumeratorGroup.MoveNext())
                {
                    var solution = enumeratorGroup.Current;

                    firstSolution = MergeSolutions(firstSolution, solution);

                    wordForm.Solutions.Remove(solution);
                }

                wordForm.Solutions.Add(firstSolution);
            }

            return true;
        }

        /// <summary>
        /// Merges similar solutions.
        /// </summary>
        /// <param name="wordForm">The wordform.</param>
        private static bool MergeSimilarSolutions(WordForm wordForm)
        {
            var solutionGroups = wordForm.Solutions
                .GroupBy(solution => solution.GetSimpleKey())
                .Where(solutionGroup => solutionGroup.Count() > 1);

            // ReSharper disable once PossibleMultipleEnumeration
            if (!solutionGroups.Any()) return false;

            bool isChanged = false;

            // ReSharper disable once PossibleMultipleEnumeration
            using var enumeratorGroups = solutionGroups.GetEnumerator();

            while (enumeratorGroups.MoveNext())
            {
                var group = enumeratorGroups.Current;

                // ReSharper disable once AssignNullToNotNullAttribute
                using var enumeratorCanBeUpdatedGroup = @group.Where(x => x.Content.Parameters.Any(y => y == 0)).GetEnumerator();

                while (enumeratorCanBeUpdatedGroup.MoveNext())
                {
                    var canBeUpdatedSolution = enumeratorCanBeUpdatedGroup.Current;
                    var newCanBeUpdatedSolution = canBeUpdatedSolution;

                    using (var enumeratorCanBeMergedGroup = @group.Where(x => SolutionsCanBeMerge(x, canBeUpdatedSolution)).GetEnumerator())
                    {
                        while (enumeratorCanBeMergedGroup.MoveNext())
                        {
                            var solution = enumeratorCanBeMergedGroup.Current;

                            newCanBeUpdatedSolution = MergeSolutions(newCanBeUpdatedSolution, solution);

                            wordForm.Solutions.Remove(solution);

                            isChanged = true;
                        }
                    }

                    if (canBeUpdatedSolution == newCanBeUpdatedSolution) continue;

                    wordForm.Solutions.Remove(canBeUpdatedSolution);
                    wordForm.Solutions.Add(newCanBeUpdatedSolution);
                }
            }

            return isChanged;
        }

        /// <summary>
        /// Merges two solutions.
        /// </summary>
        /// <param name="thisSolution">The base solution.</param>
        /// <param name="solution">The merging solution.</param>
        private static Solution MergeSolutions(Solution thisSolution, Solution solution)
        {
            if (thisSolution == solution) return thisSolution;

            var newSolution = SolutionFactory.Clone(thisSolution, null, null);

            if (solution.Left != null)
            {
                newSolution.Left = WordFormFactory.Clone(thisSolution.Left, new List<Solution>(thisSolution.Left.Solutions));
                newSolution.Left.Solutions.AddRange(solution.Left.Solutions);
            }
            else
            {
                newSolution.Left = thisSolution.Left;
            }

            if (solution.Right != null)
            {
                newSolution.Right = WordFormFactory.Clone(thisSolution.Right, new List<Solution>(thisSolution.Right.Solutions));
                newSolution.Right.Solutions.AddRange(solution.Right.Solutions);
            }
            else
            {
                newSolution.Right = thisSolution.Right;
            }

            if (solution.CollapseRating > newSolution.CollapseRating)
            {
                newSolution.CollapseRating = solution.CollapseRating;
            }

            if (solution.Rules != null)
            {
                if (newSolution.Rules is null)
                {
                    newSolution.Rules = solution.Rules;
                }
                else
                {
                    for (int i = 0; i < solution.Rules.Count; i++)
                    {
                        var rule = solution.Rules[i];
                        if (!newSolution.Rules.Contains(rule))
                        {
                            newSolution.Rules.Add(rule);
                        }
                    }
                }
            }

            if (solution.Sandhi is null) return newSolution;

            if (newSolution.Sandhi is null)
            {
                newSolution.Sandhi = solution.Sandhi;
            }
            else
            {
                for (int i = 0; i < solution.Sandhi.Count; i++)
                {
                    var sandhi = solution.Sandhi[i];
                    if (!newSolution.Sandhi.Contains(sandhi))
                    {
                        newSolution.Sandhi.Add(sandhi);
                    }
                }
            }

            return newSolution;
        }

        /// <summary>
        /// Checks solutions can be merged or not.
        /// </summary>
        /// <param name="thisSolution">The base solution.</param>
        /// <param name="solution">The merging solution.</param>
        /// <returns>Solutions can be merged or not.</returns>
        private static bool SolutionsCanBeMerge(Solution thisSolution, Solution solution)
        {
            if (thisSolution == solution) return false;

            byte[] parameters = solution.Content.Parameters;
            byte[] thisParameters = thisSolution.Content.Parameters;

            return !parameters.Where((t, i) => t != 0 && t != thisParameters[i]).Any();
        }
    }
}
