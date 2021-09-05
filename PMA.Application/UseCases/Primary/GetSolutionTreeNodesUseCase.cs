// <copyright file="GetSolutionTreeNodesUseCase.cs" company="Andrey Pospelov">
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
    /// The get solution tree nodes use case class.
    /// </summary>
    public sealed class GetSolutionTreeNodesUseCase : UseCaseWithResultBase<GetSolutionTreeNodesUseCase, IList<SolutionTreeNodeViewModelGroup>, IList<ISolutionTreeNodeViewModel>>, IGetSolutionTreeNodesUseCase
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapperBase _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="GetSolutionTreeNodesUseCase"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public GetSolutionTreeNodesUseCase(IMapperBase mapper, ILogger<GetSolutionTreeNodesUseCase> logger) : base(logger)
        {
            _mapper = mapper;
        }

        #region Overrides of UseCaseWithResultBase<GetSolutionTreeNodesUseCase,IList<Solution>,IList<ISolutionTreeNodeViewModel>>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IList<ISolutionTreeNodeViewModel>> Execute(IList<SolutionTreeNodeViewModelGroup> inputPort)
        {
            try
            {
                var viewModels = inputPort.Select(group => _mapper.Map<ISolutionTreeNodeViewModel>(group)).ToList();

                return OperationResult<IList<ISolutionTreeNodeViewModel>>.SuccessResult(viewModels);
            }
            catch (Exception exception)
            {
                return OperationResult<IList<ISolutionTreeNodeViewModel>>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<IList<ISolutionTreeNodeViewModel>>> ExecuteAsync(IList<SolutionTreeNodeViewModelGroup> inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
