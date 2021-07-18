// <copyright file="GetSimilarMorphEntryIdsUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.Models;
using PMA.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get similar morphological entry IDs use case class.
    /// </summary>
    public sealed class GetSimilarMorphEntryIdsUseCase : UseCaseWithResultBase<GetSimilarMorphEntryIdsUseCase, MorphEntry, IList<int>>, IGetSimilarMorphEntryIdsUseCase
    {
        /// <summary>
        /// The morphological entry manager.
        /// </summary>
        private readonly IMorphEntryManager _morphEntryManager;

        /// <summary>
        /// Initializes a new instance of <see cref="GetSimilarMorphEntryIdsUseCase"/> class.
        /// </summary>
        /// <param name="morphEntryManager">The morphological entry manager.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public GetSimilarMorphEntryIdsUseCase(IMorphEntryManager morphEntryManager, IMediator mediator, ParallelOptions parallelOptions, ILogger<GetSimilarMorphEntryIdsUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _morphEntryManager = morphEntryManager;

            Logger.LogInit();
        }

        #region Overrides of UseCaseWithResultBase<GetSimilarMorphEntryIdsUseCase,MorphEntry,IList<int>>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IList<int>> Execute(MorphEntry inputData)
        {
            if (inputData is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData));
                return OperationResult<IList<int>>.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData));
            }

            try
            {
                var morphEntries = _morphEntryManager.GetValues(
                    inputData.Entry,
                    inputData.Parameters,
                    inputData.Base,
                    inputData.IsVirtual,
                    inputData.Left,
                    inputData.Right);

                var ids = morphEntries.Select(x => x.Id).ToList();

                return OperationResult<IList<int>>.SuccessResult(ids);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<IList<int>>.ExceptionResult(exception);
            }
        }

        #endregion
    }
}
