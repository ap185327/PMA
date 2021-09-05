// <copyright file="GetMorphRuleInfoItemViewModelsUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.ViewModels.Controls;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get morphological rule information item view models use case class.
    /// </summary>
    public sealed class GetMorphRuleInfoItemViewModelsUseCase : UseCaseWithResultBase<GetMorphRuleInfoItemViewModelsUseCase, IList<MorphRule>, IList<IRuleInfoItemViewModel>>, IGetMorphRuleInfoItemViewModelsUseCase
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapperBase _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="UseCaseWithResultBase{T, TInput, TResult}"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public GetMorphRuleInfoItemViewModelsUseCase(IMapperBase mapper, ILogger<GetMorphRuleInfoItemViewModelsUseCase> logger) : base(logger)
        {
            _mapper = mapper;
        }

        #region Overrides of UseCaseWithResultBase<GetMorphRuleInfoItemViewModelsUseCase,IList<MorphRule>,IList<IRuleInfoItemViewModel>>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IList<IRuleInfoItemViewModel>> Execute(IList<MorphRule> inputPort)
        {
            try
            {
                var viewModels = inputPort.DistinctBy(x => x.Id).Select(morphRule => _mapper.Map<IRuleInfoItemViewModel>(morphRule)).ToList();

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
        public override Task<OperationResult<IList<IRuleInfoItemViewModel>>> ExecuteAsync(IList<MorphRule> inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
