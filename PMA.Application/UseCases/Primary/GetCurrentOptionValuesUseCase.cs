// <copyright file="GetCurrentOptionValuesUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.DataContracts;
using PMA.Domain.Interfaces.Providers;
using PMA.Domain.Interfaces.Services;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Domain.OutputPorts;
using PMA.Utils.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The get current option values use case class.
    /// </summary>
    public sealed class GetCurrentOptionValuesUseCase : UseCaseWithResultBase<GetCurrentOptionValuesUseCase, OptionValueOutputPort>, IGetCurrentOptionValuesUseCase
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
        /// Initializes a new instance of <see cref="GetCurrentOptionValuesUseCase"/> class.
        /// </summary>
        /// <param name="termDbProvider">The term database provider.</param>
        /// <param name="morphParameterDbProvider">The morphological parameter database provider.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public GetCurrentOptionValuesUseCase(ITermDbProvider termDbProvider,
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

                var rootTerms = _termDbProvider.GetValues().Select(x => x.Entry).ToList();

                int rootTermId = _settingService.GetValue<int>("Options.RootTermId");

                int selectedRootTermIndex = rootTerms.IndexOf(_termDbProvider.GetValues().Single(x => x.Id == rootTermId).Entry);

                var availableTerms = _morphParameterDbProvider.GetValues().Where(x => x.IsVisible == false).Select(x => x.Name);

                var shownTerms = _morphParameterDbProvider.GetValues().Where(x => x.IsVisible).Select(x => x.Name);

                double freqRatingRatio = _settingService.GetValue<double>("Options.FreqRatingRatio");

                return OperationResult<OptionValueOutputPort>.SuccessResult(new OptionValueOutputPort
                {
                    DebugMode = debugMode,
                    RootTerms = rootTerms,
                    SelectedRootTermIndex = selectedRootTermIndex,
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

        #endregion
    }
}
