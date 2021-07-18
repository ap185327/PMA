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
using PMA.Utils.Extensions;

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

            Logger.LogInit();
        }

        #region Implementation of IOptionViewModelInteractor

        /// <summary>
        /// Gets current option values.
        /// </summary>
        /// <returns>The operation result.</returns>
        public OperationResult<OptionValueOutputPort> GetCurrentOptionValues()
        {
            var result = _getCurrentOptionValuesUseCase.Execute();

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        /// <summary>
        /// Saves option values.
        /// </summary>
        /// <param name="inputData">Options values.</param>
        /// <returns>The operation result.</returns>
        public OperationResult SaveOptionValues(OptionValueInputPort inputData)
        {
            var result = _saveOptionValuesUseCase.Execute(inputData);

            if (!result.Success)
            {
                Logger.LogErrors(result.Messages);
            }

            return result;
        }

        #endregion
    }
}
