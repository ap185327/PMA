// <copyright file="OptionInteractor.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.Interactors.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Interactors.Primary;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.OutputPorts;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.Interactors.Primary
{
    /// <summary>
    /// The option interactor class.
    /// </summary>
    public sealed class OptionInteractor : InteractorBase<OptionInteractor>, IOptionInteractor
    {
        /// <summary>
        /// The get current option values use case.
        /// </summary>
        private readonly IGetCurrentOptionValuesUseCase _getCurrentOptionValuesUseCase;

        /// <summary>
        /// The save option values use case.
        /// </summary>
        private readonly ISaveOptionValuesUseCase _saveOptionValuesUseCase;

        /// <summary>
        /// Initializes a new instance of <see cref="OptionInteractor"/> class.
        /// </summary>
        /// <param name="getCurrentOptionValuesUseCase">The get current option values use case.</param>
        /// <param name="saveOptionValuesUseCase">The save option values use case.</param>
        /// <param name="logger">The logger.</param>
        public OptionInteractor(IGetCurrentOptionValuesUseCase getCurrentOptionValuesUseCase,
            ISaveOptionValuesUseCase saveOptionValuesUseCase,
            ILogger<OptionInteractor> logger) : base(logger)
        {
            _getCurrentOptionValuesUseCase = getCurrentOptionValuesUseCase;
            _saveOptionValuesUseCase = saveOptionValuesUseCase;
        }

        #region Implementation of IOptionInteractor

        /// <summary>
        /// Gets current option values.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The option value output port.</returns>
        public async Task<OperationResult<OptionValueOutputPort>> GetCurrentOptionValuesAsync(CancellationToken token = default)
        {
            return await _getCurrentOptionValuesUseCase.ExecuteAsync(token);
        }

        /// <summary>
        /// Saves option values.
        /// </summary>
        /// <param name="inputData">Options values.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The operation result.</returns>
        public async Task<OperationResult> SaveOptionValuesAsync(OptionValueInputPort inputData, CancellationToken token = default)
        {
            return await _saveOptionValuesUseCase.ExecuteAsync(inputData, token);
        }

        #endregion
    }
}
