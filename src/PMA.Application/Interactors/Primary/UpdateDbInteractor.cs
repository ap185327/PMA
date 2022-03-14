// <copyright file="UpdateDbInteractor.cs" company="Andrey Pospelov">
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
    /// The update database interactor class.
    /// </summary>
    public sealed class UpdateDbInteractor : InteractorBase<UpdateDbInteractor>, IUpdateDbInteractor
    {
        /// <summary>
        /// The start database updating use case.
        /// </summary>
        private readonly IStartDbUpdatingUseCase _startDbUpdatingUseCase;

        /// <summary>
        /// Initializes the new instance of <see cref="UpdateDbInteractor"/> class.
        /// </summary>
        /// <param name="startDbUpdatingUseCase">The start database updating use case.</param>
        /// <param name="logger">The logger.</param>
        public UpdateDbInteractor(IStartDbUpdatingUseCase startDbUpdatingUseCase, ILogger<UpdateDbInteractor> logger) : base(logger)
        {
            _startDbUpdatingUseCase = startDbUpdatingUseCase;
        }

        #region Implementation of IUpdateDbInteractor

        /// <summary>
        /// Starts the update database process.
        /// </summary>
        /// <param name="inputData">The update database input port.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        public async Task<OperationResult> StartDbUpdatingAsync(UpdateDbInputPort inputData, CancellationToken token = default)
        {
            return await _startDbUpdatingUseCase.ExecuteAsync(inputData, token);
        }

        #endregion
    }
}
