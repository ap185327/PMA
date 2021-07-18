// <copyright file="MainInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Utils.Extensions;

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
        /// The stop morphological analysis use case.
        /// </summary>
        private readonly IStopMorphAnalysisUseCase _stopMorphAnalysisUseCase;

        /// <summary>
        /// Initializes the new instance of <see cref="MainInteractor"/> class.
        /// </summary>
        /// <param name="startMorphAnalysisUseCase">The start morphological analysis use case.</param>
        /// <param name="stopMorphAnalysisUseCase">The stop morphological analysis use case.</param>
        /// <param name="logger">The logger.</param>
        public MainInteractor(IStartMorphAnalysisUseCase startMorphAnalysisUseCase, IStopMorphAnalysisUseCase stopMorphAnalysisUseCase, ILogger<MainInteractor> logger) : base(logger)
        {
            _startMorphAnalysisUseCase = startMorphAnalysisUseCase;
            _stopMorphAnalysisUseCase = stopMorphAnalysisUseCase;

            Logger.LogInit();
        }

        #region Implementation of IMainInteractor

        /// <summary>
        /// Starts the morphological parsing.
        /// </summary>
        /// <param name="inputData">The morphological parser input data.</param>
        /// <returns>The operation result.</returns>
        public OperationResult StartAnalysis(MorphParserInputPort inputData)
        {
            var result = _startMorphAnalysisUseCase.Execute(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Stops the morphological parsing.
        /// </summary>
        /// <returns>The operation result.</returns>
        public OperationResult StopAnalysis()
        {
            var result = _stopMorphAnalysisUseCase.Execute();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        #endregion
    }
}
