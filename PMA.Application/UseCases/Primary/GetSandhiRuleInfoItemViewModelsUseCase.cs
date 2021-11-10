// <copyright file="GetSandhiRuleInfoItemViewModelsUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get sandhi rule information item view models use case class.
    /// </summary>
    public sealed class GetSandhiRuleInfoItemViewModelsUseCase : UseCaseWithResultBase<GetSandhiRuleInfoItemViewModelsUseCase, IList<SandhiMatch>, IList<IRuleInfoItemViewModel>>, IGetSandhiRuleInfoItemViewModelsUseCase
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapperBase _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="GetSandhiRuleInfoItemViewModelsUseCase"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public GetSandhiRuleInfoItemViewModelsUseCase(IMapperBase mapper, ILogger<GetSandhiRuleInfoItemViewModelsUseCase> logger) : base(logger)
        {
            _mapper = mapper;
        }

        #region Overrides of UseCaseWithResultBase<GetSandhiRuleInfoItemViewModelsUseCase,IList<SandhiMatch>,IList<IRuleInfoItemViewModel>>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IList<IRuleInfoItemViewModel>> Execute(IList<SandhiMatch> inputPort)
        {
            var sandhiRules = new List<SandhiRule>();

            if (inputPort != null)
            {
                foreach (var sandhiMatch in inputPort)
                {
                    if (sandhiMatch.Rules != null)
                    {
                        sandhiRules.AddRange(sandhiMatch.Rules);
                    }
                }
            }

            try
            {
                var viewModels = sandhiRules.DistinctBy(x => x.Id).Select(sandhiRule => _mapper.Map<IRuleInfoItemViewModel>(sandhiRule)).ToList();

                return OperationResult<IList<IRuleInfoItemViewModel>>.SuccessResult(viewModels);
            }
            catch (Exception exception)
            {
                return OperationResult<IList<IRuleInfoItemViewModel>>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<IList<IRuleInfoItemViewModel>>> ExecuteAsync(IList<SandhiMatch> inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
