// <copyright file="ClearCacheUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.UseCases.Secondary;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="logger">The logger.</param>
        public ClearCacheUseCase(IFreqRatingManager freqRatingManager,
            IMorphEntryManager morphEntryManager,
            IMorphRuleManager morphRuleManager,
            IMorphCombinationManager morphCombinationManager,
            ILogger<ClearCacheUseCase> logger) : base(logger)
        {
            _freqRatingManager = freqRatingManager;
            _morphEntryManager = morphEntryManager;
            _morphRuleManager = morphRuleManager;
            _morphCombinationManager = morphCombinationManager;
        }

        #region Overrides of UseCaseBase<ClearCacheUseCase>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute()
        {
            _freqRatingManager.Clear();
            _morphEntryManager.Clear();
            _morphRuleManager.Clear();
            _morphCombinationManager.Clear();
            GC.Collect();

            return OperationResult.SuccessResult();
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult> ExecuteAsync(CancellationToken token = default)
        {
            return Task.FromResult(Execute());
        }

        #endregion
    }
}
