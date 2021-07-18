// <copyright file="TryToDeleteMorphEntryUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.OutputPorts;
using PMA.Utils.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The try to delete morphological entry use case class.
    /// </summary>
    public sealed class TryToDeleteMorphEntryUseCase : UseCaseWithResultBase<TryToDeleteMorphEntryUseCase, int, DeleteMorphEntryOutputPort>, ITryToDeleteMorphEntryUseCase
    {
        /// <summary>
        /// The morphological entry database provider.
        /// </summary>
        private readonly IMorphEntryDbProvider _morphEntryDbProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="TryToDeleteMorphEntryUseCase"/> class.
        /// </summary>
        /// <param name="morphEntryDbProvider">The morphological entry database provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public TryToDeleteMorphEntryUseCase(IMorphEntryDbProvider morphEntryDbProvider, IMediator mediator, ParallelOptions parallelOptions, ILogger<TryToDeleteMorphEntryUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _morphEntryDbProvider = morphEntryDbProvider;

            Logger.LogInit();
        }

        #region Overrides of UseCaseWithResultBase<TryToDeleteMorphEntryUseCase,int,DeleteMorphEntryOutputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<DeleteMorphEntryOutputPort> Execute(int inputData)
        {
            if (inputData < 0)
            {
                Logger.LogError(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputData);
                return OperationResult<DeleteMorphEntryOutputPort>.FailureResult(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputData);
            }

            try
            {
                var morphEntry = _morphEntryDbProvider.GetValues().Single(x => x.Id == inputData);
                var parentMorphEntries = _morphEntryDbProvider.GetValues()
                    .Where(x => x.Left == morphEntry || x.Right == morphEntry).ToList();

                if (parentMorphEntries.Count > 0)
                {
                    return OperationResult<DeleteMorphEntryOutputPort>.SuccessResult(new DeleteMorphEntryOutputPort
                    {
                        Id = inputData,
                        IsDeleted = false,
                        MorphEntries = parentMorphEntries
                    });
                }

                _morphEntryDbProvider.Delete(morphEntry);

                return OperationResult<DeleteMorphEntryOutputPort>.SuccessResult(new DeleteMorphEntryOutputPort
                {
                    Id = inputData,
                    IsDeleted = true
                });
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<DeleteMorphEntryOutputPort>.ExceptionResult(exception);
            }
        }

        #endregion
    }
}
