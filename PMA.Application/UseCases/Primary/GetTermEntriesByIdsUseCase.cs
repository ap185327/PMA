// <copyright file="GetTermEntriesByIdsUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.UseCases.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get term entries by IDs use case class.
    /// </summary>
    public sealed class GetTermEntriesByIdsUseCase : UseCaseWithResultBase<GetTermEntriesByIdsUseCase, GetTermEntriesByIdsInputPort, IList<string>>, IGetTermEntriesByIdsUseCase
    {
        /// <summary>
        /// The term database provider.
        /// </summary>
        private readonly ITermDbProvider _termDbProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="GetTermEntriesByIdsUseCase"/> class.
        /// </summary>
        /// <param name="termDbProvider">The term database provider.</param>
        /// <param name="logger">The logger.</param>
        public GetTermEntriesByIdsUseCase(ITermDbProvider termDbProvider, ILogger<GetTermEntriesByIdsUseCase> logger) : base(logger)
        {
            _termDbProvider = termDbProvider;
        }

        #region Overrides of UseCaseWithResultBase<GetTermEntriesByIdsUseCase,GetTermEntriesByIdsInputPort,IList<string>>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<IList<string>> Execute(GetTermEntriesByIdsInputPort inputData)
        {
            try
            {
                var termEntries = inputData.TermIds.Count == 0
                    ? new List<string> { _termDbProvider.GetValues().Single(x => x.Id == MorphConstants.UnknownTermId).Entry }
                    : inputData.UseAltPropertyEntry
                        ? inputData.TermIds.Select(x => _termDbProvider.GetValues().Single(y => y.Id == x).AltPropertyEntry).ToList()
                        : inputData.TermIds.Select(x => _termDbProvider.GetValues().Single(y => y.Id == x).Entry).ToList();

                return OperationResult<IList<string>>.SuccessResult(termEntries);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<IList<string>>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<IList<string>>> ExecuteAsync(GetTermEntriesByIdsInputPort inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
