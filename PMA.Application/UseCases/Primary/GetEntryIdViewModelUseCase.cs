// <copyright file="GetEntryIdViewModelUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Interfaces.ViewModels;
using PMA.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get entry ID view models use case class.
    /// </summary>
    public sealed class GetEntryIdViewModelUseCase : UseCaseWithResultBase<GetEntryIdViewModelUseCase, MorphEntry, IGetEntryIdViewModel>, IGetEntryIdViewModelUseCase
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapperBase _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="GetEntryIdViewModelUseCase"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public GetEntryIdViewModelUseCase(IMapperBase mapper, ILogger<GetEntryIdViewModelUseCase> logger) : base(logger)
        {
            _mapper = mapper;
        }

        #region Overrides of UseCaseWithResultBase<GetEntryIdViewModelUseCase,MorphEntry,IGetEntryIdViewModel>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IGetEntryIdViewModel> Execute(MorphEntry inputPort)
        {
            try
            {
                var viewModel = _mapper.Map<IGetEntryIdViewModel>(inputPort);

                return OperationResult<IGetEntryIdViewModel>.SuccessResult(viewModel);
            }
            catch (Exception exception)
            {
                return OperationResult<IGetEntryIdViewModel>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<IGetEntryIdViewModel>> ExecuteAsync(MorphEntry inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
