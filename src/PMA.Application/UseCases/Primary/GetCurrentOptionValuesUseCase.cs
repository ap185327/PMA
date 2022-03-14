// <copyright file="GetCurrentOptionValuesUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.OutputPorts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get current option values use case class.
    /// </summary>
    public sealed class GetCurrentOptionValuesUseCase : UseCaseWithResultBase<GetCurrentOptionValuesUseCase, OptionValueOutputPort>, IGetCurrentOptionValuesUseCase
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
        /// Initializes a new instance of <see cref="GetCurrentOptionValuesUseCase"/> class.
        /// </summary>
        /// <param name="morphParameterDbProvider">The morphological parameter database provider.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="logger">The logger.</param>
        public GetCurrentOptionValuesUseCase(IMorphParameterDbProvider morphParameterDbProvider,
            ISettingService settingService,
            ILogger<GetCurrentOptionValuesUseCase> logger) : base(logger)
        {
            _morphParameterDbProvider = morphParameterDbProvider;
            _settingService = settingService;
        }

        #region Overrides of UseCaseWithResultBase<GetCurrentOptionValuesUseCase,OptionValueOutputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <returns>The result of action execution.</returns>
        public override OperationResult<OptionValueOutputPort> Execute()
        {
            try
            {
                bool debugMode = _settingService.GetValue<bool>("Options.DebugMode");

                var availableTerms = _morphParameterDbProvider.GetValues().Where(x => x.IsVisible == false).Select(x => x.Name);

                var shownTerms = _morphParameterDbProvider.GetValues().Where(x => x.IsVisible).Select(x => x.Name);

                double freqRatingRatio = _settingService.GetValue<double>("Options.FreqRatingRatio");

                return OperationResult<OptionValueOutputPort>.SuccessResult(new OptionValueOutputPort
                {
                    DebugMode = debugMode,
                    AvailableTerms = availableTerms,
                    ShownTerms = shownTerms,
                    FreqRatingRatio = freqRatingRatio
                });
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                return OperationResult<OptionValueOutputPort>.ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of action execution.</returns>
        public override Task<OperationResult<OptionValueOutputPort>> ExecuteAsync(CancellationToken token = default)
        {
            return Task.FromResult(Execute());
        }

        #endregion
    }
}
