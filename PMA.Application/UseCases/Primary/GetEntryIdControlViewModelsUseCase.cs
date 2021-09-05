// <copyright file="GetEntryIdControlViewModelsUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Managers;
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
    /// The get entry ID control view models use case class.
    /// </summary>
    public sealed class GetEntryIdControlViewModelsUseCase : UseCaseWithResultBase<GetEntryIdControlViewModelsUseCase, MorphEntry, IList<IGetEntryIdControlViewModel>>, IGetEntryIdControlViewModelsUseCase
    {
        /// <summary>
        /// The morphological entry manager.
        /// </summary>
        private readonly IMorphEntryManager _morphEntryManager;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapperBase _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="GetEntryIdControlViewModelsUseCase"/> class.
        /// </summary>
        /// <param name="morphEntryManager">The morphological entry manager.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public GetEntryIdControlViewModelsUseCase(IMorphEntryManager morphEntryManager, IMapperBase mapper, ILogger<GetEntryIdControlViewModelsUseCase> logger) : base(logger)
        {
            _morphEntryManager = morphEntryManager;
            _mapper = mapper;
        }

        #region Overrides of UseCaseWithResultBase<GetEntryIdControlViewModelsUseCase,MorphEntry,IList<IGetEntryIdControlViewModel>>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IList<IGetEntryIdControlViewModel>> Execute(MorphEntry inputPort)
        {
            try
            {
                var morphEntries = _morphEntryManager.GetValues(inputPort.Entry, inputPort.Parameters, inputPort.Base, inputPort.IsVirtual, inputPort.Left, inputPort.Right).Where(x => x.Id > 0).ToList();

                var viewModels = morphEntries.Select(morphEntry => _mapper.Map<IGetEntryIdControlViewModel>(morphEntry)).ToList();

                return OperationResult<IList<IGetEntryIdControlViewModel>>.SuccessResult(viewModels);
            }
            catch (Exception exception)
            {
                return OperationResult<IList<IGetEntryIdControlViewModel>>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<IList<IGetEntryIdControlViewModel>>> ExecuteAsync(MorphEntry inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
