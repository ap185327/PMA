// <copyright file="ClearCacheUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.UseCases.Secondary;
using PMA.Utils.Extensions;

namespace PMA.Application.UseCases.Secondary
{
    /// <summary>
    /// The clear cache use case class.
    /// </summary>
    public sealed class ClearCacheUseCase : UseCaseBase<ClearCacheUseCase>, IClearCacheUseCase
    {
        /// <summary>
        /// The frequency rating manager.
        /// </summary>
        private readonly IFreqRatingManager _freqRatingManager;

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
        /// Initializes a new instance of <see cref="ClearCacheUseCase"/> class.
        /// </summary>
        /// <param name="freqRatingManager">The frequency rating manager.</param>
        /// <param name="morphEntryManager">The morphological entry manager.</param>
        /// <param name="morphRuleManager">The morphological rule manager.</param>
        /// <param name="morphCombinationManager">The morphological combination manager.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public ClearCacheUseCase(IFreqRatingManager freqRatingManager,
            IMorphEntryManager morphEntryManager,
            IMorphRuleManager morphRuleManager,
            IMorphCombinationManager morphCombinationManager,
            IMediator mediator,
            ParallelOptions parallelOptions,
            ILogger<ClearCacheUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _freqRatingManager = freqRatingManager;
            _morphEntryManager = morphEntryManager;
            _morphRuleManager = morphRuleManager;
            _morphCombinationManager = morphCombinationManager;

            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<ClearCacheUseCase>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute()
        {
            Logger.LogEntry();

            _freqRatingManager.Clear();
            _morphEntryManager.Clear();
            _morphRuleManager.Clear();
            _morphCombinationManager.Clear();
            GC.Collect();

            Logger.LogExit();
            return OperationResult.SuccessResult();
        }

        #endregion
    }
}
