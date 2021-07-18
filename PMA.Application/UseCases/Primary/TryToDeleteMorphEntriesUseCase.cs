// <copyright file="TryToDeleteMorphEntriesUseCase.cs" company="Andrey Pospelov">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The try to delete morphological entries use case class.
    /// </summary>
    public sealed class TryToDeleteMorphEntriesUseCase : UseCaseWithResultBase<TryToDeleteMorphEntriesUseCase, IList<int>, IList<DeleteMorphEntryOutputPort>>, ITryToDeleteMorphEntriesUseCase
    {
        /// <summary>
        /// The morphological entry database provider.
        /// </summary>
        private readonly IMorphEntryDbProvider _morphEntryDbProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="TryToDeleteMorphEntriesUseCase"/> class.
        /// </summary>
        /// <param name="morphEntryDbProvider">The morphological entry database provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public TryToDeleteMorphEntriesUseCase(IMorphEntryDbProvider morphEntryDbProvider, IMediator mediator, ParallelOptions parallelOptions, ILogger<TryToDeleteMorphEntriesUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _morphEntryDbProvider = morphEntryDbProvider;

            Logger.LogInit();
        }

        #region Overrides of UseCaseWithResultBase<TryToDeleteMorphEntriesUseCase,IList<int>,IList<DeleteMorphEntryOutputPort>>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IList<DeleteMorphEntryOutputPort>> Execute(IList<int> inputData)
        {
            if (inputData is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData));
                return OperationResult<IList<DeleteMorphEntryOutputPort>>.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData));
            }

            try
            {
                var outputPort = new List<DeleteMorphEntryOutputPort>();

                foreach (int id in inputData)
                {
                    if (id < 0)
                    {
                        Logger.LogError(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputData);
                        return OperationResult<IList<DeleteMorphEntryOutputPort>>.FailureResult(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputData);
                    }

                    var morphEntry = _morphEntryDbProvider.GetValues().Single(x => x.Id == id);
                    var parentMorphEntries = _morphEntryDbProvider.GetValues()
                        .Where(x => x.Left == morphEntry || x.Right == morphEntry).ToList();

                    if (parentMorphEntries.Count > 0)
                    {
                        outputPort.Add(new DeleteMorphEntryOutputPort
                        {
                            Id = id,
                            IsDeleted = false,
                            MorphEntries = parentMorphEntries
                        });
                    }
                    else
                    {
                        _morphEntryDbProvider.Delete(morphEntry);
                        outputPort.Add(new DeleteMorphEntryOutputPort
                        {
                            Id = id,
                            IsDeleted = true
                        });
                    }
                }

                return OperationResult<IList<DeleteMorphEntryOutputPort>>.SuccessResult(outputPort);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<IList<DeleteMorphEntryOutputPort>>.ExceptionResult(exception);
            }
        }

        #endregion
    }
}
