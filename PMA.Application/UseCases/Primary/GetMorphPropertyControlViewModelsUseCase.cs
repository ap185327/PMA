// <copyright file="GetMorphPropertyControlViewModelsUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get morphological property control view models use case class.
    /// </summary>
    public sealed class GetMorphPropertyControlViewModelsUseCase : UseCaseWithResultBase<GetMorphPropertyControlViewModelsUseCase, IList<IMorphPropertyControlViewModel>>, IGetMorphPropertyControlViewModelsUseCase
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapperBase _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="GetMorphPropertyControlViewModelsUseCase"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public GetMorphPropertyControlViewModelsUseCase(IMapperBase mapper, ILogger<GetMorphPropertyControlViewModelsUseCase> logger) : base(logger)
        {
            _mapper = mapper;
        }

        #region Overrides of UseCaseWithResultBase<GetMorphPropertyControlViewModelsUseCase,IEnumerable<IMorphPropertyControlViewModel>>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IList<IMorphPropertyControlViewModel>> Execute()
        {
            try
            {
                var properties = new List<IMorphPropertyControlViewModel>();

                for (int i = 0; i < MorphConstants.ParameterCount; i++)
                {
                    properties.Add(_mapper.Map<IMorphPropertyControlViewModel>(i));
                }

                return OperationResult<IList<IMorphPropertyControlViewModel>>.SuccessResult(properties);
            }
            catch (Exception exception)
            {
                return OperationResult<IList<IMorphPropertyControlViewModel>>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<IList<IMorphPropertyControlViewModel>>> ExecuteAsync(CancellationToken token = default)
        {
            return Task.FromResult(Execute());
        }

        #endregion
    }
}
