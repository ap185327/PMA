// <copyright file="MorphRuleInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.Interactors.Primary
{
    /// <summary>
    /// The morphological rule interactor class.
    /// </summary>
    public sealed class MorphRuleInteractor : InteractorBase<MorphRuleInteractor>, IMorphRuleInteractor
    {
        /// <summary>
        /// The get morphological rule information item view models use case.
        /// </summary>
        private readonly IGetMorphRuleInfoItemViewModelsUseCase _getMorphRuleInfoItemViewModelsUseCase;

        /// <summary>
        /// The get sandhi rule information item view models use case.
        /// </summary>
        private readonly IGetSandhiRuleInfoItemViewModelsUseCase _sandhiRuleInfoItemViewModelsUseCase;

        /// <summary>
        /// Initializes a new instance of <see cref="MorphRuleInteractor"/> class.
        /// </summary>
        /// <param name="getMorphRuleInfoItemViewModelsUseCase"></param>
        /// <param name="sandhiRuleInfoItemViewModelsUseCase"></param>
        /// <param name="logger">The logger.</param>
        public MorphRuleInteractor(IGetMorphRuleInfoItemViewModelsUseCase getMorphRuleInfoItemViewModelsUseCase,
            IGetSandhiRuleInfoItemViewModelsUseCase sandhiRuleInfoItemViewModelsUseCase,
            ILogger<MorphRuleInteractor> logger) : base(logger)
        {
            _getMorphRuleInfoItemViewModelsUseCase = getMorphRuleInfoItemViewModelsUseCase;
            _sandhiRuleInfoItemViewModelsUseCase = sandhiRuleInfoItemViewModelsUseCase;
        }

        #region Implementation of IMorphRuleInteractor

        /// <summary>
        /// Gets a collection of morphological rule information item view models.
        /// </summary>
        /// <param name="morphRules">The collection of morphological rules.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of morphological rule information item view models.</returns>
        public async Task<OperationResult<IList<IRuleInfoItemViewModel>>> GetMorphRuleInfoItemViewModelsAsync(IList<MorphRule> morphRules, CancellationToken token = default)
        {
            return await _getMorphRuleInfoItemViewModelsUseCase.ExecuteAsync(morphRules, token);
        }

        /// <summary>
        /// Gets a collection of sandhi rule information item view models.
        /// </summary>
        /// <param name="sandhiMatches">The collection of sandhi matches.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of sandhi rule information item view models.</returns>
        public async Task<OperationResult<IList<IRuleInfoItemViewModel>>> GetSandhiRuleInfoItemViewModelsAsync(IList<SandhiMatch> sandhiMatches, CancellationToken token = default)
        {
            return await _sandhiRuleInfoItemViewModelsUseCase.ExecuteAsync(sandhiMatches, token);
        }

        #endregion
    }
}
