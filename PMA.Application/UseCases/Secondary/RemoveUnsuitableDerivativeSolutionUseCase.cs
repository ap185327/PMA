// <copyright file="RemoveUnsuitableDerivativeSolutionUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.Extensions;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Enums;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The remove unsuitable derivative solution use case class.
    /// </summary>
    public sealed class RemoveUnsuitableDerivativeSolutionUseCase : UseCaseBase<RemoveUnsuitableDerivativeSolutionUseCase, MorphParserInputPort>, IRemoveUnsuitableDerivativeSolutionUseCase
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
        /// Initializes a new instance of <see cref="RemoveUnsuitableDerivativeSolutionUseCase"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public RemoveUnsuitableDerivativeSolutionUseCase(IMediator mediator, ParallelOptions parallelOptions, ILogger<RemoveUnsuitableDerivativeSolutionUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<RemoveUnsuitableDerivativeSolutionUseCase,MorphParserInputPort>

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
            InternalExecute(_inputData.WordForm);
            time.Stop();

#if DEBUG
            Trace.WriteLine($"RemoveUnsuitableDerivativeSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms ({_inputData.WordForm.TotalSolutionCount()})");
#endif
            Logger.LogInformation($"RemoveUnsuitableDerivativeSolutionUseCase.Execute() => {time.ElapsedMilliseconds} ms");

            _inputData = null;

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion

        /// <summary>
        /// Internal version of the Execute method.
        /// </summary>
        /// <param name="wordForm">The WordForm.</param>
        private void InternalExecute(WordForm wordForm)
        {
            if (wordForm is null) return;

            var solutions = wordForm.Solutions;

            var derivedSolutions = solutions.Where(x => x.Original != null).ToList();

            using (var enumerator = derivedSolutions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var derivedSolution = enumerator.Current;

                    // ReSharper disable once PossibleNullReferenceException
                    if (!Check(derivedSolution.Original, derivedSolution))
                    {
                        solutions.Remove(derivedSolution);
                    }
                }
            }

            if (solutions.Count <= MaxSolutionsForCurrentThread)
            {
                for (int i = 0; i < solutions.Count; i++)
                {
                    var solution = solutions[i];

                    InternalExecute(solution.Left);
                    InternalExecute(solution.Right);
                }
            }
            else
            {
                Parallel.ForEach(solutions, ParallelOptions, solution =>
                {
                    InternalExecute(solution.Left);
                    InternalExecute(solution.Right);
                });
            }
        }

        /// <summary>
        /// Checks if the derived solution matches the original.
        /// </summary>
        /// <param name="originalSolution">Original Solution.</param>
        /// <param name="derivedSolution">Derived Solution.</param>
        /// <returns>Whether the derived Solution matches the original Solution or not.</returns>
        private static bool Check(Solution originalSolution, Solution derivedSolution)
        {
            if (originalSolution.Content.Base != MorphBase.Unknown && originalSolution.Content.Base != derivedSolution.Content.Base) return false;

            if (originalSolution.Left != null && derivedSolution.Left is null) return false;

            if (originalSolution.Left != null)
            {
                if (originalSolution.Left.Entry != derivedSolution.Left.Entry) return false;

                if (originalSolution.Left.Solutions[0].Content.Id != 0 && originalSolution.Left.Solutions[0].Content.Id != derivedSolution.Left.Solutions[0].Content.Id) return false;
            }

            if (originalSolution.Right != null && derivedSolution.Right is null) return false;

            if (originalSolution.Right != null)
            {
                if (originalSolution.Right.Entry != derivedSolution.Right.Entry) return false;

                if (originalSolution.Right.Solutions[0].Content.Id != 0 && originalSolution.Right.Solutions[0].Content.Id != derivedSolution.Right.Solutions[0].Content.Id) return false;
            }

            int length = originalSolution.Content.Parameters.Length;

            for (int i = 0; i < length; i++)
            {
                if (originalSolution.Content.Parameters[i] != 0 && originalSolution.Content.Parameters[i] != derivedSolution.Content.Parameters[i]) return false;
            }

            return true;
        }
    }
}
