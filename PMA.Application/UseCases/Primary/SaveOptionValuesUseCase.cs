// <copyright file="SaveOptionValuesUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Utils.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The save option values use case class.
    /// </summary>
    public sealed class SaveOptionValuesUseCase : UseCaseBase<GetCurrentOptionValuesUseCase, OptionValueInputPort>, ISaveOptionValuesUseCase
    {
        /// <summary>
        /// The term database provider.
        /// </summary>
        private readonly ITermDbProvider _termDbProvider;

        /// <summary>
        /// The morphological parameter database provider.
        /// </summary>
        private readonly IMorphParameterDbProvider _morphParameterDbProvider;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// Initializes a new instance of <see cref="UseCaseBase{T, TInput}"/> class.
        /// </summary>
        /// <param name="termDbProvider">The term database provider.</param>
        /// <param name="morphParameterDbProvider">The morphological parameter database provider.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public SaveOptionValuesUseCase(ITermDbProvider termDbProvider,
            IMorphParameterDbProvider morphParameterDbProvider,
            ISettingService settingService,
            IMediator mediator,
            ParallelOptions parallelOptions,
            ILogger<GetCurrentOptionValuesUseCase> logger) : base(mediator,
            parallelOptions,
            logger)
        {
            _termDbProvider = termDbProvider;
            _morphParameterDbProvider = morphParameterDbProvider;
            _settingService = settingService;

            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<GetCurrentOptionValuesUseCase,OptionValueInputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(OptionValueInputPort inputData)
        {
            if (inputData is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData));
            }

            try
            {
                _settingService.SetValue("Options.DebugMode", inputData.DebugMode);

                _settingService.SetValue("Options.RootTermId", _termDbProvider.GetValues().Single(x => x.Entry == inputData.RootTerms[inputData.SelectedRootTermIndex]).Id);

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
                Logger.LogError(exception.Message);
                return OperationResult.ExceptionResult(exception);
            }
        }

        #endregion
    }
}
