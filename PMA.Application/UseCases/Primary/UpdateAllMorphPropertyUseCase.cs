// <copyright file="UpdateAllMorphPropertyUseCase.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using MediatR;
using Microsoft.Extensions.Logging;
using PMA.Application.UseCases.Base;
using PMA.Domain.Constants;
using PMA.Domain.DataContracts;
using PMA.Domain.InputPorts;
using PMA.Domain.Interfaces.Managers;
using PMA.Domain.Interfaces.UseCases.Primary;
using PMA.Utils.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace PMA.Application.UseCases.Primary
{
    /// <summary>
    /// The update all morphological property use case class.
    /// </summary>
    public sealed class UpdateAllMorphPropertyUseCase : UseCaseBase<UpdateAllMorphPropertyUseCase, UpdateMorphPropertyInputPort>, IUpdateAllMorphPropertyUseCase
    {
        /// <summary>
        /// The morphological combination manager.
        /// </summary>
        private readonly IMorphCombinationManager _morphCombinationManager;

        /// <summary>
        /// Initializes a new instance of <see cref="UpdateAllMorphPropertyUseCase"/> class.
        /// </summary>
        /// <param name="morphCombinationManager">The morphological combination manager.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="parallelOptions">Options that configure the operation of methods on the <see cref="Parallel"/> class.</param>
        /// <param name="logger">The logger.</param>
        public UpdateAllMorphPropertyUseCase(IMorphCombinationManager morphCombinationManager, IMediator mediator, ParallelOptions parallelOptions, ILogger<UpdateAllMorphPropertyUseCase> logger) : base(mediator, parallelOptions, logger)
        {
            _morphCombinationManager = morphCombinationManager;

            Logger.LogInit();
        }

        #region Overrides of UseCaseBase<UpdateAllMorphPropertyUseCase,UpdateMorphPropertyInputPort>

        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="inputData">The input data.</param>
        /// <returns>The result of action execution.</returns>
        public override OperationResult Execute(UpdateMorphPropertyInputPort inputData)
        {
            if (inputData is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData));
            }

            if (inputData.Properties is null)
            {
                Logger.LogError(ErrorMessageConstants.ValueIsNull, nameof(inputData.Properties));
                return OperationResult.FailureResult(ErrorMessageConstants.ValueIsNull, nameof(inputData.Properties));
            }

            if (inputData.StartIndex is >= MorphConstants.ParameterCount or < 0)
            {
                Logger.LogError(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputData.StartIndex);
                return OperationResult.FailureResult(ErrorMessageConstants.MorphParameterIndexOutOfRange, inputData.StartIndex);
            }

            for (int i = inputData.StartIndex; i < MorphConstants.ParameterCount; i++)
            {
                byte[] parameters = inputData.Properties.Where(x => x.Index < i).OrderBy(x => x.Index).Select(x => x.TermIds[x.SelectedIndex]).ToArray();

                var combinations = _morphCombinationManager.GetValidParameters(parameters);

                var values = combinations.Select(x => x[i]).Distinct().ToList();

                if (values.Count > 1)
                {
                    values.Add(MorphConstants.UnknownTermId);
                }

                inputData.Properties[i].TermIds = values.OrderBy(x => x).ToList();
            }

            return OperationResult.SuccessResult();
        }

        #endregion
    }
}
