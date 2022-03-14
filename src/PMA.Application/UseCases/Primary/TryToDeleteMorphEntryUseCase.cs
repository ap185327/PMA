// <copyright file="TryToDeleteMorphEntryUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.OutputPorts;
using System;
using System.Linq;
using System.Threading;
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
        /// <param name="logger">The logger.</param>
        public TryToDeleteMorphEntryUseCase(IMorphEntryDbProvider morphEntryDbProvider, ILogger<TryToDeleteMorphEntryUseCase> logger) : base(logger)
        {
            _morphEntryDbProvider = morphEntryDbProvider;
        }

        #region Overrides of UseCaseWithResultBase<TryToDeleteMorphEntryUseCase,int,DeleteMorphEntryOutputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<DeleteMorphEntryOutputPort> Execute(int inputPort)
        {
            try
            {
                var morphEntry = _morphEntryDbProvider.GetValues().Single(x => x.Id == inputPort);
                var parentMorphEntries = _morphEntryDbProvider.GetValues()
                    .Where(x => x.Left == morphEntry || x.Right == morphEntry).ToList();

                if (parentMorphEntries.Count > 0)
                {
                    return OperationResult<DeleteMorphEntryOutputPort>.SuccessResult(new DeleteMorphEntryOutputPort
                    {
                        Id = inputPort,
                        IsDeleted = false,
                        MorphEntries = parentMorphEntries
                    });
                }

                _morphEntryDbProvider.Delete(morphEntry);

                return OperationResult<DeleteMorphEntryOutputPort>.SuccessResult(new DeleteMorphEntryOutputPort
                {
                    Id = inputPort,
                    IsDeleted = true
                });
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<DeleteMorphEntryOutputPort>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<DeleteMorphEntryOutputPort>> ExecuteAsync(int inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
