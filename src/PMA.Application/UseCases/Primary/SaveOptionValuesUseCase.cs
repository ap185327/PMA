// <copyright file="SaveOptionValuesUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Primary;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The save option values use case class.
    /// </summary>
    public sealed class SaveOptionValuesUseCase : UseCaseBase<GetCurrentOptionValuesUseCase, OptionValueInputPort>, ISaveOptionValuesUseCase
    {
        /// <summary>
        /// The morphological parameter database provider.
        /// </summary>
        private readonly IMorphParameterDbProvider _morphParameterDbProvider;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// Initializes a new instance of <see cref="SaveOptionValuesUseCase"/> class.
        /// </summary>
        /// <param name="morphParameterDbProvider">The morphological parameter database provider.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="logger">The logger.</param>
        public SaveOptionValuesUseCase(IMorphParameterDbProvider morphParameterDbProvider,
            ISettingService settingService,
            ILogger<GetCurrentOptionValuesUseCase> logger) : base(logger)
        {
            _morphParameterDbProvider = morphParameterDbProvider;
            _settingService = settingService;
        }

        #region Overrides of UseCaseBase<GetCurrentOptionValuesUseCase,OptionValueInputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(OptionValueInputPort inputData)
        {
            try
            {
                _settingService.SetValue("Options.DebugMode", inputData.DebugMode);

                foreach (string name in inputData.AvailableTerms)
                {
                    var morphParameter = _morphParameterDbProvider.GetValues().Single(x => x.Name == name);

                    _settingService.SetValue($"MorphParameters[{morphParameter.Id}].IsVisible", false);
                }

                foreach (string name in inputData.ShownTerms)
                {
                    var morphParameter = _morphParameterDbProvider.GetValues().Single(x => x.Name == name);

                    _settingService.SetValue($"MorphParameters[{morphParameter.Id}].IsVisible", true);
                }

                _settingService.SetValue("Options.FreqRatingRatio", inputData.FreqRatingRatio);

                _morphParameterDbProvider.UpdateVisibility();

                return OperationResult.SuccessResult();
            }
            catch (Exception exception)
            {
                return OperationResult.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputPort">The input data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult> ExecuteAsync(OptionValueInputPort inputPort, CancellationToken token = default)
        {
            return Task.FromResult(Execute(inputPort));
        }

        #endregion
    }
}
