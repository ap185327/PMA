// <copyright file="GetMorphCategoryControlViewModelsUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get morphological category control view models use case class.
    /// </summary>
    public sealed class GetMorphCategoryControlViewModelsUseCase : UseCaseWithResultBase<GetMorphCategoryControlViewModelsUseCase, IList<IMorphPropertyControlViewModel>, IList<IMorphCategoryControlViewModel>>, IGetMorphCategoryControlViewModelsUseCase
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapperBase _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="GetMorphCategoryControlViewModelsUseCase"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public GetMorphCategoryControlViewModelsUseCase(IMapperBase mapper, ILogger<GetMorphCategoryControlViewModelsUseCase> logger) : base(logger)
        {
            _mapper = mapper;
        }

        #region Overrides of UseCaseWithResultBase<GetMorphCategoryControlViewModelsUseCase,IList<IMorphPropertyControlViewModel>,IList<IMorphCategoryControlViewModel>>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IList<IMorphCategoryControlViewModel>> Execute(IList<IMorphPropertyControlViewModel> inputPort)
        {
            try
            {
                var groups = inputPort.GroupBy(x => x.Category);

                var categories = groups.Select(group => _mapper.Map<IMorphCategoryControlViewModel>(group.ToList())).ToList();

                return OperationResult<IList<IMorphCategoryControlViewModel>>.SuccessResult(categories);
            }
            catch (Exception exception)
            {
                return OperationResult<IList<IMorphCategoryControlViewModel>>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<IList<IMorphCategoryControlViewModel>>> ExecuteAsync(IList<IMorphPropertyControlViewModel> inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
