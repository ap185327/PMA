// <copyright file="IMorphRuleInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Interactors.Base;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Domain.Interfaces.Interactors.Primary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IMorphRuleInteractor"/> interfacing class.
    /// </summary>
    public interface IMorphRuleInteractor : IInteractor
    {
        /// <summary>
        /// Gets a collection of morphological rule information item view models.
        /// </summary>
        /// <param name="morphRules">The collection of morphological rules.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of morphological rule information item view models.</returns>
        Task<OperationResult<IList<IRuleInfoItemViewModel>>> GetMorphRuleInfoItemViewModelsAsync(IList<MorphRule> morphRules, CancellationToken token = default);

        /// <summary>
        /// Gets a collection of sandhi rule information item view models.
        /// </summary>
        /// <param name="sandhiMatches">The collection of sandhi matches.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The collection of sandhi rule information item view models.</returns>
        Task<OperationResult<IList<IRuleInfoItemViewModel>>> GetSandhiRuleInfoItemViewModelsAsync(IList<SandhiMatch> sandhiMatches, CancellationToken token = default);
    }
}
