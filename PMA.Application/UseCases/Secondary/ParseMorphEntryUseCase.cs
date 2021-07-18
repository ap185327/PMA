// <copyright file="ParseMorphEntryUseCase.cs" company="Andrey Pospelov">
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
using PMA.Domain.Notifications;
using PMA.Utils.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The parse morphological entry use case class.
    /// </summary>
    public sealed class ParseMorphEntryUseCase : UseCaseBase<ParseMorphEntryUseCase, MorphParserInputPort>, IParseMorphEntryUseCase
    {
        /// <summary>
        /// Maximum number of solutions for current thread.
        /// </summary>
        private const int MaxRulesForCurrentThread = 30;

        /// <summary>
        /// The collection of cached solutions.
        /// </summary>
        private readonly ConcurrentDictionary<string, Solution> _tempDictionary = new();

        /// <summary>
        /// The morphological entry manager.
        /// </summary>
        private readonly IMorphEntryManager _morphEntryManager;

        /// <summary>
        /// The morphological rule manager.
        /// </summary>
        private readonly IMorphRuleManager _morphRuleManager;

        /// <summary>
        /// The morphological combination manager.
        /// </summary>
        private readonly IMorphCombinationManager _morphCombinationManager;

        /// <summary>
        /// The input data.
        /// </summary>
        private MorphParserInputPort _inputData;

        /// <summary>
        /// The current depth level value.
        /// </summary>
        private int _currentDepthLevel;

        private int CurrentDepthLevel
        {
            get => _currentDepthLevel;
            set
            {
                if (value != _currentDepthLevel)
                {
                    _currentDepthLevel = value;

                    Mediator.Publish(new DepthLevelNotification
                    {
                        CurrentDepthLevel = _currentDepthLevel
                    });
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ParseMorphEntryUseCase"/> class.
        /// </summary>
        /// <param name="morphEntryManager">The morphological entry manager.</param>
        /// <param name="morphRuleManager">The morphological rule manager.</param>
        /// <param name="morphCombinationManager">The morphological combination manager.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public ParseMorphEntryUseCase(IMorphEntryManager morphEntryManager,
            IMorphRuleManager morphRuleManager,
            IMorphCombinationManager morphCombinationManager,
            IMediator mediator,
            ParallelOptions parallelOptions,
            ILogger<ParseMorphEntryUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _morphEntryManager = morphEntryManager;
            _morphRuleManager = morphRuleManager;
            _morphCombinationManager = morphCombinationManager;

            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<ParseMorphEntryUseCase,MorphParserInputPort>

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

            _inputData = inputData;
            CurrentDepthLevel = 0;

            var time = new Stopwatch();

            time.Start();
            _inputData.WordForm = _inputData.MorphEntry.Id > 0
                ? GetWordFormFromMorphEntry(_inputData.MorphEntry, 0)
                : GetWordFormFromParameters(_inputData.MorphEntry.Entry,
                    _inputData.MorphEntry.Parameters,
                    _inputData.MorphEntry.IsVirtual,
                    _inputData.MorphEntry.Base,
                    _inputData.MorphEntry.Left,
                    _inputData.MorphEntry.Right,
                    string.Empty,
                    0);
            time.Stop();

#if DEBUG
            Trace.WriteLine($"ParseMorphEntryUseCase.Execute() => {time.ElapsedMilliseconds} ms ({_inputData.WordForm.TotalSolutionCount()})");
#endif
            Logger.LogInformation($"ParseMorphEntryUseCase.Execute() => {time.ElapsedMilliseconds} ms");

            // Clear
            _tempDictionary.Clear();
            _morphCombinationManager.Clear();
            _morphEntryManager.Clear();
            _morphRuleManager.Clear();
            _inputData = null;

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion

        #region WordForm methods

        private WordForm GetWordFormFromMorphEntry(MorphEntry morphEntry, int currentDepthLevel)
        {
            var solution = GetSolutionFromMorphEntry(morphEntry, currentDepthLevel);

            var wordForm = WordFormFactory.Create(morphEntry.Entry, solution);

            if (solution.IsCompleted()) return wordForm;

            var derivativeSolutions = GetSolutionCollectionFromMorphRules(solution, wordForm.Entry, string.Empty, currentDepthLevel);

            for (int i = 0; i < derivativeSolutions.Count; i++)
            {
                derivativeSolutions[i].Original = solution;
            }

            wordForm.Solutions.AddRange(derivativeSolutions);

            if (_inputData.ParsingType != MorphParsingType.Debug)
            {
                wordForm.Clear();
            }

            return wordForm;
        }

        private WordForm GetWordFormFromParameters(string entry, byte[] parameters, bool isVirtual, MorphBase morphBase, MorphEntry left, MorphEntry right, string label, int currentDepthLevel)
        {
            var wordForm = WordFormFactory.Create(entry);

            var solution = GetSolutionFromParameters(parameters, isVirtual, morphBase, left, right, currentDepthLevel);

            if (solution.Content.Error != SolutionError.Success)
            {
                wordForm.Solutions.Add(solution);

                return wordForm;
            }

            if (string.IsNullOrEmpty(label) || label == "dict")
            {
                wordForm.Solutions.AddRange(GetSolutionCollectionFromMorphDict(solution, entry, currentDepthLevel));
            }

            if (wordForm.Solutions.NoSuccess())
            {
                // Stop parsing if label = "dict"
                if (label == "dict")
                {
                    if (_inputData.ParsingType != MorphParsingType.Debug)
                    {
                        wordForm.Clear();
                    }
                    return wordForm;
                }

                var solutions = GetSolutionCollectionFromMorphRules(solution, entry, label, currentDepthLevel);

                wordForm.Solutions.AddRange(solutions);

                if (_inputData.ParsingType != MorphParsingType.Debug)
                {
                    wordForm.Clear();
                }
                return wordForm;
            }

            int count = wordForm.Solutions.Count;

            for (int i = 0; i < count; i++)
            {
                solution = wordForm.Solutions[i];

                if (solution.IsCompleted()) continue;

                label = solution.Rules is null ? string.Empty : solution.Rules[0].Label;

                var derivativeSolutions = GetSolutionCollectionFromMorphRules(solution, entry, label, currentDepthLevel);

                for (int j = 0; j < derivativeSolutions.Count; j++)
                {
                    derivativeSolutions[j].Original = solution;
                }

                wordForm.Solutions.AddRange(derivativeSolutions);
            }

            if (_inputData.ParsingType != MorphParsingType.Debug)
            {
                wordForm.Clear();
            }
            return wordForm;
        }

        #endregion WordForm methods

        #region Solution methods

        private Solution GetSolutionFromMorphEntry(MorphEntry morphEntry, int currentDepthLevel)
        {
            currentDepthLevel++;

            var solution = SolutionFactory.Create(
                SolutionContentFactory.Create(
                    morphEntry.Id,
                    morphEntry.Parameters,
                    morphEntry.Base,
                    morphEntry.IsVirtual)
                );

            if (morphEntry.Left != null) solution.Left = GetWordFormFromMorphEntry(morphEntry.Left, currentDepthLevel);

            if (morphEntry.Right != null) solution.Right = GetWordFormFromMorphEntry(morphEntry.Right, currentDepthLevel);

            return solution;
        }

        private Solution GetSolutionFromSandhi(Solution draftSolution, string left, string right, SandhiMatch sandhiMatch, int currentDepthLevel)
        {
            var morphRule = draftSolution.Rules[0];

            if (!morphRule.IsCollapsed)
            {
                currentDepthLevel++;
            }

            string key = draftSolution.GetUniqKey(left, right, sandhiMatch);

            if (currentDepthLevel > _inputData.MaxDepthLevel)
            {
                key += ";";

                if (_tempDictionary.TryGetValue(key, out var errorDictSolution))
                {
                    return errorDictSolution;
                }

                return _tempDictionary.GetOrAdd(key, SolutionFactory.Create(SolutionContentFactory.Create(draftSolution.Content.Id,
                        draftSolution.Content.Parameters,
                        morphRule.Base,
                        false,
                        SolutionError.DepthIsExceeded),
                    morphRule,
                    sandhiMatch));
            }

            if (CurrentDepthLevel < currentDepthLevel)
            {
                CurrentDepthLevel = currentDepthLevel;
            }

            if (_tempDictionary.TryGetValue(key, out var dictSolution))
            {
                return dictSolution;
            }

            var solution = SolutionFactory.Create(SolutionContentFactory.Create(draftSolution.Content.Id,
                    draftSolution.Content.Parameters,
                    morphRule.Base),
                morphRule,
                sandhiMatch);

            // LeftEntry
            switch (morphRule.LeftType)
            {
                case MorphRuleType.New:
                    {
                        byte[] leftParameters = ParameterFactory.Clone(morphRule.LeftParameters);

                        solution.Left = GetWordFormFromParameters(left, leftParameters, false, MorphBase.Unknown, null, null, morphRule.LeftLabel, currentDepthLevel);
                        break;
                    }
                case MorphRuleType.Copy:
                    {
                        byte[] leftParameters = ParameterFactory.Clone(solution.Content.Parameters);
                        leftParameters.OverrideBy(morphRule.LeftParameters);

                        solution.Left = GetWordFormFromParameters(left, leftParameters, false, MorphBase.Unknown, null, null, morphRule.LeftLabel, currentDepthLevel);
                        break;
                    }
            }

            if (solution.Left != null && solution.Left.Solutions.NoSuccess())
            {
                solution.Content.Error = SolutionError.NotFoundLeftByParameters;

                return _tempDictionary.GetOrAdd(key, solution);
            }

            // RightEntry
            switch (morphRule.RightType)
            {
                case MorphRuleType.New:
                    {
                        byte[] rightParameters = ParameterFactory.Clone(morphRule.RightParameters);

                        solution.Right = GetWordFormFromParameters(right, rightParameters, false, MorphBase.Unknown, null, null, morphRule.RightLabel, currentDepthLevel);
                        break;
                    }
                case MorphRuleType.Copy:
                    {
                        byte[] rightParameters = ParameterFactory.Clone(solution.Content.Parameters);
                        rightParameters.OverrideBy(morphRule.RightParameters);

                        solution.Right = GetWordFormFromParameters(right, rightParameters, false, MorphBase.Unknown, null, null, morphRule.RightLabel, currentDepthLevel);
                        break;
                    }
            }

            if (solution.Right != null && solution.Right.Solutions.NoSuccess())
            {
                solution.Content.Error = SolutionError.NotFoundRightByParameters;
            }

            return _tempDictionary.GetOrAdd(key, solution);
        }

        private Solution GetSolutionFromParameters(byte[] parameters, bool isVirtual, MorphBase morphBase, MorphEntry left, MorphEntry right, int currentDepthLevel)
        {
            currentDepthLevel++;

            var solution = SolutionFactory.Create(SolutionContentFactory.Create(parameters, morphBase, isVirtual));

            if (left != null)
            {
                solution.Left = left.Id > 0 ? GetWordFormFromMorphEntry(left, currentDepthLevel) : WordFormFactory.Create(left.Entry);
            }

            if (right is null) return solution;

            solution.Right = right.Id > 0 ? GetWordFormFromMorphEntry(right, currentDepthLevel) : WordFormFactory.Create(right.Entry);

            return solution;
        }

        #endregion Solution methods

        #region SolutionCollection methods

        private List<Solution> GetSolutionCollectionFromMorphRules(Solution draftSolution, string entry, string label, int currentDepthLevel)
        {
            var solutions = new List<Solution>();

            var morphRules = _morphRuleManager.GetAndCacheRules(label, draftSolution.Content.Parameters);

            int ruleCount = morphRules.Count;

            if (ruleCount == 0)
            {
                solutions.Add(SolutionFactory.Create(SolutionContentFactory.Clone(draftSolution.Content, SolutionError.NoRuleMatches)));

                return solutions;
            }

            if (ruleCount <= MaxRulesForCurrentThread)
            {
                for (int i = 0; i < ruleCount; i++)
                {
                    solutions.AddRange(GetSolutionCollectionFromMorphRule(draftSolution, entry, morphRules[i], currentDepthLevel));
                }
            }
            else
            {
                var newSolutionBag = new ConcurrentBag<Solution>();

                Parallel.ForEach(morphRules, ParallelOptions, morphRule =>
                {
                    var list = GetSolutionCollectionFromMorphRule(draftSolution, entry, morphRule, currentDepthLevel);

                    for (int i = 0; i < list.Count; i++)
                    {
                        newSolutionBag.Add(list[i]);
                    }
                });

                solutions.AddRange(newSolutionBag);
            }

            return solutions;
        }

        private List<Solution> GetSolutionCollectionFromMorphRule(Solution draftSolution, string entry, MorphRule morphRule, int currentDepthLevel)
        {
            var solutions = new List<Solution>();

            var solution = SolutionFactory.Create(SolutionContentFactory.Clone(draftSolution.Content), morphRule);

            var sandhiMatches = _morphRuleManager.GetAndCacheSandhiMatches(entry, morphRule);
            if (sandhiMatches is null)
            {
                solution.Content.Error = SolutionError.NoSandhiMatches;

                solutions.Add(solution);

                return solutions;
            }

            solution.Content.Parameters.UpdateByParameters(morphRule.Parameters);

            if (morphRule.NeedToCheck)
            {
                if (!_morphCombinationManager.CheckAndCache(solution.Content.Parameters, out byte[] collectiveParameters))
                {
                    solution.Content.Error = SolutionError.NoMorphCombinationMatches;

                    solutions.Add(solution);

                    return solutions;
                }

                solution.Content.Parameters.UpdateByParameters(collectiveParameters);
            }

            if (string.IsNullOrEmpty(morphRule.Entry))
            {
                /*for (int i = 0; i < sandhiMatches.Count; i++)
                {
                    var sandhiMatch = sandhiMatches[i];

                    string left = sandhiMatch.SandhiExpression + morphRule.LeftEntry;

                    if (!string.IsNullOrEmpty(left))
                    {
                        solutions.Add(GetSolutionFromSandhi(solution, left, string.Empty, sandhiMatch));
                    }
                }*/

                solutions.AddRange(sandhiMatches
                    .Select(sandhiMatch => new
                    {
                        sandhiMatch,
                        left = sandhiMatch.SandhiExpression + morphRule.Left
                    })
                    .Where(t => !string.IsNullOrEmpty(t.left))
                    .Select(t => GetSolutionFromSandhi(solution, t.left, string.Empty, t.sandhiMatch, currentDepthLevel)));
            }
            else
            {
                var rgx = new Regex(morphRule.Entry);

                for (int i = 0; i < sandhiMatches.Count; i++)
                {
                    var sandhiMatch = sandhiMatches[i];

                    if (!rgx.Match(sandhiMatch.SandhiExpression).Success) continue;

                    string left;
                    string right;

                    switch (morphRule.SandhiGroup)
                    {
                        case (int)SandhiGroup.Left:
                            {
                                left = rgx.Replace(sandhiMatch.SandhiExpression, morphRule.Left);
                                right = morphRule.Right;
                                break;
                            }
                        case (int)SandhiGroup.Right:
                            {
                                left = morphRule.Left;
                                right = rgx.Replace(sandhiMatch.SandhiExpression, morphRule.Right);
                                break;
                            }
                        default:
                            {
                                string[] parts = rgx.Replace(sandhiMatch.SandhiExpression, "|").Split('|');

                                left = parts[0] + morphRule.Left;
                                right = morphRule.Right + parts[1];
                                break;
                            }
                    }

                    if (!string.IsNullOrEmpty(left))
                    {
                        solutions.Add(GetSolutionFromSandhi(solution, left, right, sandhiMatch, currentDepthLevel));
                    }
                }
            }

            if (solutions.Count > 0) return solutions;

            solution.Content.Error = SolutionError.NoSandhiMatches;

            solutions.Add(solution);

            return solutions;
        }

        private List<Solution> GetSolutionCollectionFromMorphDict(Solution solution, string entry, int currentDepthLevel)
        {
            var solutions = new List<Solution>();

            var morphEntries = _morphEntryManager.GetValuesAndCache(entry, solution.Content.Parameters, solution.Content.Base, solution.Content.IsVirtual);

            for (int i = 0; i < morphEntries.Count; i++)
            {
                var dictSolution = GetSolutionFromMorphEntry(morphEntries[i], currentDepthLevel);

                if (solution.Left != null)
                {
                    if (dictSolution.Left is null || solution.Left.Entry != dictSolution.Left.Entry)
                    {
                        dictSolution.Content.Error = SolutionError.NoLeftMatches;
                    }
                }

                if (solution.Right != null)
                {
                    if (dictSolution.Right is null || solution.Right.Entry != dictSolution.Right.Entry)
                    {
                        dictSolution.Content.Error = SolutionError.NoRightMatches;
                    }
                }

                solutions.Add(dictSolution);
            }

            return solutions;
        }

        #endregion SolutionCollection methods
    }
}
