// <copyright file="GetWordFormTreeNodeUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.ViewModels.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get wordform tree node use case class.
    /// </summary>
    public sealed class GetWordFormTreeNodeUseCase : UseCaseWithResultBase<GetWordFormTreeNodeUseCase, GetWordFormTreeNodeInputPort, IWordFormTreeNodeViewModel>, IGetWordFormTreeNodeUseCase
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapperBase _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="GetWordFormTreeNodeUseCase"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public GetWordFormTreeNodeUseCase(IMapperBase mapper, ILogger<GetWordFormTreeNodeUseCase> logger) : base(logger)
        {
            _mapper = mapper;
        }

        #region Overrides of UseCaseWithResultBase<GetMainTreeNodeUseCase,WordForm,IWordFormTreeNodeViewModel>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IWordFormTreeNodeViewModel> Execute(GetWordFormTreeNodeInputPort inputPort)
        {
            try
            {
                var viewModel = _mapper.Map<IWordFormTreeNodeViewModel>(inputPort);

                return OperationResult<IWordFormTreeNodeViewModel>.SuccessResult(viewModel);
            }
            catch (Exception exception)
            {
                return OperationResult<IWordFormTreeNodeViewModel>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<IWordFormTreeNodeViewModel>> ExecuteAsync(GetWordFormTreeNodeInputPort inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
