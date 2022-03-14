// <copyright file="MainInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.Interactors.Primary
{
    /// <summary>
    /// The main interactor class.
    /// </summary>
    public sealed class MainInteractor : InteractorBase<MainInteractor>, IMainInteractor
    {
        /// <summary>
        /// The start morphological analysis use case.
        /// </summary>
        private readonly IStartMorphAnalysisUseCase _startMorphAnalysisUseCase;

        /// <summary>
        /// Initializes the new instance of <see cref="MainInteractor"/> class.
        /// </summary>
        /// <param name="startMorphAnalysisUseCase">The start morphological analysis use case.</param>
        /// <param name="logger">The logger.</param>
        public MainInteractor(IStartMorphAnalysisUseCase startMorphAnalysisUseCase, ILogger<MainInteractor> logger) : base(logger)
        {
            _startMorphAnalysisUseCase = startMorphAnalysisUseCase;
        }

        #region Implementation of IMainInteractor

        /// <summary>
        /// Starts the morphological parsing.
        /// </summary>
        /// <param name="inputData">The morphological parser input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        public async Task<OperationResult> StartAnalysisAsync(MorphParserInputPort inputData, CancellationToken token = default)
        {
            return await _startMorphAnalysisUseCase.ExecuteAsync(inputData, token);
        }

        #endregion
    }
}
