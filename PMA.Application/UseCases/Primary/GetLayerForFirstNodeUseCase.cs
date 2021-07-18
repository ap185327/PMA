// <copyright file="GetLayerForFirstNodeUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Utils.Extensions;
using System;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get a chronological layer for the first tree node use case class.
    /// </summary>
    public sealed class GetLayerForFirstNodeUseCase : UseCaseWithResultBase<GetLayerForFirstNodeUseCase, uint>, IGetLayerForFirstNodeUseCase
    {
        /// <summary>
        /// The frequency rating manager.
        /// </summary>
        private readonly IFreqRatingManager _freqRatingManager;

        /// <summary>
        /// Initializes a new instance of <see cref="GetLayerForFirstNodeUseCase"/> class.
        /// </summary>
        /// <param name="freqRatingManager">The frequency rating manager.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public GetLayerForFirstNodeUseCase(IFreqRatingManager freqRatingManager, IMediator mediator, ParallelOptions parallelOptions, ILogger<GetLayerForFirstNodeUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _freqRatingManager = freqRatingManager;

            Logger.LogInit();
        }

        #region Overrides of UseCaseWithResultBase<GetLayerForFirstNodeUseCase,int>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<uint> Execute()
        {
            try
            {
                uint layer = _freqRatingManager.GetLayer();

                return OperationResult<uint>.SuccessResult(layer);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<uint>.ExceptionResult(exception);
            }
        }

        #endregion
    }
}
