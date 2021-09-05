// <copyright file="GetLayerForFirstNodeUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.UseCases.Primary;
using System;
using System.Threading;
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
        /// <param name="logger">The logger.</param>
        public GetLayerForFirstNodeUseCase(IFreqRatingManager freqRatingManager, ILogger<GetLayerForFirstNodeUseCase> logger) : base(logger)
        {
            _freqRatingManager = freqRatingManager;
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

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<uint>> ExecuteAsync(CancellationToken token = default)
        {
            return Task.FromResult(Execute());
        }

        #endregion
    }
}
