// <copyright file="UpdateDbInteractor.cs" company="Andrey Pospelov">
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
    /// The update database interactor class.
    /// </summary>
    public sealed class UpdateDbInteractor : InteractorBase<UpdateDbInteractor>, IUpdateDbInteractor
    {
        /// <summary>
        /// The start database updating use case.
        /// </summary>
        private readonly IStartDbUpdatingUseCase _startDbUpdatingUseCase;

        /// <summary>
        /// The stop database updating use case.
        /// </summary>
        private readonly IStopDbUpdatingUseCase _stopDbUpdatingUseCase;

        /// <summary>
        /// Initializes the new instance of <see cref="UpdateDbInteractor"/> class.
        /// </summary>
        /// <param name="startDbUpdatingUseCase">The start database updating use case.</param>
        /// <param name="stopDbUpdatingUseCase">The stop database updating use case.</param>
        /// <param name="logger">The logger.</param>
        public UpdateDbInteractor(IStartDbUpdatingUseCase startDbUpdatingUseCase, IStopDbUpdatingUseCase stopDbUpdatingUseCase, ILogger<UpdateDbInteractor> logger) : base(logger)
        {
            _startDbUpdatingUseCase = startDbUpdatingUseCase;
            _stopDbUpdatingUseCase = stopDbUpdatingUseCase;

            Logger.LogInit();
        }

        #region Implementation of IUpdateDbInteractor

        /// <summary>
        /// Starts the update database process.
        /// </summary>
        /// <param name="inputData">The update database input port.</param>
        public OperationResult StartDbUpdating(UpdateDbInputPort inputData)
        {
            var result = _startDbUpdatingUseCase.Execute(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Stops the update database process.
        /// </summary>
        public OperationResult StopDbUpdating()
        {
            var result = _stopDbUpdatingUseCase.Execute();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        #endregion
    }
}
